using FantasyPremierLeague;
using FantasyPremierLeague.Managers;
using FantasyPremierLeague.Playwright;
using FantasyPremierLeague.SampleApi.Services;
using FantasyPremierLeague.DependencyInjection;
using FantasyPremierLeague.Playwright.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);

// MVC controllers
builder.Services.AddControllers();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the core SDK
builder.Services.AddFantasyPremierLeague(options =>
{
    options.BaseAddress =
        new Uri("https://fantasy.premierleague.com/api/");

    options.Timeout = TimeSpan.FromSeconds(60);

    options.UserAgent =
        "FantasyPremierLeague.SampleApi/1.0";
});

// Register Playwright authentication
builder.Services.AddFantasyPremierLeaguePlaywright(options =>
{
    options.Headless = true;

    options.NavigationTimeout =
        TimeSpan.FromSeconds(0);

    options.InteractionTimeout =
        TimeSpan.FromSeconds(0);
});

// Replace this with your MongoDB, SQL, Redis,
// Cassandra, or other database implementation.
builder.Services.AddSingleton<
    IFplManagerStore,
    InMemoryFplManagerStore>();

// Sample application service
builder.Services.AddScoped<
    IFplManagerService,
    FplManagerService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();