using System.Text.Json.Serialization;
using FantasyPremierLeague.Models.Bootstrap;

namespace FantasyPremierLeague.Models.Teams;

/// <summary>
/// Represents fpl entry history response.
/// </summary>
public sealed class FplEntryHistoryReponse
{
    /// <summary>
    /// Gets or sets the event.
    /// </summary>
    [JsonPropertyName("current")]
    public List<FplEntryHistory> CurrentSeason { get; set; } = [];

    /// <summary>
    /// Gets or sets the points.
    /// </summary>
    [JsonPropertyName("past")]
    public List<FplEntryPastSeasson> Seasons { get; set; } = [];

    /// <summary>
    /// Gets or sets the chip used in the current sesson so far .
    /// </summary>
    [JsonPropertyName("chips")]
    public List<FplChip> ChipUsed { get; set; } = [];




}
