namespace FantasyPremierLeague.Exceptions;
/// <summary>
/// Represents an error returned by, or encountered while calling, FPL.
/// </summary>

public class FplException : Exception
{
    /// <summary>
    /// Initializes an exception with an error message.
    /// </summary>
    public FplException(string message) : base(message) { }
    /// <summary>
    /// Initializes an exception with an error message and underlying cause.
    /// </summary>
    public FplException(string message, Exception innerException) : base(message, innerException) { }

    /// <summary>
    /// Initializes a new instance with details about a failed FPL HTTP request.
    /// </summary>
    public FplException(
        string message,
        int? statusCode,
        string? requestPath,
        string? responseBody = null,
        Exception? innerException = null)
        : base(message, innerException)
    {
        StatusCode = statusCode;
        RequestPath = requestPath;
        ResponseBody = responseBody;
    }

    /// <summary>
    /// Gets the upstream HTTP status code when the exception represents an HTTP failure.
    /// </summary>
    public int? StatusCode { get; }

    /// <summary>
    /// Gets the relative or absolute request path that failed.
    /// </summary>
    public string? RequestPath { get; }

    /// <summary>
    /// Gets the exact upstream response body when detailed errors are enabled.
    /// </summary>
    public string? ResponseBody { get; }
}
