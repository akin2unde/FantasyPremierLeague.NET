using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Teams;

/// <summary>
/// Represents fpl entry history response.
/// </summary>
public sealed class FplEntryPastSeasson
{
    /// <summary>
    /// Gets or sets the season name.
    /// </summary>
    [JsonPropertyName("season_name")]
    public string SeasonName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the rank percentage.
    /// </summary>
    [JsonPropertyName("rank_percentage")]
    public string RankPercentage { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the rank.
    /// </summary>
    [JsonPropertyName("rank")]
    public int? Rank { get; set; }

    /// <summary>
    /// Gets or sets the total points.
    /// </summary>
    [JsonPropertyName("total_points")]
    public int TotalPoints { get; set; }
}
