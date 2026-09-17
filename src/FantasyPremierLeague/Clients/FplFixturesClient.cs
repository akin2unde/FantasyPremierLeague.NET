using FantasyPremierLeague.Http;
using FantasyPremierLeague.Models.Fixtures;

namespace FantasyPremierLeague.Clients;
/// <summary>
/// Provides public FPL fixture operations.
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
    /// Gets all fixtures for the current season.
    /// </summary>

    public Task<List<FplFixture>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _http.GetPublicAsync<List<FplFixture>>(FplEndpoints.Fixtures, cancellationToken);
    }
    /// <summary>
    /// Gets fixtures scheduled for a gameweek.
    /// </summary>

    public Task<List<FplFixture>> GetByGWAsync(int gameweek, CancellationToken cancellationToken = default)
    {
        if (gameweek is <= 0) throw new ArgumentOutOfRangeException(nameof(gameweek));
        var path = string.Format(FplEndpoints.FixturesByEvent, gameweek);
        return _http.GetPublicAsync<List<FplFixture>>(path, cancellationToken);
    }
    /// <summary>
    /// Finds a fixture using its FPL fixture code.
    /// </summary>

    public async Task<FplFixture?> GetByCodeAsync(int code, CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(code);
        var all = await GetAllAsync(cancellationToken);
        return all.FirstOrDefault(_ => _.FixtureCode == code);
    }
}
