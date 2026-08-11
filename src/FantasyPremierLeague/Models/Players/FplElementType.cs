using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Players;

/// <summary>
/// Represents fpl element type.
/// </summary>
public sealed class FplElementType
{
    /// <summary>
    /// Gets or sets the id.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the singular name.
    /// </summary>
    [JsonPropertyName("singular_name")]
    public string SingularName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the plural name.
    /// </summary>
    [JsonPropertyName("plural_name")]
    public string PluralName { get; set; } = string.Empty;
}
