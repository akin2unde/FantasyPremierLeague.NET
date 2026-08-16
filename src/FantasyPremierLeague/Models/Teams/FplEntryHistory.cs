using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Teams;

/// <summary>
/// Represents fpl entry history.
/// </summary>
public sealed class FplEntryHistory
{
    /// <summary>
    /// Gets or sets the event.
    /// </summary>
    [JsonPropertyName("event")]
    public int Event { get; set; }

    /// <summary>
    /// Gets or sets the points.
    /// </summary>
    [JsonPropertyName("points")]
    public int Points { get; set; }

    /// <summary>
    /// Gets or sets the total points.
    /// </summary>
    [JsonPropertyName("total_points")]
    public int TotalPoints { get; set; }

    /// <summary>
    /// Gets or sets the rank.
    /// </summary>
    [JsonPropertyName("rank")]
    public int? Rank { get; set; }

    /// <summary>
    /// Gets or sets the overall rank.
    /// </summary>
    [JsonPropertyName("overall_rank")]
    public int? OverallRank { get; set; }

    /// <summary>
    /// Gets or sets the bank.
    /// </summary>
    [JsonPropertyName("bank")]
    public float Bank { get; set; }

    /// <summary>
    /// Gets or sets the squad value.
    /// </summary>
    [JsonPropertyName("value")]
    public float Value { get; set; }

    /// <summary>
    /// Gets or sets the event transfers.
    /// </summary>
    [JsonPropertyName("event_transfers")]
    public int EventTransfers { get; set; }

    /// <summary>
    /// Gets or sets the event transfers cost.
    /// </summary>
    [JsonPropertyName("event_transfers_cost")]
    public int EventTransfersCost { get; set; }

    /// <summary>
    /// Gets or sets the points on bench.
    /// </summary>
    [JsonPropertyName("points_on_bench")]
    public int PointsOnBench { get; set; }

    /// <summary>
    /// Gets or sets the percentage rank.
    /// </summary>
    [JsonPropertyName("percentile_rank")]
    public int? PercentageRank { get; set; }
}
