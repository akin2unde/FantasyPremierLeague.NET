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
    [HttpGet("{entryId:int}")]
    public async Task<IActionResult> Get(
        int entryId,
        CancellationToken cancellationToken)
    {
        var manager = await _service.GetByEntryIdAsync(entryId, cancellationToken);
        return manager is null ? NotFound() : Ok(manager);
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
}
