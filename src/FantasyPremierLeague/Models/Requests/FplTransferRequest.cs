using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Requests;

/// <summary>
/// Represents fpl transfer request.
/// </summary>
public sealed class FplTransferRequest
{
    /// <summary>
    /// Gets or sets the chip.
    /// </summary>
    [JsonPropertyName("chip")]
    public string? Chip { get; set; }

    /// <summary>
    /// Gets or sets the entry.
    /// </summary>
    [JsonPropertyName("entry")]
    public int Entry { get; set; }

    /// <summary>
    /// Gets or sets the event.
    /// </summary>
    [JsonPropertyName("event")]
    public int Event { get; set; }

    /// <summary>
    /// Gets or sets the transfers.
    /// </summary>
    [JsonPropertyName("transfers")]
    public List<FplTransferItem> Transfers { get; set; } = [];
}
