using FantasyPremierLeague.SampleApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FantasyPremierLeague.SampleApi.Controllers;

/// <summary>
/// Exposes sample endpoints for authenticating and retrieving FPL managers.
/// </summary>


[Route("[controller]")]
[ApiController]
public sealed class FplManagerController : ControllerBase
{
    private readonly IFplManagerService _service;

    /// <summary>
    /// Initializes a new instance of the <see cref="FplManagerController"/> class.
    /// </summary>
    public FplManagerController(IFplManagerService service)
    {
        _service = service;
    }

    /// <summary>
    /// Authenticates an FPL manager or restores a valid persisted session.
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var manager = await _service.LoginAsync(
            request.Email,
            request.Password,
            request.ForceRefresh,
            request.IncludeDetails,
            cancellationToken);

        return Ok(manager);
    }

    /// <summary>
    /// Gets a persisted manager by FPL entry identifier.
    /// </summary>
    [HttpGet("GetByEntry/{entryId:int}")]
    public async Task<IActionResult> GetByEntry(
        int entryId,
        CancellationToken cancellationToken)
    {
        var manager = await _service.GetByEntryIdAsync(entryId, cancellationToken);
        return manager is null ? NotFound() : Ok(manager);
    }
    /// <summary>
    /// Gets a persisted manager by FPL entry identifier.
    /// </summary>
    [HttpGet("GetMyTeam/{entryId:int}")]
    public async Task<IActionResult> GetMyTeam(
        int entryId,
        CancellationToken cancellationToken)
    {
        var team = await _service.GetMyTeamAsync(entryId, cancellationToken);
        return team is null ? NotFound() : Ok(team);
    }


    /// <summary>
    /// Gets static boostrap data.
    /// </summary>
    [HttpGet("GetBoostrapData")]
    public async Task<IActionResult> GetBoostrapData(
        CancellationToken cancellationToken)
    {
        var manager = await _service.GetBoostrapAsync(cancellationToken);
        return manager is null ? NotFound() : Ok(manager);
    }

    /// <summary>
    /// Gets static boostrap data.
    /// </summary>
    [HttpGet("GetMyLeague/{managerId}")]
    public async Task<IActionResult> GetMyLeague(
        int managerId,
        CancellationToken cancellationToken)
    {
        var manager = await _service.GetMyLeagueAsync(managerId, cancellationToken);
        return manager is null ? NotFound() : Ok(manager);
    }

    /// <summary>
    /// Gets static boostrap data.
    /// </summary>
    [HttpGet("GetClassicLeague/{league}")]
    public async Task<IActionResult> GetClassicLeague(
        int league,
        CancellationToken cancellationToken)
    {
        var manager = await _service.GetClassicLeagueAsync(league, cancellationToken);
        return manager is null ? NotFound() : Ok(manager);
    }
    /// <summary>
    /// Gets static boostrap data.
    /// </summary>
    [HttpGet("GetH2HLeague/{league}")]
    public async Task<IActionResult> GetH2HLeague(
        int league,
        CancellationToken cancellationToken)
    {
        var manager = await _service.GetH2HLeagueAsync(league, cancellationToken);
        return manager is null ? NotFound() : Ok(manager);
    }
    /// <summary>
    /// Gets player's live data.
    /// </summary>
    [HttpGet("GetLivePlayerData/{gw}")]
    public async Task<IActionResult> GetLivePlayerData(
        int gw,
        CancellationToken cancellationToken)
    {
        var data = await _service.GetLivePlayerDataAsync(gw, cancellationToken);
        return Ok(data);
    }

    /// <summary>
    /// Gets dream team.
    /// </summary>
    [HttpGet("GetDreamTeamData/{gw}")]
    public async Task<IActionResult> GetDreamTeamData(
        int gw,
        CancellationToken cancellationToken)
    {
        var data = await _service.GetDreamTeamDataAsync(gw, cancellationToken);
        return Ok(data);
    }
}
