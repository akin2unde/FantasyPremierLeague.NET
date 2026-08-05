using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Managers;

/// <summary>
/// Represents fpl entry.
/// </summary>
public sealed class FplEntry
{
    /// <summary>
    /// Gets or sets the id.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the player first name.
    /// </summary>
    [JsonPropertyName("player_first_name")]
    public string PlayerFirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the player last name.
    /// </summary>
    [JsonPropertyName("player_last_name")]
    public string PlayerLastName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the team name.
    /// </summary>
    [JsonPropertyName("name")]
    public string TeamName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the summary overall points.
    /// </summary>
    [JsonPropertyName("summary_overall_points")]
    public int? SummaryOverallPoints { get; set; }

    /// <summary>
    /// Gets or sets the summary overall rank.
    /// </summary>
    [JsonPropertyName("summary_overall_rank")]
    public int? SummaryOverallRank { get; set; }

    /// <summary>
    /// Gets or sets the current event.
    /// </summary>
    [JsonPropertyName("current_event")]
    public int? CurrentEvent { get; set; }

    /// <summary>
    /// Gets or sets the favourite team.
    /// </summary>
    [JsonPropertyName("favourite_team")]
    public int? FavouriteTeam { get; set; }

    /// <summary>
    /// Gets or sets the started event.
    /// </summary>
    [JsonPropertyName("started_event")]
    public int? StartedEvent { get; set; }

    /// <summary>
    /// Gets or sets the joined time.
    /// </summary>
    [JsonPropertyName("joined_time")]
    public DateTimeOffset? JoinedTime { get; set; }
}
