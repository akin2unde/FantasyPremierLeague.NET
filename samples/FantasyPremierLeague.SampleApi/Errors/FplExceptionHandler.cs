using FantasyPremierLeague.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FantasyPremierLeague.SampleApi.Errors;

/// <summary>
/// Converts SDK exceptions into consistent RFC 7807 API responses.
/// </summary>
public sealed class FplExceptionHandler : IExceptionHandler
{
    /// <inheritdoc />
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (status, title) = exception switch
        {
            FplMaintenanceException =>
                (StatusCodes.Status503ServiceUnavailable, "FPL is under maintenance"),
            FplAuthenticationException =>
                (StatusCodes.Status401Unauthorized, "FPL authentication failed"),
            FplException { StatusCode: >= 400 and <= 599 } fplException =>
                (fplException.StatusCode.GetValueOrDefault(), "FPL request failed"),
            KeyNotFoundException =>
                (StatusCodes.Status404NotFound, "Manager session not found"),
            ArgumentException =>
                (StatusCodes.Status400BadRequest, "Invalid request"),
            _ =>
                (StatusCodes.Status500InternalServerError, "Unexpected server error")
        };

        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = exception is FplException or ArgumentException or KeyNotFoundException
                ? exception.Message
                : "The request could not be completed.",
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = status;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
        return true;
    }
}
