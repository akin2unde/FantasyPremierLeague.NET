using System.Text.Json.Serialization;


namespace FantasyPremierLeague.Models.Fixtures;

/// <summary>
/// Represents an individual player's contribution to a fixture statistic
/// returned by Fantasy Premier League (FPL).
/// </summary>
public class FplFixtureStatElement
{
    /// <summary>
    /// Gets or sets the value of the fixture statistic recorded for the player.
    /// </summary>
    [JsonPropertyName("value")]
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique FPL identifier of the player
    /// associated with the statistic.
    /// </summary>
    [JsonPropertyName("element")]
    public int PlayerId { get; set; }

    /// <summary>
    /// Gets or sets the player associated with the fixture statistic.
    /// </summary>
    /// <remarks>
    /// This property is populated by the application and is not mapped
    /// directly from the FPL fixture statistic response.
    /// </remarks>
    public Players.FplElement? Player { get; set; }
}
