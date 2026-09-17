using FantasyPremierLeague.Http;
using FantasyPremierLeague.Models.Managers;
using FantasyPremierLeague.Models.Teams;

namespace FantasyPremierLeague.Clients;
/// <summary>
/// Provides public and authenticated FPL manager operations.
/// </summary>

public sealed class FplManagersClient
{
    private readonly FplHttpClient _http;
    /// <summary>
    /// Initializes a new instance of the <see cref="FplManagersClient"/> class.
    /// </summary>
    /// <param name="http">
    /// The HTTP client used to communicate with Fantasy Premier League.
    /// </param>
    public FplManagersClient(FplHttpClient http) => _http = http;
    /// <summary>
    /// Gets the public summary for a manager entry.
    /// </summary>

    public Task<FplEntry> GetEntryAsync(int entryId, CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(entryId);
        return _http.GetPublicAsync<FplEntry>(string.Format(FplEndpoints.Entry, entryId), cancellationToken);
    }
    /// <summary>
    /// Gets a manager's picks for a gameweek.
    /// </summary>

    public Task<FplTeamPicks> GetPicksAsync(int entryId, int gameweek, CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(entryId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(gameweek);
        return _http.GetPublicAsync<FplTeamPicks>(
            string.Format(FplEndpoints.EntryPicks, entryId, gameweek), cancellationToken);
    }

    /// <summary>
    /// Gets a manager's transfer history.
    /// </summary>

    public Task<List<FplEntryTransferHistory>> GetManagerTransferHistoryAsync(int entryId, CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(entryId);
        return _http.GetPublicAsync<List<FplEntryTransferHistory>>(
            string.Format(FplEndpoints.TransferHistory, entryId), cancellationToken);
    }


    /// <summary>
    /// Gets the authenticated manager's current team.
    /// </summary>
    public Task<FplTeamPicks> GetMyTeamAsync(int entryId, CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(entryId);
        return _http.GetAuthenticatedAsync<FplTeamPicks>(
            string.Format(FplEndpoints.Myteam, entryId), cancellationToken);
    }

    /// <summary>
    /// Gets a manager's current-season and past-season history.
    /// </summary>
    public Task<FplEntryHistoryReponse> GetMyGWHistoryAsync(int entryId, CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(entryId);
        return _http.GetPublicAsync<FplEntryHistoryReponse>(
            string.Format(FplEndpoints.MyGWHistory, entryId), cancellationToken);
    }


    /// <summary>
    /// Gets the authenticated manager's account profile.
    /// </summary>

    public Task<FplMe> GetCurrentAsync(CancellationToken cancellationToken = default) =>
        _http.GetAuthenticatedAsync<FplMe>(FplEndpoints.Me, cancellationToken);
}
