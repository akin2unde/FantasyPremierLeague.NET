using FantasyPremierLeague.Authentication;
using FantasyPremierLeague.Playwright.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
namespace FantasyPremierLeague.Playwright.DependencyInjection;
/// <summary>
/// Provides the FantasyPremierLeaguePlaywrightExtensions member.
/// </summary>
public static class FantasyPremierLeaguePlaywrightExtensions
{
    /// <summary>
    /// Adds Playwright-based Fantasy Premier League authentication.
    /// </summary>
    /// <param name="services">
    /// The service collection.
    /// </param>
    /// <param name="configure">
    /// An optional action used to configure Playwright authentication.
    /// </param>
    /// <returns>
    /// The same service collection so additional registrations can be chained.
    /// </returns>
    public static IServiceCollection AddFantasyPremierLeaguePlaywright(
        this IServiceCollection services,
        Action<FplPlaywrightOptions>? configure = null)
    {
        services.AddOptions<FplPlaywrightOptions>();

        if (configure is not null)
        {
            services.Configure(configure);
        }

        services.Replace(
            ServiceDescriptor.Singleton<
                IFplLoginProvider,
                PlaywrightFplLoginProvider>());

        return services;
    }
}
