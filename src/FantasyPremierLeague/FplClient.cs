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
    /// Use this to set CurrentManager on AuthenticationManager.
    /// </summary>
    public void SetFoundRecord(FplManagerRecord fplManagerRecord)
    {
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
        Boostrap =
       fplBoostrap ??
       throw new ArgumentNullException(nameof(fplBoostrap));
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
    /// Gets boostrap static data.
    /// </summary>
    public FplBoostrapClient Boostrap { get; }

    /// <summary>
    /// Logs in a manager or reuses a valid stored token.
    /// </summary>
    public Task<FplManagerRecord> LoginAsync(
        string email,
        string password,
        bool forceRefresh,
        bool includeDetails,
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
    /// Forces a new login and replaces the stored token.
    /// </summary>
    public Task<FplManagerRecord> RefreshLoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        return _authenticationManager.LoginAsync(
            email,
            password,
            forceRefresh: true,
            includeDetails: true,
            cancellationToken);
    }

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