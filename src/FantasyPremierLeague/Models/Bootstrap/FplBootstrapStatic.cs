using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Bootstrap;

/// <summary>
/// Represents fpl bootstrap static.
/// </summary>
public sealed class FplBootstrapStatic
{
    /// <summary>
    /// Gets or sets the events.
    /// </summary>
    [JsonPropertyName("events")]
    public List<FplGameweek> Events { get; set; } = [];

    /// <summary>
    /// Gets or sets the teams.
    /// </summary>
    [JsonPropertyName("teams")]
    public List<FplTeamInfo> Teams { get; set; } = [];

    /// <summary>
    /// Gets or sets the elements.
    /// </summary>
    [JsonPropertyName("elements")]
    public List<FplElement> Elements { get; set; } = [];

    /// <summary>
    /// Gets or sets the element types.
    /// </summary>
    [JsonPropertyName("element_types")]
    public List<FplElementType> ElementTypes { get; set; } = [];

    /// <summary>
    /// Gets or sets the total players.
    /// </summary>
    [JsonPropertyName("total_players")]
    public long TotalPlayers { get; set; }
}
