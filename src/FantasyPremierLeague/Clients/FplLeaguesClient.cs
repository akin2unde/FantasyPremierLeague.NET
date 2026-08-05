using FantasyPremierLeague.Http;
using FantasyPremierLeague.Models.Leagues;

namespace FantasyPremierLeague.Clients;
/// <summary>
/// Provides the FplLeaguesClient member.
/// </summary>

public sealed class FplLeaguesClient
{
    private readonly FplHttpClient _http;
    /// <summary>
    /// Initializes a new instance of the <see cref="FplLeaguesClient"/> class.
    /// </summary>
    /// <param name="http">
    /// The HTTP client used to communicate with Fantasy Premier League.
    /// </param>
    public FplLeaguesClient(FplHttpClient http) => _http = http;
    /// <summary>
    /// Provides the GetClassicStandingsAsync member.
    /// </summary>

    public Task<FplClassicLeagueStandings> GetClassicStandingsAsync(
        int leagueId,
        int page = 1,
        CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(leagueId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(page);
        return _http.GetPublicAsync<FplClassicLeagueStandings>(
            string.Format(FplEndpoints.ClassicLeague, leagueId, page), cancellationToken);
    }
}
