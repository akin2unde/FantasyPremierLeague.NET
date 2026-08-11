using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Bootstrap;

/// <summary>
/// Represents fpl bootstrap static.
/// </summary>
public class FplBootstrapStatic
{
    /// <summary>
    /// Gets or sets the phases.
    /// </summary>
    [JsonPropertyName("phases")]
    public List<FplPhase> Phases { get; set; } = [];
    /// <summary>
    /// Gets or sets the Label info for activities.
    /// </summary>

    [JsonPropertyName("element_stats")]
    public List<FplElementStats> StatsOptions { get; set; } = [];

    /// <summary>
    /// Gets or sets the events.
    /// </summary>
    [JsonPropertyName("chips")]
    public List<FplChip> Chips { get; set; } = [];

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
