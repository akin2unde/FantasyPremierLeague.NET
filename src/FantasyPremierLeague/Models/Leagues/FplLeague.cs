using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Leagues;

/// <summary>
/// Represents fpl league info.
/// </summary>
public sealed class FplLeague
{
    /// <summary>
    /// Gets or sets the is close.
    /// </summary>
    [JsonPropertyName("classic")]
    public List<FplLeagueInfo> ClassicLeague { get; set; } = [];

    /// <summary>
    /// Gets or sets the date created.
    /// </summary>
    [JsonPropertyName("h2h")]
    public List<FplLeagueInfo> H2HLeague { get; set; } = [];



}
