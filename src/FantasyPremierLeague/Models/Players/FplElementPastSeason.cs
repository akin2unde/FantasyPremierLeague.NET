using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Players;

/// <summary>
/// Represents fpl element past season.
/// </summary>
public sealed class FplElementPastSeason
{
    /// <summary>
    /// Gets or sets the season name.
    /// </summary>
    [JsonPropertyName("season_name")]
    public string SeasonName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the element code.
    /// </summary>
    [JsonPropertyName("element_code")]
    public int ElementCode { get; set; }

    /// <summary>
    /// Gets or sets the start cost.
    /// </summary>
    [JsonPropertyName("start_cost")]
    public int StartCost { get; set; }

    /// <summary>
    /// Gets or sets the end cost.
    /// </summary>
    [JsonPropertyName("end_cost")]
    public int EndCost { get; set; }

    /// <summary>
    /// Gets or sets the total points.
    /// </summary>
    [JsonPropertyName("total_points")]
    public int TotalPoints { get; set; }
}
