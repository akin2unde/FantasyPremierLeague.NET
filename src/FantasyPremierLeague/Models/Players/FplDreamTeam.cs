using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Players;

/// <summary>
/// Represents the Dream Team returned by the Fantasy Premier League API for a gameweek.
/// </summary>
public sealed class FplDreamTeam
{
    /// <summary>
    /// Gets or sets the highest-scoring player in the gameweek Dream Team.
    /// </summary>
    [JsonPropertyName("top_player")]
    public FplDreamTeamTopPlayer TopPlayer { get; set; } = new();

    /// <summary>
    /// Gets or sets the eleven players selected for the gameweek Dream Team.
    /// </summary>
    [JsonPropertyName("team")]
    public List<FplDreamTeamPlayer> Team { get; set; } = [];
}
