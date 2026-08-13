using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Players;

/// <summary>
/// Represents fpl live element explain.
/// </summary>
public sealed class FplLiveElementExplain
{
    /// <summary>
    /// Gets or sets the fixture.
    /// </summary>
    [JsonPropertyName("fixture")]
    public string? Fixture { get; set; }

    /// <summary>
    /// Gets or sets the stats.
    /// </summary>
    [JsonPropertyName("stats")]
    public List<FplElementStats> PlayerStats { get; set; } = [];

}
