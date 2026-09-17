using FantasyPremierLeague.Authentication;
using FantasyPremierLeague.Clients;
using FantasyPremierLeague.Http;
using FantasyPremierLeague.Managers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
namespace FantasyPremierLeague.DependencyInjection;
/// <summary>
/// Provides dependency-injection registration for the core FPL SDK.
/// </summary>
public static class FantasyPremierLeagueServiceCollectionExtensions
{
    /// <summary>
    /// Registers the FPL clients, authentication manager, HTTP pipeline, and default in-memory store.
    /// </summary>
    public static IServiceCollection AddFantasyPremierLeague(
        this IServiceCollection services,
        Action<FplOptions>? configure = null)
    {
        services.AddOptions<FplOptions>();

        if (configure is not null)
        {
            services.Configure(configure);
        }

        services.TryAddSingleton<IFplManagerStore, InMemoryFplManagerStore>();
        services.TryAddSingleton<IFplLoginProvider, MissingFplLoginProvider>();
        services.AddScoped<
         ILoadProfile,
         LoadProfile>();

        services.AddScoped<
            IFplAuthenticationManager,
            FplAuthenticationManager>();

        services.AddHttpClient(
            "FantasyPremierLeague",
            (serviceProvider, client) =>
            {
                var options = serviceProvider
                    .GetRequiredService<IOptions<FplOptions>>()
                    .Value;

                client.BaseAddress = options.BaseAddress;
                client.Timeout = options.Timeout;

                client.DefaultRequestHeaders.UserAgent.ParseAdd(
                    options.UserAgent);
            });

        services.AddScoped(serviceProvider =>
        {
            var httpClientFactory = serviceProvider
                .GetRequiredService<IHttpClientFactory>();

            var authenticationManager = serviceProvider
                .GetRequiredService<IFplAuthenticationManager>();

            return new FplHttpClient(
                httpClientFactory.CreateClient("FantasyPremierLeague"),
                authenticationManager,
                serviceProvider.GetRequiredService<IOptions<FplOptions>>());
        });
        services.AddScoped<FplBootstrapClient>();
        services.AddScoped<FplBoostrapClient>();
        services.AddScoped<FplPlayersClient>();
        services.AddScoped<FplFixturesClient>();
        services.AddScoped<FplManagersClient>();
        services.AddScoped<FplLeaguesClient>();
        services.AddScoped<FplTeamClient>();

        services.AddScoped<FplClient>();

        return services;
    }
    /// <summary>
    /// Replaces the default manager store with an application-provided implementation.
    /// </summary>
    public static IServiceCollection AddFantasyPremierLeagueManagerStore<TStore>(this IServiceCollection services) where TStore : class, IFplManagerStore
    { services.Replace(ServiceDescriptor.Singleton<IFplManagerStore, TStore>()); return services; }
}
