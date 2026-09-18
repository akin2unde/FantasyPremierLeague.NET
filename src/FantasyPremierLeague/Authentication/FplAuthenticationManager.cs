using FantasyPremierLeague.Exceptions;
using FantasyPremierLeague.Managers;
using Microsoft.Extensions.Options;

namespace FantasyPremierLeague.Authentication;

/// <summary>
/// Coordinates FPL login, access-token reuse, refresh-token exchange,
/// profile loading, and manager-session persistence.
/// </summary>
internal sealed class FplAuthenticationManager : IFplAuthenticationManager
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILoadProfile _loadProfile;
    private readonly IFplLoginProvider _loginProvider;
    private readonly IFplManagerStore _managerStore;
    private readonly FplOptions _options;
    private readonly SemaphoreSlim _loginLock = new(1, 1);

    /// <inheritdoc />
    public FplManagerRecord? CurrentManager { get; set; }

    /// <summary>
    /// Initializes a new authentication manager.
    /// </summary>
    public FplAuthenticationManager(
        IFplLoginProvider loginProvider,
        IFplManagerStore managerStore,
        ILoadProfile loadProfile,
        IHttpClientFactory httpClientFactory,
        IOptions<FplOptions> options)
    {
        _loginProvider = loginProvider ?? throw new ArgumentNullException(nameof(loginProvider));
        _managerStore = managerStore ?? throw new ArgumentNullException(nameof(managerStore));
        _loadProfile = loadProfile ?? throw new ArgumentNullException(nameof(loadProfile));
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    /// <inheritdoc />
    public async Task<FplManagerRecord> LoginAsync(
        string email,
        string password,
        bool forceRefresh,
        bool includeDetails,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        email = email.Trim().ToLowerInvariant();

        await _loginLock.WaitAsync(cancellationToken);
        try
        {
            var saved = string.Equals(
                    CurrentManager?.Email,
                    email,
                    StringComparison.OrdinalIgnoreCase)
                ? CurrentManager
                : await _managerStore.GetByEmailAsync(email, cancellationToken);

            if (_options.ReuseStoredToken &&
                !forceRefresh &&
                saved is not null &&
                saved.HasUsableToken(_options.RefreshBeforeExpiry))
            {
                CurrentManager = saved;
                await LoadDetailsIfRequestedAsync(saved, includeDetails, cancellationToken);
                if (includeDetails)
                    await _managerStore.SaveAsync(saved, cancellationToken);
                return saved;
            }

            if (saved is not null && saved.HasUsableRefreshToken())
            {
                try
                {
                    if (includeDetails)
                        await RefreshManagerSessionAsync(saved, cancellationToken);
                    return saved;
                }
                catch (FplAuthenticationException) when (!string.IsNullOrWhiteSpace(password))
                {
                    // The refresh token may have been revoked before its JWT
                    // expiration. Fall back to the configured login provider
                    // when the caller supplied credentials.
                }
            }

            ArgumentException.ThrowIfNullOrWhiteSpace(password);

            var session = await _loginProvider.LoginAsync(
                email,
                password,
                cancellationToken);

            if (string.IsNullOrWhiteSpace(session.AccessToken))
            {
                throw new FplAuthenticationException(
                    "The login provider returned an empty access token.");
            }

            var record = saved ?? new FplManagerRecord
            {
                Email = email,
                AccessToken = session.AccessToken,
                Password = password
            };

            ApplySession(record, session);
            CurrentManager = record;
            await LoadDetailsIfRequestedAsync(record, includeDetails, cancellationToken);
            await _managerStore.SaveAsync(record, cancellationToken);
            return record;
        }
        finally
        {
            _loginLock.Release();
        }
    }

    /// <inheritdoc />
    public Task<string> GetCurrentAccessTokenAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(
            CurrentManager?.AccessToken ??
            throw new FplAuthenticationException(
                "No active manager session. Call LoginAsync or SetFoundRecord first."));
    }

    /// <inheritdoc />
    public async Task<string> RefreshCurrentAsync(CancellationToken cancellationToken)
    {
        var manager = CurrentManager ??
            throw new FplAuthenticationException(
                "No active manager session is available to refresh.");

        if (!manager.HasUsableRefreshToken())
        {
            throw new FplAuthenticationException(
                "The current manager does not have a usable refresh token. Log in again.");
        }

        await _loginLock.WaitAsync(cancellationToken);
        try
        {
            await RefreshManagerSessionAsync(manager, cancellationToken);
            return manager.AccessToken;
        }
        finally
        {
            _loginLock.Release();
        }
    }

    /// <inheritdoc />
    public async Task SaveCurrentAsync(
        FplManagerRecord manager,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(manager);
        CurrentManager = manager;
        await _managerStore.SaveAsync(manager, cancellationToken);
    }

    /// <inheritdoc />
    public Task InvalidateCurrentAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        CurrentManager = null;
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task LogoutAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        var normalizedEmail = email.Trim().ToLowerInvariant();
        await _managerStore.RemoveAsync(normalizedEmail, cancellationToken);

        if (string.Equals(CurrentManager?.Email, normalizedEmail, StringComparison.OrdinalIgnoreCase))
        {
            CurrentManager = null;
        }
    }

    private async Task RefreshManagerSessionAsync(
        FplManagerRecord manager,
        CancellationToken cancellationToken)
    {
        var session = await RefreshSessionAsync(
            new FplSession
            {
                AccessToken = manager.AccessToken,
                ExpiresAt = manager.TokenExpiresAt,
                RefreshToken = manager.RefreshToken,
                RefreshTokenExpiresAt = manager.RefreshTokenExpiresAt
            },
            cancellationToken);
        await LoadDetailsIfRequestedAsync(manager, true, cancellationToken);
        ApplySession(manager, session);
        CurrentManager = manager;
        await _managerStore.SaveAsync(manager, cancellationToken);
    }

    private async Task<FplSession> RefreshSessionAsync(
        FplSession currentSession,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(currentSession.RefreshToken))
        {
            throw new FplAuthenticationException(
                "The FPL session does not contain a refresh token.");
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, _options.TokenEndpoint)
        {
            Content = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    ["grant_type"] = "refresh_token",
                    ["refresh_token"] = currentSession.RefreshToken,
                    ["scope"] = _options.AuthenticationScope,
                    ["client_id"] = _options.AuthenticationClientId
                })
        };

        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            using var response = await httpClient.SendAsync(request, cancellationToken);
            var body = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var message =
                    $"FPL token refresh failed with HTTP {(int)response.StatusCode} " +
                    $"({response.ReasonPhrase}).";

                if (_options.ExposeDetailedErrors && !string.IsNullOrWhiteSpace(body))
                {
                    message += $" Body: {body}";
                }

                throw new FplAuthenticationException(message);
            }

            var refreshedSession = FplSessionParser.Parse(body, DateTimeOffset.UtcNow);

            if (string.IsNullOrWhiteSpace(refreshedSession.RefreshToken))
            {
                refreshedSession.RefreshToken = currentSession.RefreshToken;
                refreshedSession.RefreshTokenExpiresAt = currentSession.RefreshTokenExpiresAt;
            }
            else if (refreshedSession.RefreshTokenExpiresAt is null &&
                     string.Equals(
                         refreshedSession.RefreshToken,
                         currentSession.RefreshToken,
                         StringComparison.Ordinal))
            {
                refreshedSession.RefreshTokenExpiresAt =
                    currentSession.RefreshTokenExpiresAt;
            }

            return refreshedSession;
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (FplAuthenticationException) when (_options.ExposeDetailedErrors)
        {
            throw;
        }
        catch (Exception exception)
        {
            throw new FplAuthenticationException(
                "Unable to refresh the FPL session. Log in again if the refresh token is no longer valid.",
                exception);
        }
    }

    private async Task LoadDetailsIfRequestedAsync(
        FplManagerRecord manager,
        bool includeDetails,
        CancellationToken cancellationToken)
    {
        if (!includeDetails && !_options.LoadProfileAfterLogin)
        {
            return;
        }

        await _loadProfile.SetProfileAsync(manager, cancellationToken);
        // await _managerStore.SaveAsync(manager, cancellationToken);
    }

    private static void ApplySession(FplManagerRecord manager, FplSession session)
    {
        manager.AccessToken = session.AccessToken;
        manager.TokenExpiresAt = session.ExpiresAt;
        manager.RefreshToken = session.RefreshToken;
        manager.RefreshTokenExpiresAt = session.RefreshTokenExpiresAt;
        manager.UpdatedAt = DateTimeOffset.UtcNow;
        // manager.Password = null;
    }
}
