using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Players;

/// <summary>
/// Represents fpl live element type.
/// </summary>
public sealed class FplLiveElement
{


    /// <summary>
    /// Gets or sets the players.
    /// </summary>

    [JsonPropertyName("elements")]
    public List<FplLiveElementInfo> Players { get; set; } = [];


}
