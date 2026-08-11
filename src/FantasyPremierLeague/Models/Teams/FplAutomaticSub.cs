using System.Text.Json.Serialization;
using FantasyPremierLeague.Models.Players;

namespace FantasyPremierLeague.Models.Teams;

/// <summary>
/// Represents fpl automatic sub.
/// </summary>
public sealed class FplAutomaticSub
{
    /// <summary>
    /// Gets or sets the entry.
    /// </summary>
    [JsonPropertyName("entry")]
    public int Entry { get; set; }

    /// <summary>
    /// Gets or sets the element in.
    /// </summary>
    [JsonPropertyName("element_in")]
    public int ElementIn { get; set; }

    /// <summary>
    /// Gets or sets the element out.
    /// </summary>
    [JsonPropertyName("element_out")]
    public int ElementOut { get; set; }

    /// <summary>
    /// Gets or sets the event.
    /// </summary>
    [JsonPropertyName("event")]
    public int Event { get; set; }

    /// <summary>
    /// Gets or sets the cost of the player transferred into the team.
    /// </summary>
    /// <remarks>
    /// This value represents the player's price at the time the transfer
    /// was made.
    /// </remarks>
    [JsonPropertyName("element_in_cost")]
    public string PlayerInCost { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the cost of the player transferred out of the team.
    /// </summary>
    /// <remarks>
    /// This value represents the player's price at the time the transfer
    /// was made.
    /// </remarks>
    [JsonPropertyName("element_out_cost")]
    public string PlayerOutCost { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time at which the transfer was made.
    /// </summary>
    [JsonPropertyName("time")]
    public string GWDate { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the FPL player who was transferred into the team.
    /// </summary>
    /// <remarks>
    /// This property contains the resolved player information associated
    /// with the incoming player's FPL element identifier.
    /// </remarks>
    public FplElement? PlayerSubIn { get; set; }

    /// <summary>
    /// Gets or sets the FPL player who was transferred out of the team.
    /// </summary>
    /// <remarks>
    /// This property contains the resolved player information associated
    /// with the outgoing player's FPL element identifier.
    /// </remarks>
    public FplElement? PlayerSubOut { get; set; }
}
