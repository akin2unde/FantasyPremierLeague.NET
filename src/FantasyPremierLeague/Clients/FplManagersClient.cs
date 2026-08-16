using FantasyPremierLeague.Http;
using FantasyPremierLeague.Models.Managers;
using FantasyPremierLeague.Models.Teams;

namespace FantasyPremierLeague.Clients;
/// <summary>
/// Provides the FplManagersClient member.
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
    /// Provides the GetEntryAsync member.
    /// </summary>

    public Task<FplEntry> GetEntryAsync(int entryId, CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(entryId);
        return _http.GetPublicAsync<FplEntry>(string.Format(FplEndpoints.Entry, entryId), cancellationToken);
    }
    /// <summary>
    /// Provides the GetPicksAsync member.
    /// </summary>

    public Task<FplTeamPicks> GetPicksAsync(int entryId, int gameweek, CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(entryId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(gameweek);
        return _http.GetPublicAsync<FplTeamPicks>(
            string.Format(FplEndpoints.EntryPicks, entryId, gameweek), cancellationToken);
    }


    /// <summary>
    /// Provides the GetMyTeamsAsync member.
    /// </summary>
    public Task<FplTeamPicks> GetMyTeamAsync(int entryId, CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(entryId);
        return _http.GetAuthenticatedAsync<FplTeamPicks>(
            string.Format(FplEndpoints.Myteam, entryId), cancellationToken);
    }



    /// <summary>
    /// Provides the GetCurrentAsync member.
    /// </summary>

    public Task<FplMe> GetCurrentAsync(CancellationToken cancellationToken = default) =>
        _http.GetAuthenticatedAsync<FplMe>(FplEndpoints.Me, cancellationToken);
}
