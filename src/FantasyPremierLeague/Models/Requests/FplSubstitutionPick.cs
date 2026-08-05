using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Requests;

/// <summary>
/// Represents fpl substitution pick.
/// </summary>
public sealed class FplSubstitutionPick
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
    /// Gets or sets the is captain.
    /// </summary>
    [JsonPropertyName("is_captain")]
    public bool IsCaptain { get; set; }

    /// <summary>
    /// Gets or sets the is vice captain.
    /// </summary>
    [JsonPropertyName("is_vice_captain")]
    public bool IsViceCaptain { get; set; }
}
