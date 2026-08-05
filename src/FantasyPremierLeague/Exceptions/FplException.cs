namespace FantasyPremierLeague.Exceptions;
/// <summary>
/// Provides the FplException member.
/// </summary>

public class FplException : Exception
{
    /// <summary>
    /// Describes the FplException member.
    /// </summary>
    public FplException(string message) : base(message) { }
    /// <summary>
    /// Describes the FplException member.
    /// </summary>
    public FplException(string message, Exception innerException) : base(message, innerException) { }
}
