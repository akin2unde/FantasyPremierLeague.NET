using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Players;

/// <summary>
/// Represents fpl element upcoming fixture.
/// </summary>
public sealed class FplElementUpcomingFixture
{
    /// <summary>
    /// Gets or sets the id.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

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
    /// Gets or sets the is home.
    /// </summary>
    [JsonPropertyName("is_home")]
    public bool IsHome { get; set; }

    /// <summary>
    /// Gets or sets the difficulty.
    /// </summary>
    [JsonPropertyName("difficulty")]
    public int Difficulty { get; set; }

    /// <summary>
    /// Gets or sets the kickoff time.
    /// </summary>
    [JsonPropertyName("kickoff_time")]
    public DateTimeOffset? KickoffTime { get; set; }
}
