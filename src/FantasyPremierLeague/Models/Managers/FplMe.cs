using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Managers;

/// <summary>
/// Represents fpl me.
/// </summary>
public sealed class FplMe
{
    /// <summary>
    /// Gets or sets the player.
    /// </summary>
    [JsonPropertyName("player")]
    public FplMePlayer? Player { get; set; }
}
