using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Leagues;

/// <summary>
/// Represents a paged response containing head-to-head league matches.
/// </summary>
public sealed class FplH2HMatchesResponse
{
    /// <summary>
    /// Gets or sets a value indicating whether another page of matches is available.
    /// </summary>
    [JsonPropertyName("has_next")]
    public bool HasNext { get; set; }

    /// <summary>
    /// Gets or sets the current result page number.
    /// </summary>
    [JsonPropertyName("page")]
    public int Page { get; set; }

    /// <summary>
    /// Gets or sets the head-to-head matches returned for the current page.
    /// </summary>
    [JsonPropertyName("results")]
    public List<FplH2HMatch> Results { get; set; } = [];
}
