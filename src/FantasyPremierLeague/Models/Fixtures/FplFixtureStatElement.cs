
using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Fixtures;


/// <summary>
/// Represents a collection of fixture statistics returned by
/// Fantasy Premier League (FPL) for both the home and away teams.
/// </summary>
public class FplFixtureStats
{
    /// <summary>
    /// Gets or sets the identifier that describes the type of
    /// fixture statistic.
    /// </summary>
    /// <remarks>
    /// Examples may include goals scored, assists, saves,
    /// yellow cards, red cards, bonus points, or other
    /// fixture-related statistics.
    /// </remarks>
    [JsonPropertyName("identifier")]
    public string Identifier { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the collection of player statistic elements
    /// associated with the away team.
    /// </summary>
    [JsonPropertyName("a")]
    public List<FplFixtureStatElement> Away { get; set; } = [];
    /// <summary>
    /// Gets or sets the collection of player statistic elements
    /// associated with the home team.
    /// </summary>
    [JsonPropertyName("h")]
    public List<FplFixtureStatElement> Home { get; set; } = [];
}
