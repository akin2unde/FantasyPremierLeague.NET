using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Players;

/// <summary>
/// Represents a player entry in an FPL gameweek Dream Team.
/// </summary>
public sealed class FplDreamTeamPlayer
{
    /// <summary>
    /// Gets or sets the player's FPL element identifier.
    /// Use this value to match the player with an element from the bootstrap-static endpoint.
    /// </summary>
    [JsonPropertyName("element")]
    public int Element { get; set; }

    /// <summary>
    /// Gets or sets the points scored by the player in the selected gameweek.
    /// </summary>
    [JsonPropertyName("points")]
    public int Points { get; set; }

    /// <summary>
    /// Gets or sets the player's display position in the Dream Team, from 1 through 11.
    /// </summary>
    [JsonPropertyName("position")]
    public int Position { get; set; }

    /// <summary>
    /// Gets or sets the player's model.
    /// </summary>
    public FplElement? Player { get; set; }
}
