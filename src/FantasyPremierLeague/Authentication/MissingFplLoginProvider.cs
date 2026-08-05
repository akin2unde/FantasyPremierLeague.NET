using FantasyPremierLeague.Exceptions;
namespace FantasyPremierLeague.Authentication;
internal sealed class MissingFplLoginProvider : IFplLoginProvider
{
    /// <summary>
    /// Provides the LoginAsync member.
    /// </summary>
    public Task<FplSession> LoginAsync(string email,string password,CancellationToken cancellationToken=default) =>
        throw new FplAuthenticationException("No login provider is registered. Install FantasyPremierLeague.NET.Playwright and call AddFantasyPremierLeaguePlaywright().");
}
