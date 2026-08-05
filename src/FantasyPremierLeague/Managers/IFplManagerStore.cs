namespace FantasyPremierLeague.Managers;

/// <summary>
/// Defines persistence operations for FPL manager records.
/// </summary>
public interface IFplManagerStore
{
    /// <summary>
    /// Retrieves a manager using an email address.
    /// </summary>
    Task<FplManagerRecord?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a manager using an FPL entry identifier.
    /// </summary>
    Task<FplManagerRecord?> GetByEntryIdAsync(
        int entryId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves or updates a manager record.
    /// </summary>
    Task SaveAsync(
        FplManagerRecord manager,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a manager record using an email address.
    /// </summary>
    Task RemoveAsync(
        string email,
        CancellationToken cancellationToken = default);
}