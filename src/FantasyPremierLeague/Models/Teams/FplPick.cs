using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Teams;

/// <summary>
/// Represents fpl pick.
/// </summary>
public sealed class FplPick
{
    /// <summary>
    /// Gets or sets the element.
    /// </summary>
    [JsonPropertyName("element")]
    public int Element { get; set; }

    /// <summary>
    /// Gets or sets the position.
    /// </summary>
    [JsonPropertyName("position")]
    public int Position { get; set; }

    /// <summary>
    /// Gets or sets the multiplier.
    /// </summary>
    [JsonPropertyName("multiplier")]
    public int Multiplier { get; set; }

    /// <summary>
    /// Gets or sets the is captain.
    /// </summary>
    [JsonPropertyName("is_captain")]
    public bool IsCaptain { get; set; }

    /// <summary>
    /// Gets or sets the is vice captain.
    /// </summary>
    [JsonPropertyName("is_vice_captain")]
    public bool IsViceCaptain { get; set; }

    /// <summary>
    /// Gets or sets the type of player , def or keeper or forward.
    /// </summary>
    [JsonPropertyName("element_type")]
    public int PlayerType { get; set; }

    /// <summary>
    /// Gets or sets price.
    /// </summary>
    [JsonPropertyName("selling_price")]
    public float SellingPrice { get; set; }

    /// <summary>
    /// Gets or sets the purchase price.
    /// </summary>
    [JsonPropertyName("purchase_price")]
    public float Cost { get; set; }
}
