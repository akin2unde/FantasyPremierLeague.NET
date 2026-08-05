using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Managers;

/// <summary>
/// Represents fpl me player.
/// </summary>
public sealed class FplMePlayer
{
    /// <summary>
    /// Gets or sets the id.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the first name.
    /// </summary>
    [JsonPropertyName("first_name")]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the last name.
    /// </summary>
    [JsonPropertyName("last_name")]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Team name.
    /// </summary>
    public string TeamName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the entry id.
    /// </summary>
    [JsonPropertyName("entry")]
    public int EntryId { get; set; }

    /// <summary>
    /// Gets or sets the region.
    /// </summary>
    [JsonPropertyName("region")]
    public int? Region { get; set; }
}
