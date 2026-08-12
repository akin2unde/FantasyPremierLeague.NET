using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Players;

/// <summary>
/// Represents fpl live element info.
/// </summary>
public sealed class FplLiveElementInfo
{
    /// <summary>
    /// Gets or sets the stats.
    /// </summary>
    [JsonPropertyName("stats")]
    public FplElement? PlayerStats { get; set; }

    /// <summary>
    /// Gets or sets the singular name.
    /// </summary>
    [JsonPropertyName("explain")]
    public List<FplElement> Explain { get; set; } = [];

}
