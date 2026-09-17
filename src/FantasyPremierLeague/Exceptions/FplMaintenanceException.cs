namespace FantasyPremierLeague.Exceptions;
/// <summary>
/// Represents the period in which FPL is unavailable while the game is updated.
/// </summary>
public sealed class FplMaintenanceException : FplException
{
    /// <summary>
    /// Initializes the exception with the standard maintenance message.
    /// </summary>
    public FplMaintenanceException()
        : base("Fantasy Premier League is currently being updated.")
    {
    }
    /// <summary>
    /// Initializes the exception with a custom maintenance message.
    /// </summary>
    public FplMaintenanceException(string message)
        : base(message)
    {
    }
}
