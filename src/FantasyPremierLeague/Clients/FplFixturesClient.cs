using FantasyPremierLeague.Http;
using FantasyPremierLeague.Models.Fixtures;

namespace FantasyPremierLeague.Clients;
/// <summary>
/// Provides the FplFixturesClient member.
/// </summary>

public sealed class FplFixturesClient
{
    private readonly FplHttpClient _http;
    /// <summary>
    /// Initializes a new instance of the <see cref="FplFixturesClient"/> class.
    /// </summary>
    /// <param name="http">
    /// The HTTP client used to communicate with Fantasy Premier League.
    /// </param>
    public FplFixturesClient(FplHttpClient http) => _http = http;
    /// <summary>
    /// Provides the GetAllAsync member.
    /// </summary>

    public Task<List<FplFixture>> GetAllAsync(int? gameweek = null, CancellationToken cancellationToken = default)
    {
        if (gameweek is <= 0) throw new ArgumentOutOfRangeException(nameof(gameweek));
        var path = gameweek is null ? FplEndpoints.Fixtures : string.Format(FplEndpoints.FixturesByEvent, gameweek);
        return _http.GetPublicAsync<List<FplFixture>>(path, cancellationToken);
    }
}
