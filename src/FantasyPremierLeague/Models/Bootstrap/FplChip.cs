using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Bootstrap;

/// <summary>
/// Represents chip team info.
/// </summary>
public class FplChip
{

    /// <summary>
    /// Gets or sets chip type.
    /// </summary>

    [JsonPropertyName("chip_type")]
    public string ChipType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets chip name.
    /// </summary>

    [JsonPropertyName("name")]
    public string ChipShortName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets chip unit/number.
    /// </summary>

    [JsonPropertyName("number")]
    public int Qty { get; set; }

    /// <summary>
    /// Gets or sets start event gw number.
    /// </summary>

    [JsonPropertyName("start_event")]
    public int StartGW { get; set; }

    /// <summary>
    /// Gets or sets end event gw number.
    /// </summary>

    [JsonPropertyName("stop_event")]
    public int EndGW { get; set; }
}
