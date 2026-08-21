using FantasyPremierLeague.Managers;
using FantasyPremierLeague.Models.Bootstrap;
using FantasyPremierLeague.Models.Leagues;
using FantasyPremierLeague.Models.Managers;
using FantasyPremierLeague.Models.Players;
using FantasyPremierLeague.Models.Teams;

namespace FantasyPremierLeague.SampleApi.Services;

/// <summary>
/// Defines application-level operations for FPL managers.
/// </summary>
public interface IFplManagerService
{
    /// <summary>
    /// Authenticates a manager or restores a valid persisted session.
    /// </summary>
    Task<FplManagerRecord?> LoginAsync(
        string email,
        string password,
        bool forceRefresh,
        bool includeDetails,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a persisted manager by entry identifier.
    /// </summary>
    Task<FplEntry> GetByEntryIdAsync(
        int entryId,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets a persisted manager by entry identifier.
    /// </summary>
    Task<FplTeamPicks> GetMyTeamAsync(
        int entryId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets boosrap static file.
    /// </summary>
    Task<FplBootstrapStatic> GetBoostrapAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a persisted manager by entry identifier.
    /// </summary>
    Task<FplClassicLeague> GetClassicLeagueAsync(
        int leagueId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a persisted manager by entry identifier.
    /// </summary>
    Task<FplH2HLeague> GetH2HLeagueAsync(
        int leagueId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a persisted manager by entry identifier.
    /// </summary>
    Task<FplLeague?> GetMyLeagueAsync(
        int managerId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a live player data.
    /// </summary>
    Task<FplLiveElement?> GetLivePlayerDataAsync(
        int gw,
        CancellationToken cancellationToken = default);
}
