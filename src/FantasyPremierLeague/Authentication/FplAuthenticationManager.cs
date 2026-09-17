using FantasyPremierLeague.Exceptions;
using FantasyPremierLeague.Managers;
using Microsoft.Extensions.Options;
namespace FantasyPremierLeague.Authentication;

internal sealed class FplAuthenticationManager : IFplAuthenticationManager
{
    private readonly ILoadProfile _loadProfile;
    private readonly IFplLoginProvider _loginProvider;
    private readonly IFplManagerStore _managerStore;
    private readonly FplOptions _options;
    private readonly SemaphoreSlim _loginLock = new(1, 1);
    /// <summary>
    /// Provides the member member.
    /// </summary>
    public FplManagerRecord? CurrentManager { get; set; }
    /// <summary>
    /// Describes the FplAuthenticationManager member.
    /// </summary>
    public FplAuthenticationManager(IFplLoginProvider loginProvider, IFplManagerStore managerStore, ILoadProfile loadProfile, IOptions<FplOptions> options)
    {
        _loginProvider = loginProvider;
        _managerStore = managerStore;
        _loadProfile = loadProfile;
        _options = options.Value;
    }
    // public async Task<FplManagerRecord> SaveManagerAsync(
    //   FplManagerRecord manager,
    //   CancellationToken cancellationToken = default)
    // {
    //     ArgumentNullException.ThrowIfNull(manager);

    //     if (manager.EntryId <= 0)
    //     {
    //         throw new InvalidOperationException(
    //             "Manager entry ID must be available before loading profile data.");
    //     }

    //     var profile = await _fplClient.Managers.GetProfileAsync(
    //         manager.EntryId,
    //         cancellationToken);

    //     var entry = await _fplClient.Managers.GetEntryAsync(
    //         manager.EntryId,
    //         cancellationToken);

    //     manager.Profile = profile;
    //     manager.Entry = entry;

    //     await _managerStore.SaveAsync(
    //         manager,
    //         cancellationToken);

    //     return manager;
    // }
    private const string TokenEndpoint =
    "https://account.premierleague.com/as/token";

    private const string ClientId =
        "bfcbaf69-aade-4c1b-8f00-c1cb8a193030";

    public static async Task<FplSession> RefreshSessionAsync(
        FplSession currentSession,
        CancellationToken cancellationToken = default)
    {
        HttpClient httpClient = new();

        if (string.IsNullOrWhiteSpace(currentSession.RefreshToken))
        {
            throw new FplAuthenticationException(
                "The FPL session does not contain a refresh token.");
        }

        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            TokenEndpoint);

        request.Content = new FormUrlEncodedContent(
            new Dictionary<string, string>
            {
                ["grant_type"] = "refresh_token",
                ["refresh_token"] = currentSession.RefreshToken,
                ["scope"] = "openid profile email",
                ["client_id"] = ClientId
            });
        try
        {
            using var response = await httpClient.SendAsync(
                request,
                cancellationToken);

            var body = await response.Content.ReadAsStringAsync(
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new FplAuthenticationException(
                    $"FPL token refresh failed. " +
                    $"Status: {(int)response.StatusCode}. " +
                    $"Response: {body}");
            }

            var refreshedSession = FplSessionParser.Parse(
                body,
                DateTimeOffset.UtcNow);

            // Keep the previous refresh token only when FPL does not return a new one.
            refreshedSession.RefreshToken =
                string.IsNullOrWhiteSpace(refreshedSession.RefreshToken)
                    ? currentSession.RefreshToken
                    : refreshedSession.RefreshToken;

            return refreshedSession;
        }
        catch (Exception)
        {
            throw new Exception("Unable to refresh");
        }
    }
    /// <summary>
    /// Describes the LoginAsync member.
    /// </summary>

    public async Task<FplManagerRecord> LoginAsync(string email, string password, bool forceRefresh, bool includeDetails, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        email = email.Trim().ToLowerInvariant();
        await _loginLock.WaitAsync(cancellationToken);
        try
        {
            var saved = CurrentManager ?? await _managerStore.GetByEmailAsync(email, cancellationToken);
            if (!forceRefresh && saved is not null && saved.HasUsableToken(_options.RefreshBeforeExpiry))
            {
                CurrentManager ??= saved;
                return saved;
            }
            if (saved is not null && saved.HasUsableRefreshToken())
            {
                //refresh token 
                var refresh = await RefreshSessionAsync(new FplSession { AccessToken = saved.AccessToken, RefreshToken = saved.RefreshToken, RefreshTokenExpiresAt = saved.RefreshTokenExpiresAt }, cancellationToken);
                saved.AccessToken = refresh.AccessToken;
                saved.RefreshToken = refresh.RefreshToken;
                saved.TokenExpiresAt = refresh.ExpiresAt;
                saved.RefreshTokenExpiresAt = refresh.RefreshTokenExpiresAt;
                CurrentManager ??= saved;
                return saved;
            }
            ArgumentException.ThrowIfNullOrWhiteSpace(password);
            var session = await _loginProvider.LoginAsync(email, password, cancellationToken);
            if (string.IsNullOrWhiteSpace(session.AccessToken)) throw new FplAuthenticationException("The login provider returned an empty access token.");
            var record = saved ?? new FplManagerRecord { Email = email, AccessToken = session.AccessToken };
            record.AccessToken = session.AccessToken;
            record.TokenExpiresAt = session.ExpiresAt;
            record.RefreshToken = session.RefreshToken;
            record.UpdatedAt = DateTimeOffset.UtcNow;
            record.Password = password;
            record.RefreshTokenExpiresAt = session.RefreshTokenExpiresAt;
            if (_options.LoadProfileAfterLogin)
            {
                await _loadProfile.SetProfileAsync(record, cancellationToken);
            }
            await _managerStore.SaveAsync(record, cancellationToken);
            CurrentManager = record;
            //remove the password
            record.Password = string.Empty;
            return record;
        }
        finally { _loginLock.Release(); }
    }
    /// <summary>
    /// Provides the GetCurrentAccessTokenAsync member.
    /// </summary>
    public Task<string> GetCurrentAccessTokenAsync(CancellationToken cancellationToken)
    { cancellationToken.ThrowIfCancellationRequested(); return Task.FromResult(CurrentManager?.AccessToken ?? throw new FplAuthenticationException("No active manager session. Call LoginAsync first.")); }
    /// <summary>
    /// Describes the RefreshCurrentAsync member.
    /// </summary>
    public async Task<string> RefreshCurrentAsync(CancellationToken cancellationToken)
    {
        if (CurrentManager is null || CurrentManager.Password is null) throw new FplAuthenticationException("The current session cannot be refreshed because credentials are unavailable.");
        var refreshed = await LoginAsync(CurrentManager.Email, CurrentManager.Password, true, true, cancellationToken); return refreshed.AccessToken;
    }
    /// <summary>
    /// Provides the SaveCurrentAsync member.
    /// </summary>
    public async Task SaveCurrentAsync(FplManagerRecord manager, CancellationToken cancellationToken)
    {
        CurrentManager = manager; await _managerStore.SaveAsync(manager, cancellationToken);
    }
    /// <summary>
    /// Describes the InvalidateCurrentAsync member.
    /// </summary>
    public Task InvalidateCurrentAsync(CancellationToken cancellationToken)
    {
        CurrentManager = null;
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task LogoutAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        return _managerStore.RemoveAsync(
            email.Trim().ToLowerInvariant(),
            cancellationToken);
    }
}
