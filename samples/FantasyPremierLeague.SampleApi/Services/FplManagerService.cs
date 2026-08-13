using FantasyPremierLeague.Managers;
using FantasyPremierLeague.Models.Bootstrap;
using FantasyPremierLeague.Models.Leagues;
using FantasyPremierLeague.Models.Managers;
using FantasyPremierLeague.Models.Teams;

namespace FantasyPremierLeague.SampleApi.Services;

/// <summary>
/// Demonstrates how an application service can wrap the SDK.
/// </summary>
public sealed class FplManagerService : IFplManagerService
{
    private readonly FplClient _fplClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="FplManagerService"/> class.
    /// </summary>
    public FplManagerService(FplClient fplClient)
    {
        _fplClient = fplClient;
    }

    /// <inheritdoc />
    public async Task<FplManagerRecord?> LoginAsync(
        string email,
        string password,
        bool forceRefresh,
        bool includeDetails,
        CancellationToken cancellationToken = default)
    {
        var res = await _fplClient.LoginAsync(email, password, forceRefresh, includeDetails, cancellationToken);
        return res;
    }


    /// <inheritdoc />
    public Task<FplEntry> GetByEntryIdAsync(
        int entryId,
        CancellationToken cancellationToken = default) =>
        _fplClient.Managers.GetEntryAsync(entryId, cancellationToken);

    public Task<FplTeamPicks> GetMyTeamAsync(
   int entryId,
   CancellationToken cancellationToken = default) =>
   _fplClient.Managers.GetMyTeamAsync(entryId, cancellationToken);

    /// <inheritdoc />
    public Task<FplBootstrapStatic> GetBoostrapAsync(
        CancellationToken cancellationToken = default) =>
        _fplClient.Boostrap.GetDataAsync(cancellationToken);

    public Task<FplClassicLeague> GetClassicLeagueAsync(int leagueId, CancellationToken cancellationToken = default)
    {
        return _fplClient.Leagues.GetClassicStandingsAsync(leagueId, 1, cancellationToken);
    }

    public Task<FplH2HLeague> GetH2HLeagueAsync(int leagueId, CancellationToken cancellationToken = default)
    {
        return _fplClient.Leagues.GetH2HStandingsAsync(leagueId, 1, cancellationToken);
    }

    public async Task<FplLeague?> GetMyLeagueAsync(int managerId, CancellationToken cancellationToken = default)
    {
        return await _fplClient.Leagues.GetMyLeagueAsync(managerId, cancellationToken);
    }
}
