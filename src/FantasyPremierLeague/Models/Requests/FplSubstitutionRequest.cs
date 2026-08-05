using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Requests;

/// <summary>
/// Represents fpl substitution request.
/// </summary>
public sealed class FplSubstitutionRequest
{
    /// <summary>
    /// Gets or sets the chip.
    /// </summary>
    [JsonPropertyName("chip")]
    public string? Chip { get; set; }

    /// <summary>
    /// Gets or sets the picks.
    /// </summary>
    [JsonPropertyName("picks")]
    public List<FplSubstitutionPick> Picks { get; set; } = [];
}
