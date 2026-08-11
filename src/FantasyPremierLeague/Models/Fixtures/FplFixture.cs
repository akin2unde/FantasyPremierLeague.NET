using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Fixtures;

/// <summary>
/// Represents fpl fixture.
/// </summary>
public class FplFixture
{
    /// <summary>
    /// Gets or sets the id.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the code.
    /// </summary>
    [JsonPropertyName("code")]
    public long FixtureCode { get; set; }

    /// <summary>
    /// Gets or sets the event.
    /// </summary>
    [JsonPropertyName("event")]
    public int? Event { get; set; }

    /// <summary>
    /// Gets or sets the home team id.
    /// </summary>
    [JsonPropertyName("team_h")]
    public int HomeTeamId { get; set; }

    /// <summary>
    /// Gets or sets the away team id.
    /// </summary>
    [JsonPropertyName("team_a")]
    public int AwayTeamId { get; set; }

    /// <summary>
    /// Gets or sets the home score.
    /// </summary>
    [JsonPropertyName("team_h_score")]
    public int? HomeScore { get; set; }

    /// <summary>
    /// Gets or sets the away score.
    /// </summary>
    [JsonPropertyName("team_a_score")]
    public int? AwayScore { get; set; }

    /// <summary>
    /// Gets or sets the home difficulty.
    /// </summary>
    [JsonPropertyName("team_h_difficulty")]
    public int HomeDifficulty { get; set; }

    /// <summary>
    /// Gets or sets the away difficulty.
    /// </summary>
    [JsonPropertyName("team_a_difficulty")]
    public int AwayDifficulty { get; set; }

    /// <summary>
    /// Gets or sets the kickoff time.
    /// </summary>
    [JsonPropertyName("kickoff_time")]
    public DateTimeOffset? GameTime { get; set; }

    /// <summary>
    /// Gets or sets the finished.
    /// </summary>
    [JsonPropertyName("finished")]
    public bool Finished { get; set; }

    /// <summary>
    /// Gets or sets the started.
    /// </summary>
    [JsonPropertyName("started")]
    public bool? Started { get; set; }

    /// <summary>
    /// Gets or sets the minutes.
    /// </summary>
    [JsonPropertyName("minutes")]
    public int MinutePlayed { get; set; }

    /// <summary>
    /// Gets or sets the game week name .
    /// </summary>
    [JsonPropertyName("event_name")]
    public string GWName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the game week stat info .
    /// </summary>
    [JsonPropertyName("stats")]
    public List<FplFixtureStatElement> GameStats { get; set; } = [];

}
