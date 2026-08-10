using FantasyPremierLeague.Http;
using FantasyPremierLeague.Models.Bootstrap;
using FantasyPremierLeague.Models.Fixtures;

namespace FantasyPremierLeague.Clients;
/// <summary>
/// Provides the FplBoostrapClient .
/// </summary>

public sealed class FplBoostrapClient
{
    private readonly FplHttpClient _http;
    /// <summary>
    /// Initializes a new instance of the <see cref="FplFixturesClient"/> class.
    /// </summary>
    /// <param name="http">
    /// The HTTP client used to communicate with Fantasy Premier League.
    /// </param>
    public FplBoostrapClient(FplHttpClient http) => _http = http;
    /// <summary>
    /// Provides the GetDataAsync member.
    /// </summary>

    public Task<FplBootstrapStatic> GetDataAsync(CancellationToken cancellationToken = default)
    {
        return _http.GetPublicAsync<FplBootstrapStatic>(FplEndpoints.Bootstrap, cancellationToken);
    }
}
