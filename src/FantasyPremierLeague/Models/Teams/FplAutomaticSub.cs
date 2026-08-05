using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Teams;

/// <summary>
/// Represents fpl automatic sub.
/// </summary>
public sealed class FplAutomaticSub
{
    /// <summary>
    /// Gets or sets the entry.
    /// </summary>
    [JsonPropertyName("entry")]
    public int Entry { get; set; }

    /// <summary>
    /// Gets or sets the element in.
    /// </summary>
    [JsonPropertyName("element_in")]
    public int ElementIn { get; set; }

    /// <summary>
    /// Gets or sets the element out.
    /// </summary>
    [JsonPropertyName("element_out")]
    public int ElementOut { get; set; }

    /// <summary>
    /// Gets or sets the event.
    /// </summary>
    [JsonPropertyName("event")]
    public int Event { get; set; }
}
