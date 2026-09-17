using FantasyPremierLeague.Models.Requests;
using FantasyPremierLeague.SampleApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FantasyPremierLeague.SampleApi.Controllers;

/// <summary>
/// Demonstrates every endpoint currently exposed by FantasyPremierLeague.NET.
/// </summary>
[Route("api/fpl")]
[ApiController]
public sealed class FplManagerController : ControllerBase
{
    private readonly IFplManagerService _service;

    /// <summary>Initializes the sample controller.</summary>
    public FplManagerController(IFplManagerService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>Authenticates a manager or reuses/refreshes a persisted session.</summary>
    [HttpPost("authentication/login")]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var manager = await _service.LoginAsync(
            request.Email,
            request.Password,
            request.ForceRefresh,
            request.IncludeDetails,
            cancellationToken);

        return Ok(new
        {
            manager.Email,
            manager.EntryId,
            manager.TokenExpiresAt,
            manager.RefreshTokenExpiresAt,
            manager.Profile,
            manager.Entry
        });
    }

    /// <summary>Deletes a persisted manager session.</summary>
    [HttpDelete("authentication/{email}")]
    public async Task<IActionResult> Logout(string email, CancellationToken cancellationToken)
    {
        await _service.LogoutAsync(email, cancellationToken);
        return NoContent();
    }

    /// <summary>Refreshes the access token for an authenticated entry.</summary>
    [HttpPost("authentication/{entryId:int}/refresh")]
    public async Task<IActionResult> Refresh(int entryId, CancellationToken cancellationToken)
    {
        await _service.RefreshAsync(entryId, cancellationToken);
        return Ok(new { Refreshed = true });
    }

    /// <summary>Gets FPL bootstrap-static data.</summary>
    [HttpGet("bootstrap")]
    public async Task<IActionResult> GetBootstrap(CancellationToken cancellationToken) =>
        Ok(await _service.GetBootstrapAsync(cancellationToken));

    /// <summary>Gets an individual player's summary, history, and upcoming fixtures.</summary>
    [HttpGet("players/{playerId:int}")]
    public async Task<IActionResult> GetPlayer(int playerId, CancellationToken cancellationToken) =>
        Ok(await _service.GetPlayerSummaryAsync(playerId, cancellationToken));

    /// <summary>Gets live player data for a gameweek.</summary>
    [HttpGet("gameweeks/{gameweek:int}/live")]
    public async Task<IActionResult> GetLivePlayers(int gameweek, CancellationToken cancellationToken) =>
        Ok(await _service.GetLivePlayerDataAsync(gameweek, cancellationToken));

    /// <summary>Gets the dream team for a gameweek.</summary>
    [HttpGet("gameweeks/{gameweek:int}/dream-team")]
    public async Task<IActionResult> GetDreamTeam(int gameweek, CancellationToken cancellationToken) =>
        Ok(await _service.GetDreamTeamAsync(gameweek, cancellationToken));

    /// <summary>Gets every fixture.</summary>
    [HttpGet("fixtures")]
    public async Task<IActionResult> GetFixtures(CancellationToken cancellationToken) =>
        Ok(await _service.GetFixturesAsync(cancellationToken));

    /// <summary>Gets fixtures for one gameweek.</summary>
    [HttpGet("gameweeks/{gameweek:int}/fixtures")]
    public async Task<IActionResult> GetFixturesByGameweek(int gameweek, CancellationToken cancellationToken) =>
        Ok(await _service.GetFixturesByGameweekAsync(gameweek, cancellationToken));

    /// <summary>Gets a fixture using its FPL fixture code.</summary>
    [HttpGet("fixtures/code/{code:int}")]
    public async Task<IActionResult> GetFixtureByCode(int code, CancellationToken cancellationToken)
    {
        var fixture = await _service.GetFixtureByCodeAsync(code, cancellationToken);
        return fixture is null ? NotFound() : Ok(fixture);
    }

