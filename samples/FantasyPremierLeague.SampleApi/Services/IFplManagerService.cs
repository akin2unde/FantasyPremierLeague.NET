using FantasyPremierLeague.Managers;
using FantasyPremierLeague.Models.Bootstrap;

namespace FantasyPremierLeague.SampleApi.Services;

/// <summary>
/// Defines application-level operations for FPL managers.
/// </summary>
public interface IFplManagerService
{
    /// <summary>
    /// Authenticates a manager or restores a valid persisted session.
    /// </summary>
    Task<FplManagerRecord?> LoginAsync(
        string email,
        string password,
        bool includeDetails,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a persisted manager by entry identifier.
    /// </summary>
    Task<FplManagerRecord?> GetByEntryIdAsync(
        int entryId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets boosrap static file.
    /// </summary>
    Task<FplBootstrapStatic> GetBoostrapAsync(
        CancellationToken cancellationToken = default);
}
