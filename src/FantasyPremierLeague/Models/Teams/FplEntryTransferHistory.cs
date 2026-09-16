using System.Text.Json.Serialization;
using FantasyPremierLeague.Models.Bootstrap;

namespace FantasyPremierLeague.Models.Teams;

/// <summary>
/// Represents fpl entry transfer history .
/// </summary>
public sealed class FplEntryTransferHistory
{
    /// <summary>
    /// Gets or sets element in.
    /// </summary>
    [JsonPropertyName("element_in")]
    public int PlayerIn { get; set; }

    /// <summary>
    /// Gets or sets the element out.
    /// </summary>
    [JsonPropertyName("element_out")]
    public int PlayerOut { get; set; }

    /// <summary>
    /// Gets or sets manager.
    /// </summary>
    [JsonPropertyName("entry")]
    public int Manager { get; set; }

    /// <summary>
    /// Gets or sets the element in cost.
    /// </summary>
    [JsonPropertyName("element_in_cost")]
    public int PlayerInCost { get; set; }

    /// <summary>
    /// Gets or sets the element out cost.
    /// </summary>
    [JsonPropertyName("element_out_cost")]
    public int PlayerOutCost { get; set; }

    /// <summary>
    /// Gets or sets Gw.
    /// </summary>
    [JsonPropertyName("event")]
    public int GW { get; set; }

}
