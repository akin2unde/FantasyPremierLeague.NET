using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Leagues;

/// <summary>
/// Represents fpl standing entry.
/// </summary>
public sealed class FplH2HStandingEntry
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
    /// Gets or sets  real-world first and last name of the manager..
    /// </summary>
    [JsonPropertyName("player_name")]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the rank.
    /// </summary>
    [JsonPropertyName("rank")]
    public int Rank { get; set; }

    /// <summary>
    /// Gets or sets the total h2h point.
    /// </summary>
    [JsonPropertyName("points_total")]
    public int Total { get; set; }


    /// <summary>
    /// Gets or sets the total fpl point.
    /// </summary>
    [JsonPropertyName("total")]
    public int OverallTotal { get; set; }


    /// <summary>
    /// Gets or sets the player first name.
    /// </summary>
    [JsonPropertyName("movement")]
    public string Movement { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the total match played.
    /// </summary>
    [JsonPropertyName("matches_played")]
    public int Played { get; set; }

    /// <summary>
    /// Gets or sets the Total number of H2H matches won (3 points per win)..
    /// </summary>
    [JsonPropertyName("matches_won")]
    public int Won { get; set; }

    /// <summary>
    /// Gets or sets the Total number of H2H matches won (3 points per win)..
    /// </summary>
    [JsonPropertyName("matches_drawn")]
    public int Draw { get; set; }

    /// <summary>
    /// Gets or sets the Total number of H2H matches won (3 points per win)..
    /// </summary>
    [JsonPropertyName("matches_lost")]
    public int Lost { get; set; }


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
