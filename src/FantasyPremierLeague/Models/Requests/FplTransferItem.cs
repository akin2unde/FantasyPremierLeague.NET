using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Requests;

/// <summary>
/// Represents fpl transfer item.
/// </summary>
public sealed class FplTransferItem
{
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
    /// Gets or sets the purchase price.
    /// </summary>
    [JsonPropertyName("purchase_price")]
    public int PurchasePrice { get; set; }

    /// <summary>
    /// Gets or sets the selling price.
    /// </summary>
    [JsonPropertyName("selling_price")]
    public int SellingPrice { get; set; }
}