    /// <summary>Gets a public manager entry.</summary>
    [HttpGet("managers/{entryId:int}")]
    public async Task<IActionResult> GetManager(int entryId, CancellationToken cancellationToken) =>
        Ok(await _service.GetEntryAsync(entryId, cancellationToken));

    /// <summary>Gets a manager's picks for a gameweek.</summary>
    [HttpGet("managers/{entryId:int}/gameweeks/{gameweek:int}/picks")]
    public async Task<IActionResult> GetPicks(int entryId, int gameweek, CancellationToken cancellationToken) =>
        Ok(await _service.GetPicksAsync(entryId, gameweek, cancellationToken));

    /// <summary>Gets a manager's transfer history.</summary>
    [HttpGet("managers/{entryId:int}/transfers")]
    public async Task<IActionResult> GetTransferHistory(int entryId, CancellationToken cancellationToken) =>
        Ok(await _service.GetTransferHistoryAsync(entryId, cancellationToken));

    /// <summary>Gets the authenticated manager's current team.</summary>
    [HttpGet("managers/{entryId:int}/my-team")]
    public async Task<IActionResult> GetMyTeam(int entryId, CancellationToken cancellationToken) =>
        Ok(await _service.GetMyTeamAsync(entryId, cancellationToken));

    /// <summary>Gets a manager's current-season and past-season history.</summary>
    [HttpGet("managers/{entryId:int}/history")]
    public async Task<IActionResult> GetSeasonHistory(int entryId, CancellationToken cancellationToken) =>
        Ok(await _service.GetSeasonHistoryAsync(entryId, cancellationToken));

    /// <summary>Gets the authenticated manager profile.</summary>
    [HttpGet("managers/{entryId:int}/me")]
    public async Task<IActionResult> GetCurrentManager(int entryId, CancellationToken cancellationToken) =>
        Ok(await _service.GetCurrentManagerAsync(entryId, cancellationToken));

    /// <summary>Gets all leagues associated with a manager.</summary>
    [HttpGet("managers/{entryId:int}/leagues")]
    public async Task<IActionResult> GetManagerLeagues(int entryId, CancellationToken cancellationToken) =>
        Ok(await _service.GetManagerLeaguesAsync(entryId, cancellationToken));

    /// <summary>Gets classic-league standings.</summary>
    [HttpGet("leagues/classic/{leagueId:int}")]
    public async Task<IActionResult> GetClassicLeague(int leagueId, [FromQuery] int page = 1, CancellationToken cancellationToken = default) =>
        Ok(await _service.GetClassicLeagueAsync(leagueId, page, cancellationToken));

    /// <summary>Gets head-to-head league standings.</summary>
    [HttpGet("leagues/h2h/{leagueId:int}")]
    public async Task<IActionResult> GetH2HLeague(int leagueId, [FromQuery] int page = 1, CancellationToken cancellationToken = default) =>
        Ok(await _service.GetH2HLeagueAsync(leagueId, page, cancellationToken));

    /// <summary>Gets head-to-head matches for a league and gameweek.</summary>
    [HttpGet("leagues/h2h/{leagueId:int}/gameweeks/{gameweek:int}/matches")]
    public async Task<IActionResult> GetH2HMatches(int leagueId, int gameweek, [FromQuery] int page = 1, CancellationToken cancellationToken = default) =>
        Ok(await _service.GetH2HMatchesAsync(leagueId, gameweek, page, cancellationToken));

    /// <summary>Submits the authenticated manager's lineup.</summary>
    [HttpPost("managers/{entryId:int}/lineup")]
    public async Task<IActionResult> SubmitLineup(int entryId, FplSubstitutionRequest request, CancellationToken cancellationToken) =>
        Ok(await _service.SubmitLineupAsync(entryId, request, cancellationToken));

    /// <summary>Submits transfers for the manager identified by the request's entry value.</summary>
    [HttpPost("transfers")]
    public async Task<IActionResult> SubmitTransfers(FplTransferRequest request, CancellationToken cancellationToken) =>
        Ok(await _service.SubmitTransfersAsync(request, cancellationToken));
}
