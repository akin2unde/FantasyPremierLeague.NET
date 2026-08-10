using FantasyPremierLeague.Exceptions;
using FantasyPremierLeague.Managers;
using Microsoft.Extensions.Options;
namespace FantasyPremierLeague.Authentication;

internal sealed class FplAuthenticationManager : IFplAuthenticationManager
{
    private readonly IFplLoginProvider _loginProvider;
    private readonly IFplManagerStore _managerStore;
    private readonly FplOptions _options;
    private readonly SemaphoreSlim _loginLock = new(1, 1);
    private string? _currentPassword;
    /// <summary>
    /// Provides the member member.
    /// </summary>
    public FplManagerRecord? CurrentManager { get; set; }
    /// <summary>
    /// Describes the FplAuthenticationManager member.
    /// </summary>
    public FplAuthenticationManager(IFplLoginProvider loginProvider, IFplManagerStore managerStore, IOptions<FplOptions> options)
    {
        _loginProvider = loginProvider;
        _managerStore = managerStore;
        _options = options.Value;
    }
    /// <summary>
    /// Describes the LoginAsync member.
    /// </summary>
    public async Task<FplManagerRecord> LoginAsync(string email, string password, bool forceRefresh, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email); ArgumentException.ThrowIfNullOrWhiteSpace(password);
        email = email.Trim().ToLowerInvariant(); await _loginLock.WaitAsync(cancellationToken);
        try
        {
            var saved = CurrentManager ?? await _managerStore.GetByEmailAsync(email, cancellationToken);
            if (!forceRefresh && saved is not null && saved.HasUsableToken(_options.RefreshBeforeExpiry))
            {
                CurrentManager ??= saved;
                _currentPassword = password;
                return saved;
            }
            var session = await _loginProvider.LoginAsync(email, password, cancellationToken);
            if (string.IsNullOrWhiteSpace(session.AccessToken)) throw new FplAuthenticationException("The login provider returned an empty access token.");
            var record = saved ?? new FplManagerRecord { Email = email, AccessToken = session.AccessToken };
            record.AccessToken = session.AccessToken;
            record.TokenExpiresAt = session.ExpiresAt;
            record.RefreshToken = session.RefreshToken;
            record.UpdatedAt = DateTimeOffset.UtcNow;
            await _managerStore.SaveAsync(record, cancellationToken);
            CurrentManager = record; _currentPassword = password;
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
        if (CurrentManager is null || _currentPassword is null) throw new FplAuthenticationException("The current session cannot be refreshed because credentials are unavailable.");
        var refreshed = await LoginAsync(CurrentManager.Email, _currentPassword, true, cancellationToken); return refreshed.AccessToken;
    }
    /// <summary>
    /// Provides the SaveCurrentAsync member.
    /// </summary>
    public async Task SaveCurrentAsync(FplManagerRecord manager, CancellationToken cancellationToken)
    { CurrentManager = manager; await _managerStore.SaveAsync(manager, cancellationToken); }
    /// <summary>
    /// Describes the InvalidateCurrentAsync member.
    /// </summary>
    public Task InvalidateCurrentAsync(CancellationToken cancellationToken)
    { CurrentManager = null; _currentPassword = null; return Task.CompletedTask; }

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
