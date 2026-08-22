using FantasyPremierLeague.Http;
using FantasyPremierLeague.Models.Bootstrap;
using FantasyPremierLeague.Models.Players;

namespace FantasyPremierLeague.Clients;

/// <summary>
/// Provides Fantasy Premier League player operations.
/// </summary>
public sealed class FplPlayersClient
{
    private readonly FplHttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="FplPlayersClient"/> class.
    /// </summary>
    /// <param name="httpClient">
    /// The HTTP client used to communicate with Fantasy Premier League.
    /// </param>
    public FplPlayersClient(FplHttpClient httpClient)
    {
        _httpClient = httpClient
            ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Gets the bootstrap data.
    /// </summary>
    public Task<FplBootstrapStatic> GetBootstrapAsync(
        CancellationToken cancellationToken = default)
    {
        return _httpClient.GetPublicAsync<FplBootstrapStatic>(
            "bootstrap-static/",
            cancellationToken);
    }

    /// <summary>
    /// Gets details for a player.
    /// </summary>
    public Task<FplElementSummary> GetPlayerSummaryAsync(
        int playerId,
        CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(playerId);

        return _httpClient.GetPublicAsync<FplElementSummary>(
            $"element-summary/{playerId}/",
            cancellationToken);
    }

    /// <summary>
    /// Gets details for a player live data.
    /// </summary>
    public Task<FplLiveElement> GetPlayerLiveAsync(
        int gw,
        CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(gw);

        return _httpClient.GetPublicAsync<FplLiveElement>(
            string.Format(FplEndpoints.PLayerLIVE, gw),
            cancellationToken);
    }

    /// <summary>
    /// Gets details for dream team in a particular gw.
    /// </summary>
    public Task<FplDreamTeam> GetGWDreamTeamAsync(
        int gw,
        CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(gw);

        return _httpClient.GetPublicAsync<FplDreamTeam>(
            string.Format(FplEndpoints.DreamTeam, gw),
            cancellationToken);
    }


}