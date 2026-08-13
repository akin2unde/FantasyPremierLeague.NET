using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Leagues;

/// <summary>
/// Represents fpl standings page.
/// </summary>
public sealed class FplH2HStandingsPage
{
    /// <summary>
    /// Gets or sets the has next.
    /// </summary>
    [JsonPropertyName("has_next")]
    public bool HasNext { get; set; }

    /// <summary>
    /// Gets or sets the page.
    /// </summary>
    [JsonPropertyName("page")]
    public int Page { get; set; }

    /// <summary>
    /// Gets or sets the results.
    /// </summary>
    [JsonPropertyName("results")]
    public List<FplH2HStandingEntry> Results { get; set; } = [];
}
