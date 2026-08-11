using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Bootstrap;

/// <summary>
/// Represents pahses og game info.
/// </summary>
public sealed class FplPhase
{
    /// <summary>
    /// Gets or sets id.
    /// </summary>
    [JsonPropertyName("id")]
    public long Id { get; set; }
    /// <summary>
    /// Gets or sets name.
    /// </summary>

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets Start GW of the phase.
    /// </summary>

    [JsonPropertyName("start_event")]
    public long StartEvent { get; set; }
    /// <summary>
    /// Gets or sets End GW of the phase.
    /// </summary>

    [JsonPropertyName("stop_event")]
    public long StopEvent { get; set; }
    /// <summary>
    /// Gets or sets manager with the highest score.
    /// </summary>

    [JsonPropertyName("highest_score")]
    public dynamic? HighestScoreManager { get; set; }




}
