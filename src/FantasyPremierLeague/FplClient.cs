using FantasyPremierLeague.Authentication;
using FantasyPremierLeague.Clients;
using FantasyPremierLeague.Managers;

namespace FantasyPremierLeague;

/// <summary>
/// Provides the main entry point for Fantasy Premier League operations.
/// </summary>
public sealed class FplClient
{
    private readonly IFplAuthenticationManager _authenticationManager;
    /// <summary>
    /// Selects a persisted manager record for subsequent authenticated operations.
    /// </summary>
    public void SetFoundRecord(FplManagerRecord fplManagerRecord)
    {
        ArgumentNullException.ThrowIfNull(fplManagerRecord);
        _authenticationManager.CurrentManager = fplManagerRecord;
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="FplClient"/> class.
    /// </summary>
    public FplClient(
        IFplAuthenticationManager authenticationManager,
        FplPlayersClient players,
        FplFixturesClient fixtures,
        FplManagersClient managers,
        FplLeaguesClient leagues,
        FplTeamClient team,
        FplBoostrapClient fplBoostrap

        )
    {
        _authenticationManager =
            authenticationManager ??
            throw new ArgumentNullException(nameof(authenticationManager));

        Players =
            players ??
            throw new ArgumentNullException(nameof(players));

        Fixtures =
            fixtures ??
            throw new ArgumentNullException(nameof(fixtures));

        Managers =
            managers ??
            throw new ArgumentNullException(nameof(managers));

        Leagues =
            leagues ??
            throw new ArgumentNullException(nameof(leagues));

        Team =
            team ??
            throw new ArgumentNullException(nameof(team));
        _legacyBootstrap = fplBoostrap ??
            throw new ArgumentNullException(nameof(fplBoostrap));
        Bootstrap = _legacyBootstrap.Inner;
    }

    /// <summary>
    /// Gets player operations.
    /// </summary>
    public FplPlayersClient Players { get; }

    /// <summary>
    /// Gets fixture operations.
    /// </summary>
    public FplFixturesClient Fixtures { get; }

    /// <summary>
    /// Gets manager operations.
    /// </summary>
    public FplManagersClient Managers { get; }

    /// <summary>
    /// Gets league operations.
    /// </summary>
    public FplLeaguesClient Leagues { get; }

    /// <summary>
    /// Gets authenticated team operations.
    /// </summary>
    public FplTeamClient Team { get; }

    /// <summary>
    /// Gets bootstrap-static operations.
    /// </summary>
    public FplBootstrapClient Bootstrap { get; }

    private readonly FplBoostrapClient _legacyBootstrap;

    /// <summary>
    /// Gets bootstrap-static operations.
    /// </summary>
    /// <remarks>Use <see cref="Bootstrap"/>. This alias is retained for compatibility.</remarks>
    [Obsolete("Use Bootstrap instead.")]
    public FplBoostrapClient Boostrap => _legacyBootstrap;

    /// <summary>
    /// Logs in a manager or reuses a valid stored token.
    /// </summary>
    public Task<FplManagerRecord> LoginAsync(
        string email,
        string password,
        bool forceRefresh = false,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return _authenticationManager.LoginAsync(
            email,
            password,
            forceRefresh: forceRefresh,
            includeDetails,
            cancellationToken);
    }

    /// <summary>
    /// Refreshes the active manager's access token using its stored refresh token.
    /// </summary>
    public Task<string> RefreshCurrentSessionAsync(
        CancellationToken cancellationToken = default) =>
        _authenticationManager.RefreshCurrentAsync(cancellationToken);



    /// <summary>
    /// Removes the stored manager authentication record.
    /// </summary>
    public Task LogoutAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return _authenticationManager.LogoutAsync(
            email,
            cancellationToken);
    }
}
