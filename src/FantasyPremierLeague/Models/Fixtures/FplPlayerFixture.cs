using System.Text.Json.Serialization;

namespace FantasyPremierLeague.Models.Fixtures;

/// <summary>
/// Represents a Fantasy Premier League player's performance
/// and statistics for a specific fixture.
/// </summary>
public class FplPlayerFixture
{
    /// <summary>
    /// Gets or sets the total FPL points earned by the player
    /// in the fixture.
    /// </summary>
    [JsonPropertyName("total_points")]
    public int Point { get; set; }

    /// <summary>
    /// Gets or sets the unique FPL identifier of the player.
    /// </summary>
    [JsonPropertyName("element")]
    public int PlayerId { get; set; }

    /// <summary>
    /// Stores the opponent team identifier returned using
    /// the <c>opponent</c> property.
    /// </summary>
    private int opponentTeamId;

    /// <summary>
    /// Gets or sets the identifier of the player's opponent team.
    /// </summary>
    /// <remarks>
    /// FPL responses may provide the opponent identifier using either
    /// <c>opponent</c> or <c>opponent_team</c>. When
    /// <c>opponent_team</c> contains a value greater than zero,
    /// that value takes precedence.
    /// </remarks>
    [JsonPropertyName("opponent")]
    public int OpponentTeamId
    {
        get
        {
            return OpponentTeamIdL > 0
                ? OpponentTeamIdL
                : opponentTeamId;
        }
        set
        {
            opponentTeamId = value;
        }
    }

    /// <summary>
    /// Gets or sets the opponent team identifier returned by
    /// the <c>opponent_team</c> property.
    /// </summary>
    [JsonPropertyName("opponent_team")]
    private int OpponentTeamIdL { get; set; }


    /// <summary>
    /// Gets or sets the home team's score for the fixture.
    /// </summary>
    [JsonPropertyName("team_h_score")]
    public string HomeScore { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the away team's score for the fixture.
    /// </summary>
    [JsonPropertyName("team_a_score")]
    public string AwayScore { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the identifier of the fixture.
    /// </summary>
    [JsonPropertyName("fixture")]
    public string FixtureCode { get; set; } = string.Empty;


    /// <summary>
    /// Gets or sets the number of goals scored by the player.
    /// </summary>
    [JsonPropertyName("goals_scored")]
    public int Goal { get; set; }

    /// <summary>
    /// Gets or sets the number of assists made by the player.
    /// </summary>
    [JsonPropertyName("assists")]
    public int Assist { get; set; }

    /// <summary>
    /// Gets or sets the number of clean sheets recorded by the player.
    /// </summary>
    [JsonPropertyName("clean_sheets")]
    public int CleanSheet { get; set; }

    /// <summary>
    /// Gets or sets the number of goals conceded while the player
    /// was participating in the fixture.
    /// </summary>
    [JsonPropertyName("goals_conceded")]
    public int GoalConceded { get; set; }

    /// <summary>
    /// Gets or sets the number of own goals scored by the player.
    /// </summary>
    [JsonPropertyName("own_goals")]
    public int OwnGoal { get; set; }

    /// <summary>
    /// Gets or sets the number of penalties saved by the player.
    /// </summary>
    [JsonPropertyName("penalties_saved")]
    public int PenaltySaved { get; set; }

    /// <summary>
    /// Gets or sets the number of penalties missed by the player.
    /// </summary>
    [JsonPropertyName("penalties_missed")]
    public int PenaltyMissed { get; set; }

    /// <summary>
    /// Gets or sets the number of yellow cards received by the player.
    /// </summary>
    [JsonPropertyName("yellow_cards")]
    public int YellowCard { get; set; }

    /// <summary>
    /// Gets or sets the number of red cards received by the player.
    /// </summary>
    [JsonPropertyName("red_cards")]
    public int RedCard { get; set; }

    /// <summary>
    /// Gets or sets the number of saves made by the player.
    /// </summary>
    [JsonPropertyName("saves")]
    public int Save { get; set; }

    /// <summary>
    /// Gets or sets the number of bonus points awarded to the player.
    /// </summary>
    [JsonPropertyName("bonus")]
    public int Bonus { get; set; }

    /// <summary>
    /// Gets or sets the player's Bonus Points System (BPS) score.
    /// </summary>
    [JsonPropertyName("bps")]
    public int BPS { get; set; }

    /// <summary>
    /// Gets or sets the player's influence score for the fixture.
    /// </summary>
    [JsonPropertyName("influence")]
    public string Influence { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the player's creativity score for the fixture.
    /// </summary>
    [JsonPropertyName("creativity")]
    public string Creativity { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the player's threat score for the fixture.
    /// </summary>
    [JsonPropertyName("threat")]
    public string Threat { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the player's ICT index for the fixture.
    /// </summary>
    /// <remarks>
    /// The ICT index combines influence, creativity, and threat.
    /// </remarks>
    [JsonPropertyName("ict_index")]
    public string IctIndex { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the player's value for the fixture.
    /// </summary>
    [JsonPropertyName("value")]
    public int Value { get; set; }

    /// <summary>
    /// Gets or sets the player's net transfer balance.
    /// </summary>
    [JsonPropertyName("transfers_balance")]
    public int TransferBalance { get; set; }

    /// <summary>
    /// Gets or sets the number of FPL managers who selected the player.
    /// </summary>
    [JsonPropertyName("selected")]
    public int Selected { get; set; }

    /// <summary>
    /// Gets or sets the number of transfers involving the player
    /// being transferred in.
    /// </summary>
    [JsonPropertyName("transfers_in")]
    public int TransferIn { get; set; }

    /// <summary>
    /// Gets or sets the number of transfers involving the player
    /// being transferred out.
    /// </summary>
    [JsonPropertyName("transfers_out")]
    public int TransferOut { get; set; }

    /// <summary>
    /// Gets or sets the gameweek number associated with the fixture.
    /// </summary>
    [JsonPropertyName("round")]
    public int Round { get; set; }

    /// <summary>
    /// Gets or sets the scheduled kickoff time of the fixture.
    /// </summary>
    [JsonPropertyName("kickoff_time")]
    public DateTime GameTime { get; set; } = DateTime.Now;

    /// <summary>
    /// Gets or sets a value indicating whether the player's team
    /// was the home team in the fixture.
    /// </summary>
    [JsonPropertyName("was_home")]
    public bool IsHome { get; set; }

    /// <summary>
    /// Gets or sets the number of minutes played by the player
    /// in the fixture.
    /// </summary>
    [JsonPropertyName("minutes")]
    public int MinutePlayed { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the fixture has finished.
    /// </summary>
    public bool Finished { get; set; } = true;

    /// <summary>
    /// Gets or sets the fixture difficulty rating for the home team.
    /// </summary>
    [JsonPropertyName("team_h_difficulty")]
    public int HomeDifficulty { get; set; }

    /// <summary>
    /// Gets or sets the fixture difficulty rating for the away team.
    /// </summary>
    [JsonPropertyName("team_a_difficulty")]
    public int AwayDifficulty { get; set; }
}