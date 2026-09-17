namespace FantasyPremierLeague.Exceptions;
/// <summary>
/// Represents an FPL login or token-refresh failure.
/// </summary>

public sealed class FplAuthenticationException : FplException
{
    /// <summary>
    /// Initializes an authentication exception with an error message.
    /// </summary>
    public FplAuthenticationException(string message) : base(message) { }
    /// <summary>
    /// Initializes an authentication exception with an error message and underlying cause.
    /// </summary>
    public FplAuthenticationException(string message, Exception innerException) : base(message, innerException) { }
}
