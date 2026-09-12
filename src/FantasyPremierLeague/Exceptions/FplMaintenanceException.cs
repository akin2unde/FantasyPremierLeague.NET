namespace FantasyPremierLeague.Exceptions;
/// <summary>
/// Provides the FplMaintenanceException member.
/// </summary>
public sealed class FplMaintenanceException : Exception
{
    /// <summary>
    /// Describes the FplMaintenanceException member.
    /// </summary>
    public FplMaintenanceException()
        : base("Fantasy Premier League is currently being updated.")
    {
    }
    /// <summary>
    /// Describes the FplMaintenanceException member.
    /// </summary>
    public FplMaintenanceException(string message)
        : base(message)
    {
    }
}