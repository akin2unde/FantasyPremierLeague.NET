namespace FantasyPremierLeague.Authentication;
/// <summary>
/// Provides the IFplLoginProvider member.
/// </summary>
public interface IFplLoginProvider
{
    /// <summary>
    /// Describes the LoginAsync member.
    /// </summary>
    Task<FplSession> LoginAsync(string email, string password, CancellationToken cancellationToken = default);
}
