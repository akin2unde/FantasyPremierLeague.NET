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
/// Demonstrates how an application service can expose every SDK client while
/// restoring the appropriate persisted manager before authenticated calls.
/// </summary>
public sealed class FplManagerService : IFplManagerService
{
    private readonly FplClient _fplClient;
    private readonly IFplManagerStore _managerStore;

    /// <summary>Initializes the sample FPL application service.</summary>
    public FplManagerService(FplClient fplClient, IFplManagerStore managerStore)
    {
        _fplClient = fplClient ?? throw new ArgumentNullException(nameof(fplClient));
        _managerStore = managerStore ?? throw new ArgumentNullException(nameof(managerStore));
    }

    /// <inheritdoc />
    public Task<FplManagerRecord> LoginAsync(string email, string password, bool forceRefresh, bool includeDetails, CancellationToken cancellationToken = default) =>
        _fplClient.LoginAsync(email, password, forceRefresh, includeDetails, cancellationToken);

    /// <inheritdoc />
    public Task LogoutAsync(string email, CancellationToken cancellationToken = default) =>
        _fplClient.LogoutAsync(email, cancellationToken);

    /// <inheritdoc />
    public async Task<string> RefreshAsync(int entryId, CancellationToken cancellationToken = default)
    {
        await SelectManagerAsync(entryId, cancellationToken);
        return await _fplClient.RefreshCurrentSessionAsync(cancellationToken);
    }

    /// <inheritdoc />
    public Task<FplBootstrapStatic> GetBootstrapAsync(CancellationToken cancellationToken = default) =>
        _fplClient.Bootstrap.GetDataAsync(cancellationToken);

    /// <inheritdoc />
    public Task<FplElementSummary> GetPlayerSummaryAsync(int playerId, CancellationToken cancellationToken = default) =>
        _fplClient.Players.GetPlayerSummaryAsync(playerId, cancellationToken);

    /// <inheritdoc />
    public Task<FplLiveElement> GetLivePlayerDataAsync(int gameweek, CancellationToken cancellationToken = default) =>
        _fplClient.Players.GetPlayerLiveAsync(gameweek, cancellationToken);

    /// <inheritdoc />
    public Task<FplDreamTeam> GetDreamTeamAsync(int gameweek, CancellationToken cancellationToken = default) =>
        _fplClient.Players.GetGWDreamTeamAsync(gameweek, cancellationToken);

    /// <inheritdoc />
    public Task<List<FplFixture>> GetFixturesAsync(CancellationToken cancellationToken = default) =>
        _fplClient.Fixtures.GetAllAsync(cancellationToken);

    /// <inheritdoc />
    public Task<List<FplFixture>> GetFixturesByGameweekAsync(int gameweek, CancellationToken cancellationToken = default) =>
        _fplClient.Fixtures.GetByGWAsync(gameweek, cancellationToken);

    /// <inheritdoc />
    public Task<FplFixture?> GetFixtureByCodeAsync(int code, CancellationToken cancellationToken = default) =>
        _fplClient.Fixtures.GetByCodeAsync(code, cancellationToken);

    /// <inheritdoc />
    public Task<FplEntry> GetEntryAsync(int entryId, CancellationToken cancellationToken = default) =>
        _fplClient.Managers.GetEntryAsync(entryId, cancellationToken);

    /// <inheritdoc />
    public Task<FplTeamPicks> GetPicksAsync(int entryId, int gameweek, CancellationToken cancellationToken = default) =>
        _fplClient.Managers.GetPicksAsync(entryId, gameweek, cancellationToken);

    /// <inheritdoc />
    public Task<List<FplEntryTransferHistory>> GetTransferHistoryAsync(int entryId, CancellationToken cancellationToken = default) =>
        _fplClient.Managers.GetManagerTransferHistoryAsync(entryId, cancellationToken);

    /// <inheritdoc />
    public async Task<FplTeamPicks> GetMyTeamAsync(int entryId, CancellationToken cancellationToken = default)
    {
        await SelectManagerAsync(entryId, cancellationToken);
        return await _fplClient.Managers.GetMyTeamAsync(entryId, cancellationToken);
    }

    /// <inheritdoc />
    public Task<FplEntryHistoryReponse> GetSeasonHistoryAsync(int entryId, CancellationToken cancellationToken = default) =>
        _fplClient.Managers.GetMyGWHistoryAsync(entryId, cancellationToken);

    /// <inheritdoc />
    public async Task<FplMe> GetCurrentManagerAsync(int entryId, CancellationToken cancellationToken = default)
    {
        await SelectManagerAsync(entryId, cancellationToken);
        return await _fplClient.Managers.GetCurrentAsync(cancellationToken);
    }

    /// <inheritdoc />
    public Task<FplLeague?> GetManagerLeaguesAsync(int entryId, CancellationToken cancellationToken = default) =>
        _fplClient.Leagues.GetMyLeagueAsync(entryId, cancellationToken);

    /// <inheritdoc />
    public Task<FplClassicLeague> GetClassicLeagueAsync(int leagueId, int page, CancellationToken cancellationToken = default) =>
        _fplClient.Leagues.GetClassicStandingsAsync(leagueId, page, cancellationToken);

    /// <inheritdoc />
    public Task<FplH2HLeague> GetH2HLeagueAsync(int leagueId, int page, CancellationToken cancellationToken = default) =>
        _fplClient.Leagues.GetH2HStandingsAsync(leagueId, page, cancellationToken);

    /// <inheritdoc />
    public Task<FplH2HMatchesResponse> GetH2HMatchesAsync(int leagueId, int gameweek, int page, CancellationToken cancellationToken = default) =>
        _fplClient.Leagues.GetH2HFixtureAsync(leagueId, gameweek, page, cancellationToken);

    /// <inheritdoc />
    public async Task<FplOperationResult> SubmitLineupAsync(int entryId, FplSubstitutionRequest request, CancellationToken cancellationToken = default)
    {
        await SelectManagerAsync(entryId, cancellationToken);
        return await _fplClient.Team.SubmitLineupAsync(entryId, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<FplOperationResult> SubmitTransfersAsync(FplTransferRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        await SelectManagerAsync(request.Entry, cancellationToken);
        return await _fplClient.Team.SubmitTransfersAsync(request, cancellationToken);
    }

    private async Task SelectManagerAsync(int entryId, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(entryId);
        var manager = await _managerStore.GetByEntryIdAsync(entryId, cancellationToken) ??
            throw new KeyNotFoundException(
                $"No authenticated manager session was found for entry {entryId}. Log in first.");

        _fplClient.SetFoundRecord(manager);
    }
}
