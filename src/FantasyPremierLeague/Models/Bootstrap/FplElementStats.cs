using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Bootstrap;

/// <summary>
/// Represents label and name value info of activities.
/// </summary>
public sealed class FplElementStats
{
    /// <summary>
    /// Gets or sets label name.
    /// </summary>

    [JsonPropertyName("label")]
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets label value.
    /// </summary>

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;


}
