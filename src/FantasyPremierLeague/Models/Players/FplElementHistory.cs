using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Players;

/// <summary>
/// Represents fpl element history.
/// </summary>
public sealed class FplElementHistory
{
    /// <summary>
    /// Gets or sets the element id.
    /// </summary>
    [JsonPropertyName("element")]
    public int ElementId { get; set; }

    /// <summary>
    /// Gets or sets the fixture id.
    /// </summary>
    [JsonPropertyName("fixture")]
    public int FixtureId { get; set; }

    /// <summary>
    /// Gets or sets the opponent team id.
    /// </summary>
    [JsonPropertyName("opponent_team")]
    public int OpponentTeamId { get; set; }

    /// <summary>
    /// Gets or sets the total points.
    /// </summary>
    [JsonPropertyName("total_points")]
    public int TotalPoints { get; set; }

    /// <summary>
    /// Gets or sets the was home.
    /// </summary>
    [JsonPropertyName("was_home")]
    public bool WasHome { get; set; }

    /// <summary>
    /// Gets or sets the round.
    /// </summary>
    [JsonPropertyName("round")]
    public int Round { get; set; }

    /// <summary>
    /// Gets or sets the minutes.
    /// </summary>
    [JsonPropertyName("minutes")]
    public int Minutes { get; set; }

    /// <summary>
    /// Gets or sets the goals scored.
    /// </summary>
    [JsonPropertyName("goals_scored")]
    public int GoalsScored { get; set; }

    /// <summary>
    /// Gets or sets the assists.
    /// </summary>
    [JsonPropertyName("assists")]
    public int Assists { get; set; }

    /// <summary>
    /// Gets or sets the bonus.
    /// </summary>
    [JsonPropertyName("bonus")]
    public int Bonus { get; set; }
}
