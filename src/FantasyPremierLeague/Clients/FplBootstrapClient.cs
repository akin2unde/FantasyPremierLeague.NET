using FantasyPremierLeague.Http;
using FantasyPremierLeague.Models.Bootstrap;

namespace FantasyPremierLeague.Clients;

/// <summary>
/// Provides access to FPL bootstrap-static data.
/// </summary>
public sealed class FplBootstrapClient
{
    private readonly FplHttpClient _httpClient;

    /// <summary>Initializes a new bootstrap client.</summary>
    public FplBootstrapClient(FplHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Gets the current gameweeks, teams, players, positions, chips, phases,
    /// and other global FPL metadata.
    /// </summary>
    public Task<FplBootstrapStatic> GetDataAsync(
        CancellationToken cancellationToken = default) =>
        _httpClient.GetPublicAsync<FplBootstrapStatic>(
            FplEndpoints.Bootstrap,
            cancellationToken);
}
