using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Players;

/// <summary>
/// Represents a statistical field or metric associated with a Fantasy
/// Premier League player.
/// </summary>
/// <remarks>
/// Contains metadata describing a player statistic, including its display
/// label, name, value, identifier, and points information.
/// </remarks>
public sealed class FplElementStats
{
    /// <summary>
    /// Gets or sets the display label of the player statistic.
    /// </summary>
    [JsonPropertyName("label")]
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the player statistic.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value associated with the player statistic.
    /// </summary>
    [JsonPropertyName("value")]
    public int Value { get; set; }

    /// <summary>
    /// Gets or sets the identifier used to uniquely identify the
    /// player statistic.
    /// </summary>
    [JsonPropertyName("identifier")]
    public string ActionName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the points associated with the player statistic.
    /// </summary>
    [JsonPropertyName("points")]
    public int? Point { get; set; }
}