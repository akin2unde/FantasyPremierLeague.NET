using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Bootstrap;

/// <summary>
/// Represents Team transfer info.
/// </summary>
public class FplGWTransferInfo
{

    /// <summary>
    /// Gets or sets bank available balance.
    /// </summary>

    [JsonPropertyName("bank")]
    public float? Bank { get; set; }

    /// <summary>
    /// Gets or sets chip name.
    /// </summary>

    [JsonPropertyName("cost")]
    public int? TransferCost { get; set; }

    /// <summary>
    /// Gets or sets chip unit/number.
    /// </summary>

    [JsonPropertyName("limit")]
    public string? TranferAvailable { get; set; }

    /// <summary>
    /// Gets or sets start event gw number.
    /// </summary>

    [JsonPropertyName("made")]
    public int TransferMade { get; set; }

    /// <summary>
    /// Gets or sets status.
    /// </summary>

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    private float _value;
    /// <summary>
    /// Gets or sets Squad total value without value in bank.
    /// </summary>
    [JsonPropertyName("value")]
    public float SquadValue { get { return _value; } set { _value = value; } }
}
