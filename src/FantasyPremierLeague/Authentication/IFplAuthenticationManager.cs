using FantasyPremierLeague.Managers;
namespace FantasyPremierLeague.Authentication;
/// <summary>
/// Coordinates manager authentication and token persistence.
/// </summary>
public interface IFplAuthenticationManager
{
    /// <summary>
    /// Gets and Sets current manager
    /// </summary>
    FplManagerRecord? CurrentManager { get; set; }
    /// <summary>
    /// Provides the LoginAsync member.
    /// </summary>
    Task<FplManagerRecord> LoginAsync(string email, string password, bool forceRefresh, bool includeDetails, CancellationToken cancellationToken);
    /// <summary>
    /// Describes the GetCurrentAccessTokenAsync member.
    /// </summary>
    Task<string> GetCurrentAccessTokenAsync(CancellationToken cancellationToken);
    /// <summary>
    /// Describes the RefreshCurrentAsync member.
    /// </summary>
    Task<string> RefreshCurrentAsync(CancellationToken cancellationToken);
    /// <summary>
    /// Describes the SaveCurrentAsync member.
    /// </summary>
    Task SaveCurrentAsync(FplManagerRecord manager, CancellationToken cancellationToken);
    /// <summary>
    /// Provides the InvalidateCurrentAsync member.
    /// </summary>
    Task InvalidateCurrentAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Removes the stored authentication information for a manager.
    /// </summary>
    /// <param name="email">The manager's email address.</param>
    /// <param name="cancellationToken">
    /// The token used to cancel the operation.
    /// </param>
    Task LogoutAsync(
        string email,
        CancellationToken cancellationToken = default);
}
