using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Bootstrap;

/// <summary>
/// Represents fpl gameweek.
/// </summary>
public sealed class FplGameweek
{
    /// <summary>
    /// Gets or sets the id.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the deadline time.
    /// </summary>
    [JsonPropertyName("deadline_time")]
    public DateTimeOffset DeadlineTime { get; set; }

    /// <summary>
    /// Gets or sets the is current.
    /// </summary>
    [JsonPropertyName("is_current")]
    public bool IsCurrent { get; set; }

    /// <summary>
    /// Gets or sets the is next.
    /// </summary>
    [JsonPropertyName("is_next")]
    public bool IsNext { get; set; }

    /// <summary>
    /// Gets or sets the finished.
    /// </summary>
    [JsonPropertyName("finished")]
    public bool Finished { get; set; }
}
