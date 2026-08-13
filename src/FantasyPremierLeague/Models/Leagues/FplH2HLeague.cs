using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Leagues;

/// <summary>
/// Represents fpl classic league standings.
/// </summary>
public sealed class FplH2HLeague
{
    /// <summary>
    /// Gets or sets the league.
    /// </summary>
    [JsonPropertyName("league")]
    public FplLeagueInfo? League { get; set; }

    /// <summary>
    /// Gets or sets the standings.
    /// </summary>
    [JsonPropertyName("standings")]
    public FplH2HStandingsPage? Standings { get; set; }

    /// <summary>
    /// Gets or sets new entries.
    /// </summary>

    [JsonPropertyName("new_entries")]
    public FplH2HStandingsPage? NewEntries { get; set; }
}