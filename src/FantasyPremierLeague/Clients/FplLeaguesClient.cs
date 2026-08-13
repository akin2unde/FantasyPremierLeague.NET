using FantasyPremierLeague.Http;
using FantasyPremierLeague.Models.Leagues;

namespace FantasyPremierLeague.Clients;
/// <summary>
/// Provides the FplLeaguesClient member.
/// </summary>

public sealed class FplLeaguesClient
{
    private readonly FplHttpClient _http;
    private readonly FplManagersClient _fplManagersClient;
    /// <summary>
    /// Initializes a new instance of the <see cref="FplLeaguesClient"/> class.
    /// </summary>
    /// <param name="http">
    /// The HTTP client used to communicate with Fantasy Premier League.
    /// </param>

    ///  /// <summary>
    /// Initializes a new instance of the <see cref="FplLeaguesClient"/> class.
    /// </summary>
    /// <param name="fplManagersClient">
    /// The manager client used to communicate with Fantasy Premier League.
    /// </param>
    public FplLeaguesClient(FplHttpClient http, FplManagersClient fplManagersClient)
    {
        _http = http;
        _fplManagersClient = fplManagersClient;
    }
    /// <summary>
    /// Provides the GetClassicStandingsAsync member.
    /// </summary>

    public Task<FplClassicLeague> GetClassicStandingsAsync(
        int leagueId,
        int page = 1,
        CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(leagueId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(page);
        return _http.GetPublicAsync<FplClassicLeague>(
            string.Format(FplEndpoints.ClassicLeague, leagueId, page), cancellationToken);
    }

    /// <summary>
    /// Provides the GetH2HStandingsAsync member.
    /// </summary>

    public Task<FplH2HLeague> GetH2HStandingsAsync(
        int leagueId,
        int page = 1,
        CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(leagueId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(page);
        return _http.GetPublicAsync<FplH2HLeague>(
            string.Format(FplEndpoints.H2HLeague, leagueId, page), cancellationToken);
    }


    /// <summary>
    /// Provides the GetMyLeagueAsync member.
    /// </summary>

    public async Task<FplLeague?> GetMyLeagueAsync(
        int entry,
        CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(entry);
        var resp = await _fplManagersClient.GetEntryAsync(entry, cancellationToken);
        return resp.Leagues;
    }
}
