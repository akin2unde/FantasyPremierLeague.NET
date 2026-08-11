using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Bootstrap;

/// <summary>
/// Represents fpl gameweek.
/// </summary>
public class FplGameweek
{
    /// <summary>
    /// Gets or sets the id.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the deadline time.
    /// </summary>
    [JsonPropertyName("deadline_time")]
    public DateTimeOffset Deadline { get; set; }

    /// <summary>
    /// Gets or sets the is current.
    /// </summary>
    [JsonPropertyName("is_current")]
    public bool IsCurrent { get; set; }

    /// <summary>
    /// Gets or sets the is next.
    /// </summary>
    [JsonPropertyName("is_next")]
    public bool IsNext { get; set; }

    /// <summary>
    /// Gets or sets the finished.
    /// </summary>
    [JsonPropertyName("finished")]
    public bool IsFinished { get; set; }

    /// <summary>
    /// Gets or sets the average score.
    /// </summary>
    [JsonPropertyName("average_entry_score")]
    public double AverageScore { get; set; }

    /// <summary>
    /// Gets or sets the gw highest score manager id.
    /// </summary>

    [JsonPropertyName("highest_score")]
    public string HighestScore { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets data checked.
    /// </summary>

    [JsonPropertyName("data_checked")]
    public bool DateChecked { get; set; }

    /// <summary>
    /// Gets or sets most captained player id for the gw.
    /// </summary>
    [JsonPropertyName("most_captained")]
    public string MostCaptainId { get; set; } = string.Empty;


    /// <summary>
    /// Gets or sets most selected player id for the gw.
    /// </summary>
    [JsonPropertyName("most_selected")]
    public string MostSelectedId { get; set; } = string.Empty;


    /// <summary>
    /// Gets or sets most transferred in player id for the gw.
    /// </summary>
    [JsonPropertyName("most_transferred_in")]
    public string MostTransferredInId { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets most vice captained player id for the gw.
    /// </summary>

    [JsonPropertyName("most_vice_captained")]
    public string MostVCaptainId { get; set; } = string.Empty;


    /// <summary>
    /// Gets or sets top player id for the gw.
    /// </summary>
    [JsonPropertyName("top_element")]
    public string TopElementId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets cup leagues created.
    /// </summary>
    [JsonPropertyName("cup_leagues_created")]
    public bool CupLeagueCreated { get; set; }

    /// <summary>
    /// Gets or sets manager with the highest score for the gw.
    /// </summary>
    [JsonPropertyName("highest_scoring_entry")]
    public int? HighestScoringEntryId { get; set; }

    /// <summary>
    /// Gets or sets deadline time epoch for the gw.
    /// </summary>

    [JsonPropertyName("deadline_time_epoch")]
    public int DeadLineTimeEpoch { get; set; }

    /// <summary>
    /// Gets or sets deadline time game offset for the gw.
    /// </summary>

    [JsonPropertyName("deadline_time_game_offset")]
    public int DeadLineTimeGameOffset { get; set; }

    /// <summary>
    /// Gets or sets deadline time formatted for the gw.
    /// </summary>

    [JsonPropertyName("deadline_time_formatted")]
    public string DeadLineTimeFormatted { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets is previous  gw.
    /// </summary>
    [JsonPropertyName("is_previous")]
    public bool IsPrevious { get; set; }

}
