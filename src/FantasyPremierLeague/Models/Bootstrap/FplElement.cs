using System.Text.Json.Serialization;
using FPLBE.Models;

namespace FantasyPremierLeague.Models.Bootstrap;

/// <summary>
/// Represents fpl element.
/// </summary>
public class FplElement
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
    public int PlayerCode { get; set; }

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
    public string ShortName { get; set; } = string.Empty;


    /// <summary>
    /// Gets or sets the element type.
    /// </summary>
    [JsonPropertyName("element_type")]
    public int ElementType { get; set; }

    /// <summary>
    /// Gets or sets the now cost.
    /// </summary>
    [JsonPropertyName("now_cost")]
    public float Price { get; set; }

    /// <summary>
    /// Gets or sets the total points.
    /// </summary>
    [JsonPropertyName("total_points")]
    public int TotalPoints { get; set; }


    /// <summary>
    /// Gets or sets the photo.
    /// </summary>
    [JsonPropertyName("photo")]
    public string Photo { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the player club code.
    /// </summary>
    [JsonPropertyName("team_code")]
    public int TeamCode { get; set; }

    /// <summary>
    /// Gets or sets the player status.
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the player squad number.
    /// </summary>
    [JsonPropertyName("squad_number")]
    public int? SquadNumber { get; set; }


    /// <summary>
    /// Gets or sets the player multiplier (2x for captain 3x for tripple captain).
    /// </summary>
    [JsonPropertyName("multiplier")]
    public int Multiplier { get; set; } = 1;

    /// <summary>
    /// Gets or sets the player injury news or suspension.
    /// </summary>
    [JsonPropertyName("news")]
    public string News { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets if the player is manager's captain.
    /// </summary>
    [JsonPropertyName("is_captain")]
    public bool Captain { get; set; }

    /// <summary>
    /// Gets or sets if the player is manager's vice captain.
    /// </summary>
    [JsonPropertyName("is_vice_captain")]
    public bool VCaptain { get; set; }

    /// <summary>
    /// Gets or sets chance of playing this round.
    /// </summary>
    [JsonPropertyName("chance_of_playing_this_round")]
    public string ChanceOfPlayingThisRound { get; set; } = string.Empty;


    /// <summary>
    /// Gets or sets chance of playing next round.
    /// </summary>
    [JsonPropertyName("chance_of_playing_next_round")]
    public string ChanceOfPlayingNextRound { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the player's value-for-form metric.
    /// </summary>
    [JsonPropertyName("value_form")]
    public double ValueForm { get; set; }

    /// <summary>
    /// Gets or sets the player's value-for-season metric.
    /// </summary>
    [JsonPropertyName("value_season")]
    public double ValueSeason { get; set; }

    /// <summary>
    /// Gets or sets the player's price change since the start of the season.
    /// </summary>
    [JsonPropertyName("cost_change_start")]
    public int CostChangeStart { get; set; }

    /// <summary>
    /// Gets or sets the player's price change for the current gameweek.
    /// </summary>
    [JsonPropertyName("cost_change_event")]
    public int CostChangeEvent { get; set; }

    /// <summary>
    /// Gets or sets the player's price fall since the start of the season.
    /// </summary>
    [JsonPropertyName("cost_change_start_fall")]
    public int CostChangeStartFall { get; set; }

    /// <summary>
    /// Gets or sets the player's price fall for the current gameweek.
    /// </summary>
    [JsonPropertyName("cost_change_event_fall")]
    public int CostChangeEventFall { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the player is currently in the dream team.
    /// </summary>
    [JsonPropertyName("in_dreamteam")]
    public bool InDreamteam { get; set; }

    /// <summary>
    /// Gets or sets the number of times the player has appeared in a dream team.
    /// </summary>
    [JsonPropertyName("dreamteam_count")]
    public int DreamteamCount { get; set; }

    /// <summary>
    /// Gets or sets the percentage of FPL managers who currently own the player.
    /// </summary>
    [JsonPropertyName("selected_by_percent")]
    public double OwnershipPercentage { get; set; }

    /// <summary>
    /// Gets or sets the player's current form rating.
    /// </summary>
    [JsonPropertyName("form")]
    public double Form { get; set; }

    /// <summary>
    /// Gets or sets the total number of transfers out involving the player.
    /// </summary>
    [JsonPropertyName("transfers_out")]
    public int TransfersOut { get; set; }

    /// <summary>
    /// Gets or sets the total number of transfers in involving the player.
    /// </summary>
    [JsonPropertyName("transfers_in")]
    public int TransfersIn { get; set; }

    /// <summary>
    /// Gets or sets the number of transfers out involving the player
    /// during the current gameweek.
    /// </summary>
    [JsonPropertyName("transfers_out_event")]
    public int TransfersOutEvent { get; set; }

    /// <summary>
    /// Gets or sets the number of transfers in involving the player
    /// during the current gameweek.
    /// </summary>
    [JsonPropertyName("transfers_in_event")]
    public int TransfersInEvent { get; set; }

    /// <summary>
    /// Gets or sets the number of loans into the player's team.
    /// </summary>
    [JsonPropertyName("loans_in")]
    public int LoansIn { get; set; }

    /// <summary>
    /// Gets or sets the number of loans out from the player's team.
    /// </summary>
    [JsonPropertyName("loans_out")]
    public int LoansOut { get; set; }

    /// <summary>
    /// Gets or sets the number of times the player has been loaned in.
    /// </summary>
    [JsonPropertyName("loaned_in")]
    public int LoanedIn { get; set; }

    /// <summary>
    /// Gets or sets the number of times the player has been loaned out.
    /// </summary>
    [JsonPropertyName("loaned_out")]
    public int LoanedOut { get; set; }



    /// <summary>
    /// Gets or sets the player's points for the current gameweek.
    /// </summary>
    [JsonPropertyName("event_points")]
    public int GWScore { get; set; }

    /// <summary>
    /// Gets or sets the player's average points per game.
    /// </summary>
    [JsonPropertyName("points_per_game")]
    public double PointsPerGame { get; set; }

    /// <summary>
    /// Gets or sets the player's expected points for the current gameweek.
    /// </summary>
    [JsonPropertyName("ep_this")]
    public double? EpThis { get; set; }

    /// <summary>
    /// Gets or sets the player's expected points for the next gameweek.
    /// </summary>
    [JsonPropertyName("ep_next")]
    public double? EpNext { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the player has a special status.
    /// </summary>
    [JsonPropertyName("special")]
    public bool Special { get; set; }

    /// <summary>
    /// Gets or sets the total number of minutes played by the player.
    /// </summary>
    [JsonPropertyName("minutes")]
    public int Minutes { get; set; }

    /// <summary>
    /// Gets or sets the number of goals scored by the player.
    /// </summary>
    [JsonPropertyName("goals_scored")]
    public int GoalsScored { get; set; }

    /// <summary>
    /// Gets or sets the number of assists made by the player.
    /// </summary>
    [JsonPropertyName("assists")]
    public int Assists { get; set; }

    /// <summary>
    /// Gets or sets the number of clean sheets recorded by the player.
    /// </summary>
    [JsonPropertyName("clean_sheets")]
    public int CleanSheets { get; set; }

    /// <summary>
    /// Gets or sets the number of goals conceded while the player was on the pitch.
    /// </summary>
    [JsonPropertyName("goals_conceded")]
    public int GoalsConceded { get; set; }

    /// <summary>
    /// Gets or sets the number of own goals scored by the player.
    /// </summary>
    [JsonPropertyName("own_goals")]
    public int OwnGoals { get; set; }

    /// <summary>
    /// Gets or sets the number of penalties saved by the player.
    /// </summary>
    [JsonPropertyName("penalties_saved")]
    public int PenaltiesSaved { get; set; }

    /// <summary>
    /// Gets or sets the number of penalties missed by the player.
    /// </summary>
    [JsonPropertyName("penalties_missed")]
    public int PenaltiesMissed { get; set; }

    /// <summary>
    /// Gets or sets the number of yellow cards received by the player.
    /// </summary>
    [JsonPropertyName("yellow_cards")]
    public int YellowCards { get; set; }

    /// <summary>
    /// Gets or sets the number of red cards received by the player.
    /// </summary>
    [JsonPropertyName("red_cards")]
    public int RedCards { get; set; }

    /// <summary>
    /// Gets or sets the number of saves made by the player.
    /// </summary>
    [JsonPropertyName("saves")]
    public int Saves { get; set; }

    /// <summary>
    /// Gets or sets the total bonus points awarded to the player.
    /// </summary>
    [JsonPropertyName("bonus")]
    public int Bonus { get; set; }

    /// <summary>
    /// Gets or sets the player's Bonus Points System (BPS) score.
    /// </summary>
    [JsonPropertyName("bps")]
    public int Bps { get; set; }

    /// <summary>
    /// Gets or sets the player's influence score.
    /// </summary>
    [JsonPropertyName("influence")]
    public double Influence { get; set; }

    /// <summary>
    /// Gets or sets the player's creativity score.
    /// </summary>
    [JsonPropertyName("creativity")]
    public double Creativity { get; set; }

    /// <summary>
    /// Gets or sets the player's threat score.
    /// </summary>
    [JsonPropertyName("threat")]
    public double Threat { get; set; }

    /// <summary>
    /// Gets or sets the player's ICT index.
    /// </summary>
    /// <remarks>
    /// The ICT index combines the player's influence, creativity,
    /// and threat metrics.
    /// </remarks>
    [JsonPropertyName("ict_index")]
    public double IctIndex { get; set; }

    /// <summary>
    /// Gets or sets the player's EA index.
    /// </summary>
    [JsonPropertyName("ea_index")]
    public double EaIndex { get; set; }

    /// <summary>
    /// Gets or sets the player's FPL position name.
    /// </summary>
    public FplPlayerPosition PlayerPosition
    {
        get
        {
            return ElementType == (int)FplPlayerPosition.NotSet
                     ? (FplPlayerPosition)ElementType
                     : PlayerPosition;
        }
    }

    private int position;

    /// <summary>
    /// Gets or sets the player's position value.
    /// </summary>
    /// <remarks>
    /// When <see cref="PlayerPosition"/> has not already been set,
    /// assigning this property also converts the numeric position into
    /// an <see cref="FplPlayerPosition"/> value.
    /// </remarks>
    [JsonPropertyName("position")]
    public int Position
    {
        get
        {
            return position;
        }

        set
        {
            position = value;

        }
    }

    /// <summary>
    /// Gets or sets the identifier of the team to which the player belongs.
    /// </summary>
    [JsonPropertyName("team")]
    public int TeamId { get; set; }
}
