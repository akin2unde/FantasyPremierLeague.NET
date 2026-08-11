using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Leagues;

/// <summary>
/// Represents fpl classic league standings.
/// </summary>
public sealed class FplClassicLeagueStandings
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
    public FplStandingsPage? Standings { get; set; }
}