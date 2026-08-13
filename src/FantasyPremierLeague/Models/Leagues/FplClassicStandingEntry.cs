using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Leagues;

/// <summary>
/// Represents fpl standing entry.
/// </summary>
public sealed class FplClassicStandingEntry
{
    /// <summary>
    /// Gets or sets the entry.
    /// </summary>
    [JsonPropertyName("entry")]
    public int Manager { get; set; }

    /// <summary>
    /// Gets or sets the entry name.
    /// </summary>
    [JsonPropertyName("entry_name")]
    public string TeamName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the player name.
    /// </summary>
    [JsonPropertyName("player_name")]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the rank.
    /// </summary>
    [JsonPropertyName("rank")]
    public int Rank { get; set; }

    /// <summary>
    /// Gets or sets the total.
    /// </summary>
    [JsonPropertyName("total")]
    public int Total { get; set; }


    /// <summary>
    /// Gets or sets the player first name.
    /// </summary>
    [JsonPropertyName("player_first_name")]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the player last name.
    /// </summary>
    [JsonPropertyName("player_last_name")]
    public string LastName { get; set; } = string.Empty;
}
