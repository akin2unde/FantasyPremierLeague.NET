using System.Text.Json.Serialization;
using FantasyPremierLeague.Models.Bootstrap;

namespace FantasyPremierLeague.Models.Teams;

/// <summary>
/// Represents fpl team picks.
/// </summary>
public sealed class FplTeamPicks
{
    /// <summary>
    /// Gets or sets the active chip.
    /// </summary>
    [JsonPropertyName("active_chip")]
    public string? ActiveChip { get; set; }

    /// <summary>
    /// Gets or sets the automatic subs.
    /// </summary>
    [JsonPropertyName("automatic_subs")]
    public List<FplAutomaticSub> PlayerSubs { get; set; } = [];

    /// <summary>
    /// Gets or sets the entry history.
    /// </summary>
    [JsonPropertyName("entry_history")]
    public FplEntryHistory? EntryHistory { get; set; }

    /// <summary>
    /// Gets or sets the picks.
    /// </summary>
    [JsonPropertyName("picks")]
    public List<FplPick> Picks { get; set; } = [];
    /// <summary>
    /// Gets or sets the chips.
    /// </summary>

    [JsonPropertyName("chips")]
    public List<FplChip> Chips { get; set; } = [];

    /// <summary>
    /// Gets or sets the TeamInfo.
    /// </summary>

    [JsonPropertyName("transfers")]
    public FplGWTransferInfo? TeamInfo { get; set; }
}
