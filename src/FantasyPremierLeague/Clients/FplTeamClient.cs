using System.Text.Json;
using FantasyPremierLeague.Http;
using FantasyPremierLeague.Models.Requests;
using FantasyPremierLeague.Results;

namespace FantasyPremierLeague.Clients;
/// <summary>
/// Provides the FplTeamClient member.
/// </summary>

public sealed class FplTeamClient
{
    private readonly FplHttpClient _http;
    /// <summary>
    /// Initializes a new instance of the <see cref="FplTeamClient"/> class.
    /// </summary>
    /// <param name="http">
    /// The HTTP client used to communicate with Fantasy Premier League.
    /// </param>
    public FplTeamClient(FplHttpClient http) => _http = http;
    /// <summary>
    /// Provides the SubmitLineupAsync member.
    /// </summary>

    public async Task<FplOperationResult> SubmitLineupAsync(
        int entryId,
        FplSubstitutionRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(entryId);
        ArgumentNullException.ThrowIfNull(request);
        if (request.Picks.Count == 0) throw new ArgumentException("At least one pick is required.", nameof(request));

        var payload = await _http.PostAuthenticatedAsync<FplSubstitutionRequest, JsonElement>(
            string.Format(FplEndpoints.MyTeam, entryId), request, cancellationToken);
        return new FplOperationResult { Payload = payload };
    }
    /// <summary>
    /// Provides the SubmitTransfersAsync member.
    /// </summary>

    public async Task<FplOperationResult> SubmitTransfersAsync(
        FplTransferRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (request.Entry <= 0) throw new ArgumentOutOfRangeException(nameof(request.Entry));
        if (request.Event <= 0) throw new ArgumentOutOfRangeException(nameof(request.Event));
        if (request.Transfers.Count == 0) throw new ArgumentException("At least one transfer is required.", nameof(request));

        var payload = await _http.PostAuthenticatedAsync<FplTransferRequest, JsonElement>(
            FplEndpoints.Transfers, request, cancellationToken);
        return new FplOperationResult { Payload = payload };
    }
}
