using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Players;

/// <summary>
/// Represents fpl element summary.
/// </summary>
public sealed class FplElementSummary
{
    /// <summary>
    /// Gets or sets the fixtures.
    /// </summary>
    [JsonPropertyName("fixtures")]
    public List<FplElementUpcomingFixture> Fixtures { get; set; } = [];

    /// <summary>
    /// Gets or sets the history.
    /// </summary>
    [JsonPropertyName("history")]
    public List<FplElementHistory> History { get; set; } = [];

    /// <summary>
    /// Gets or sets the history past.
    /// </summary>
    [JsonPropertyName("history_past")]
    public List<FplElementPastSeason> HistoryPast { get; set; } = [];
}
