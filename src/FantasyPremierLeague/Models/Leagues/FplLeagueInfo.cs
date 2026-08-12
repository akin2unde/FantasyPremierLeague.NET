using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Leagues;

/// <summary>
/// Represents fpl league info.
/// </summary>
public sealed class FplLeagueInfo
{
    /// <summary>
    /// Gets or sets the is close.
    /// </summary>
    [JsonPropertyName("closed")]
    public int IsClose { get; set; }

    /// <summary>
    /// Gets or sets the date created.
    /// </summary>
    [JsonPropertyName("created")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the id.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;


    /// <summary>
    /// Gets or sets the short name of the league.
    /// </summary>
    [JsonPropertyName("short_name")]
    public string ShortName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the format or type of the league.
    /// </summary>
    /// <remarks>
    /// Indicates the league type, such as a public or private league,
    /// depending on the value returned by the Fantasy Premier League API.
    /// </remarks>
    [JsonPropertyName("league_type")]
    public string LeagueFormat { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current rank of the league.
    /// </summary>
    [JsonPropertyName("rank")]
    public string Rank { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current rank of the manager's entry within the league.
    /// </summary>
    [JsonPropertyName("entry_rank")]
    public string EntryRank { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the manager's previous rank within the league.
    /// </summary>
    /// <remarks>
    /// Represents the entry's rank from the previous ranking period,
    /// allowing movement in league position to be determined.
    /// </remarks>
    [JsonPropertyName("entry_last_rank")]
    public string LastRank { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the admin id.
    /// </summary>
    [JsonPropertyName("admin_entry")]
    public int AdminEntry { get; set; }

    /// <summary>
    /// Gets or sets has cup.
    /// </summary>
    [JsonPropertyName("has_cup")]
    public bool HasCup { get; set; }

}
