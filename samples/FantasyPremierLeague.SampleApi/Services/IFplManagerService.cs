using FantasyPremierLeague.Managers;
using FantasyPremierLeague.Models.Bootstrap;
using FantasyPremierLeague.Models.Fixtures;
using FantasyPremierLeague.Models.Leagues;
using FantasyPremierLeague.Models.Managers;
using FantasyPremierLeague.Models.Players;
using FantasyPremierLeague.Models.Requests;
using FantasyPremierLeague.Models.Teams;
using FantasyPremierLeague.Results;

namespace FantasyPremierLeague.SampleApi.Services;

/// <summary>
/// Defines the complete set of SDK operations demonstrated by the sample API.
/// </summary>
public interface IFplManagerService
{
    /// <summary>Authenticates a manager or reuses/refreshes a stored session.</summary>
    Task<FplManagerRecord> LoginAsync(string email, string password, bool forceRefresh, bool includeDetails, CancellationToken cancellationToken = default);

    /// <summary>Removes a stored manager session.</summary>
    Task LogoutAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>Refreshes the active session associated with an entry.</summary>
    Task<string> RefreshAsync(int entryId, CancellationToken cancellationToken = default);

    /// <summary>Gets bootstrap-static data.</summary>
    Task<FplBootstrapStatic> GetBootstrapAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets a player's season summary and fixtures.</summary>
    Task<FplElementSummary> GetPlayerSummaryAsync(int playerId, CancellationToken cancellationToken = default);

    /// <summary>Gets live player data for a gameweek.</summary>
    Task<FplLiveElement> GetLivePlayerDataAsync(int gameweek, CancellationToken cancellationToken = default);

    /// <summary>Gets the dream team for a gameweek.</summary>
    Task<FplDreamTeam> GetDreamTeamAsync(int gameweek, CancellationToken cancellationToken = default);

    /// <summary>Gets all fixtures.</summary>
    Task<List<FplFixture>> GetFixturesAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets fixtures for a gameweek.</summary>
    Task<List<FplFixture>> GetFixturesByGameweekAsync(int gameweek, CancellationToken cancellationToken = default);

    /// <summary>Finds a fixture by its FPL fixture code.</summary>
    Task<FplFixture?> GetFixtureByCodeAsync(int code, CancellationToken cancellationToken = default);

    /// <summary>Gets a public manager entry.</summary>
    Task<FplEntry> GetEntryAsync(int entryId, CancellationToken cancellationToken = default);

    /// <summary>Gets a manager's picks for a gameweek.</summary>
    Task<FplTeamPicks> GetPicksAsync(int entryId, int gameweek, CancellationToken cancellationToken = default);

    /// <summary>Gets a manager's transfer history.</summary>
    Task<List<FplEntryTransferHistory>> GetTransferHistoryAsync(int entryId, CancellationToken cancellationToken = default);

    /// <summary>Gets a manager's current authenticated team.</summary>
    Task<FplTeamPicks> GetMyTeamAsync(int entryId, CancellationToken cancellationToken = default);

    /// <summary>Gets a manager's current-season and past-season history.</summary>
    Task<FplEntryHistoryReponse> GetSeasonHistoryAsync(int entryId, CancellationToken cancellationToken = default);

    /// <summary>Gets the authenticated manager profile.</summary>
    Task<FplMe> GetCurrentManagerAsync(int entryId, CancellationToken cancellationToken = default);

    /// <summary>Gets the leagues attached to a manager entry.</summary>
    Task<FplLeague?> GetManagerLeaguesAsync(int entryId, CancellationToken cancellationToken = default);

    /// <summary>Gets classic-league standings.</summary>
    Task<FplClassicLeague> GetClassicLeagueAsync(int leagueId, int page, CancellationToken cancellationToken = default);

    /// <summary>Gets head-to-head league standings.</summary>
    Task<FplH2HLeague> GetH2HLeagueAsync(int leagueId, int page, CancellationToken cancellationToken = default);

    /// <summary>Gets head-to-head matches for a league and gameweek.</summary>
    Task<FplH2HMatchesResponse> GetH2HMatchesAsync(int leagueId, int gameweek, int page, CancellationToken cancellationToken = default);

    /// <summary>Submits a manager's lineup and substitutions.</summary>
    Task<FplOperationResult> SubmitLineupAsync(int entryId, FplSubstitutionRequest request, CancellationToken cancellationToken = default);

    /// <summary>Submits transfers for a manager.</summary>
    Task<FplOperationResult> SubmitTransfersAsync(FplTransferRequest request, CancellationToken cancellationToken = default);
}
