using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Leagues;

/// <summary>
/// Represents a match between two entries in a Fantasy Premier League
/// head-to-head league.
/// </summary>
public sealed class FplH2HMatch
{
    /// <summary>
    /// Gets or sets the unique identifier of the head-to-head match.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the FPL entry identifier of the first manager.
    /// </summary>
    [JsonPropertyName("entry_1_entry")]
    public int Entry1Entry { get; set; }

    /// <summary>
    /// Gets or sets the team name of the first entry.
    /// </summary>
    [JsonPropertyName("entry_1_name")]
    public string Entry1Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the full name of the first manager.
    /// </summary>
    [JsonPropertyName("entry_1_player_name")]
    public string Entry1PlayerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the gameweek points scored by the first entry.
    /// </summary>
    [JsonPropertyName("entry_1_points")]
    public int Entry1Points { get; set; }

    /// <summary>
    /// Gets or sets the win indicator for the first entry.
    /// </summary>
    [JsonPropertyName("entry_1_win")]
    public int Entry1Win { get; set; }

    /// <summary>
    /// Gets or sets the draw indicator for the first entry.
    /// </summary>
    [JsonPropertyName("entry_1_draw")]
    public int Entry1Draw { get; set; }

    /// <summary>
    /// Gets or sets the loss indicator for the first entry.
    /// </summary>
    [JsonPropertyName("entry_1_loss")]
    public int Entry1Loss { get; set; }

    /// <summary>
    /// Gets or sets the league points awarded to the first entry for this match.
    /// </summary>
    [JsonPropertyName("entry_1_total")]
    public int Entry1Total { get; set; }

    /// <summary>
    /// Gets or sets the FPL entry identifier of the second manager.
    /// </summary>
    [JsonPropertyName("entry_2_entry")]
    public int Entry2Entry { get; set; }

    /// <summary>
    /// Gets or sets the team name of the second entry.
    /// </summary>
    [JsonPropertyName("entry_2_name")]
    public string Entry2Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the full name of the second manager.
    /// </summary>
    [JsonPropertyName("entry_2_player_name")]
    public string Entry2PlayerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the gameweek points scored by the second entry.
    /// </summary>
    [JsonPropertyName("entry_2_points")]
    public int Entry2Points { get; set; }

    /// <summary>
    /// Gets or sets the win indicator for the second entry.
    /// </summary>
    [JsonPropertyName("entry_2_win")]
    public int Entry2Win { get; set; }

    /// <summary>
    /// Gets or sets the draw indicator for the second entry.
    /// </summary>
    [JsonPropertyName("entry_2_draw")]
    public int Entry2Draw { get; set; }

    /// <summary>
    /// Gets or sets the loss indicator for the second entry.
    /// </summary>
    [JsonPropertyName("entry_2_loss")]
    public int Entry2Loss { get; set; }

    /// <summary>
    /// Gets or sets the league points awarded to the second entry for this match.
    /// </summary>
    [JsonPropertyName("entry_2_total")]
    public int Entry2Total { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the match belongs to a knockout stage.
    /// </summary>
    [JsonPropertyName("is_knockout")]
    public bool IsKnockout { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the head-to-head league.
    /// </summary>
    [JsonPropertyName("league")]
    public int League { get; set; }

    /// <summary>
    /// Gets or sets the winning entry identifier when supplied by FPL.
    /// </summary>
    [JsonPropertyName("winner")]
    public int? Winner { get; set; }

    /// <summary>
    /// Gets or sets the seed value used for a knockout match, when applicable.
    /// </summary>
    [JsonPropertyName("seed_value")]
    public int? SeedValue { get; set; }

    /// <summary>
    /// Gets or sets the gameweek in which the match is played.
    /// </summary>
    [JsonPropertyName("event")]
    public int Event { get; set; }

    /// <summary>
    /// Gets or sets the tiebreak value or description when supplied by FPL.
    /// </summary>
    [JsonPropertyName("tiebreak")]
    public string? Tiebreak { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether an entry receives a bye.
    /// </summary>
    [JsonPropertyName("is_bye")]
    public bool IsBye { get; set; }

    /// <summary>
    /// Gets or sets the knockout round name, or an empty string for league matches.
    /// </summary>
    [JsonPropertyName("knockout_name")]
    public string KnockoutName { get; set; } = string.Empty;
}
