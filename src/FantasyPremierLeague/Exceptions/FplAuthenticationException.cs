namespace FantasyPremierLeague.Exceptions;
/// <summary>
/// Provides the FplAuthenticationException member.
/// </summary>

public sealed class FplAuthenticationException : FplException
{
    /// <summary>
    /// Describes the FplAuthenticationException member.
    /// </summary>
    public FplAuthenticationException(string message) : base(message) { }
    /// <summary>
    /// Describes the FplAuthenticationException member.
    /// </summary>
    public FplAuthenticationException(string message, Exception innerException) : base(message, innerException) { }
}
