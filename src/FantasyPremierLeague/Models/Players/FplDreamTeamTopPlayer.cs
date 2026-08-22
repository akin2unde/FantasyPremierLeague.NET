using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Players;

/// <summary>
/// Represents the highest-scoring player in an FPL gameweek Dream Team.
/// </summary>
public sealed class FplDreamTeamTopPlayer
{
    /// <summary>
    /// Gets or sets the FPL element identifier of the player.
    /// </summary>
    [JsonPropertyName("id")]
    public int Element { get; set; }

    /// <summary>
    /// Gets or sets the points scored by the player in the gameweek.
    /// </summary>
    [JsonPropertyName("points")]
    public int Points { get; set; }
}
