using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Leagues;

/// <summary>
/// Represents fpl classic league standings.
/// </summary>
public sealed class FplClassicLeague
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
    public FplClassicStandingsPage? Standings { get; set; }

    /// <summary>
    /// Gets or sets new entries.
    /// </summary>

    [JsonPropertyName("new_entries")]
    public FplClassicStandingsPage? NewEntries { get; set; }
    /// <summary>
    /// Gets or sets last time the league was updated.
    /// </summary>

    [JsonPropertyName("last_updated_data")]
    public DateTimeOffset? LastUpdatedAt { get; set; }
}