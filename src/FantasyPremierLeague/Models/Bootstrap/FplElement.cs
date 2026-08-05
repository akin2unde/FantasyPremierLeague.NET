using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Bootstrap;

/// <summary>
/// Represents fpl element.
/// </summary>
public sealed class FplElement
{
    /// <summary>
    /// Gets or sets the id.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the code.
    /// </summary>
    [JsonPropertyName("code")]
    public int Code { get; set; }

    /// <summary>
    /// Gets or sets the first name.
    /// </summary>
    [JsonPropertyName("first_name")]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the second name.
    /// </summary>
    [JsonPropertyName("second_name")]
    public string SecondName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the web name.
    /// </summary>
    [JsonPropertyName("web_name")]
    public string WebName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the team id.
    /// </summary>
    [JsonPropertyName("team")]
    public int TeamId { get; set; }

    /// <summary>
    /// Gets or sets the element type.
    /// </summary>
    [JsonPropertyName("element_type")]
    public int ElementType { get; set; }

    /// <summary>
    /// Gets or sets the now cost.
    /// </summary>
    [JsonPropertyName("now_cost")]
    public int NowCost { get; set; }

    /// <summary>
    /// Gets or sets the total points.
    /// </summary>
    [JsonPropertyName("total_points")]
    public int TotalPoints { get; set; }

    /// <summary>
    /// Gets or sets the event points.
    /// </summary>
    [JsonPropertyName("event_points")]
    public int EventPoints { get; set; }

    /// <summary>
    /// Gets or sets the photo.
    /// </summary>
    [JsonPropertyName("photo")]
    public string Photo { get; set; } = string.Empty;
}
