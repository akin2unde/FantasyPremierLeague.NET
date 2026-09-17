# FantasyPremierLeague.NET

[![Build](https://github.com/akin2unde/FantasyPremierLeague.NET/actions/workflows/ci.yml/badge.svg)](https://github.com/akin2unde/FantasyPremierLeague.NET/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/FantasyPremierLeague.NET.svg)](https://www.nuget.org/packages/FantasyPremierLeague.NET)
[![Downloads](https://img.shields.io/nuget/dt/FantasyPremierLeague.NET.svg)](https://www.nuget.org/packages/FantasyPremierLeague.NET)
[![License](https://img.shields.io/github/license/akin2unde/FantasyPremierLeague.NET)](LICENSE)

A strongly typed, asynchronous .NET 10 SDK for the Fantasy Premier League API. It supports public data, authenticated manager operations, automatic access-token refresh, replaceable session persistence, and optional Playwright login.

> The Fantasy Premier League API is not an officially supported public developer API. Endpoints and authentication behaviour can change without notice. Use authenticated write operations carefully and comply with the Premier League's applicable terms.

## Features

- Strongly typed bootstrap, player, fixture, manager, league, pick, history, and live-gameweek models
- Public and authenticated FPL clients
- Playwright-based interactive login in an optional package
- Access-token expiration read from the JWT `exp` claim
- Refresh-token expiration read and stored separately
- Automatic access-token reuse and refresh
- Refresh-token rotation support
- Automatic authenticated-request retry after `401 Unauthorized`
- Pluggable manager/session persistence through `IFplManagerStore`
- Optional manager profile and entry loading
- Configurable detailed upstream errors
- Dependency-injection integration
- XML documentation and a complete ASP.NET Core sample API

## Packages

Install the core SDK:

```bash
dotnet add package FantasyPremierLeague.NET
```

Install Playwright authentication only when browser login is required:

```bash
dotnet add package FantasyPremierLeague.NET.Playwright
```

After installing the Playwright package, install Chromium for the target environment. From the build output directory, run the generated Playwright installer, for example:

```bash
pwsh bin/Debug/net10.0/playwright.ps1 install chromium
```

## Registration

```csharp
using FantasyPremierLeague.DependencyInjection;
using FantasyPremierLeague.Playwright.DependencyInjection;

builder.Services.AddFantasyPremierLeague(options =>
{
    options.BaseAddress =
        new Uri("https://fantasy.premierleague.com/api/");

    options.Timeout = TimeSpan.FromSeconds(60);
    options.RefreshBeforeExpiry = TimeSpan.FromMinutes(5);
    options.ReuseStoredToken = true;
    options.LoadProfileAfterLogin = true;

    // Keep false in production unless exact upstream messages are required
    // and the application protects them from logs and public responses.
    options.ExposeDetailedErrors = false;
});

builder.Services.AddFantasyPremierLeaguePlaywright(options =>
{
    options.Headless = true;
    options.NavigationTimeout = TimeSpan.FromSeconds(60);
    options.InteractionTimeout = TimeSpan.FromSeconds(30);
    options.ShowLog = false;
});
```

The core package does not depend on Playwright. Applications that already have another `IFplLoginProvider` can register that provider instead.

## Manager persistence

The SDK includes `InMemoryFplManagerStore` for development and tests. Production applications should implement `IFplManagerStore` using their database:

```csharp
public sealed class MongoFplManagerStore : IFplManagerStore
{
    public Task<FplManagerRecord?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        // Load by normalized email.
    }

    public Task<FplManagerRecord?> GetByEntryIdAsync(
        int entryId,
        CancellationToken cancellationToken = default)
    {
        // Load by FPL entry ID.
    }

    public Task SaveAsync(
        FplManagerRecord manager,
        CancellationToken cancellationToken = default)
    {
        // Insert or replace the session.
    }

    public Task RemoveAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        // Delete or revoke the stored session.
    }
}
```

Register it with:

```csharp
builder.Services
    .AddFantasyPremierLeagueManagerStore<MongoFplManagerStore>();
```

Persist at least these `FplManagerRecord` fields:

- `Email`
- `EntryId`
- `AccessToken`
- `TokenExpiresAt`
- `RefreshToken`
- `RefreshTokenExpiresAt`
- `UpdatedAt`
- `Profile` and `Entry` when required

Access and refresh tokens are credentials. Encrypt them at rest, exclude them from application logs, and never return them from a public API unless that is an intentional design decision. The SDK does not need to persist the manager's password.

## Authentication and refresh flow

```text
LoginAsync
  ├─ usable stored access token → reuse it
  ├─ usable stored refresh token → request new tokens
  └─ no usable session → run the configured login provider
```

The FPL access token normally lasts one hour. The SDK reads the absolute `exp` value from the access-token JWT rather than assuming the duration. It reads the refresh token's separate `exp` claim into `RefreshTokenExpiresAt`.

When FPL returns a replacement refresh token, the SDK stores it immediately. When the refresh response omits a refresh token, the SDK preserves the existing token and expiration.

Authenticate:

```csharp
var manager = await client.LoginAsync(
    email: "user@example.com",
    password: "password",
    forceRefresh: false,
    includeDetails: true,
    cancellationToken);
```

Explicitly refresh the selected manager:

```csharp
client.SetFoundRecord(manager);

var accessToken = await client.RefreshCurrentSessionAsync(
    cancellationToken);
```

In an ASP.NET Core application, `FplClient` is scoped. Before an authenticated call in a later HTTP request, load the correct manager from your `IFplManagerStore` and select it:

```csharp
var manager = await managerStore.GetByEntryIdAsync(
    entryId,
    cancellationToken);

if (manager is null)
    throw new InvalidOperationException("Log in first.");

client.SetFoundRecord(manager);

var team = await client.Managers.GetMyTeamAsync(
    entryId,
    cancellationToken);
```

## Client API

Inject `FplClient` and use its feature clients.

### Bootstrap and players

```csharp
var bootstrap = await client.Bootstrap.GetDataAsync(cancellationToken);
var player = await client.Players.GetPlayerSummaryAsync(playerId, cancellationToken);
var live = await client.Players.GetPlayerLiveAsync(gameweek, cancellationToken);
var dreamTeam = await client.Players.GetGWDreamTeamAsync(gameweek, cancellationToken);
```

`client.Boostrap` remains as an obsolete compatibility alias. New code should use `client.Bootstrap`.

### Fixtures

```csharp
var allFixtures = await client.Fixtures.GetAllAsync(cancellationToken);
var gameweekFixtures = await client.Fixtures.GetByGWAsync(gameweek, cancellationToken);
var fixture = await client.Fixtures.GetByCodeAsync(fixtureCode, cancellationToken);
```

### Managers

```csharp
var entry = await client.Managers.GetEntryAsync(entryId, cancellationToken);
var picks = await client.Managers.GetPicksAsync(entryId, gameweek, cancellationToken);
var transfers = await client.Managers.GetManagerTransferHistoryAsync(entryId, cancellationToken);
var history = await client.Managers.GetMyGWHistoryAsync(entryId, cancellationToken);

// Authenticated
var me = await client.Managers.GetCurrentAsync(cancellationToken);
var myTeam = await client.Managers.GetMyTeamAsync(entryId, cancellationToken);
```

### Leagues

```csharp
var leagues = await client.Leagues.GetMyLeagueAsync(entryId, cancellationToken);
var classic = await client.Leagues.GetClassicStandingsAsync(leagueId, page, cancellationToken);
var h2h = await client.Leagues.GetH2HStandingsAsync(leagueId, page, cancellationToken);
var matches = await client.Leagues.GetH2HFixtureAsync(leagueId, gameweek, page, cancellationToken);
```

### Lineup and transfers

These methods modify the authenticated manager's FPL team.

```csharp
var lineupResult = await client.Team.SubmitLineupAsync(
    entryId,
    new FplSubstitutionRequest
    {
        Chip = null,
        Picks = picks
    },
    cancellationToken);

var transferResult = await client.Team.SubmitTransfersAsync(
    new FplTransferRequest
    {
        Entry = entryId,
        Event = gameweek,
        Chip = null,
        Transfers = transfers
    },
    cancellationToken);
```

## Available SDK operations

| Area | SDK method | Authentication |
| --- | --- | --- |
| Authentication | `LoginAsync` | Browser login or stored session |
| Authentication | `RefreshCurrentSessionAsync` | Refresh token |
| Authentication | `LogoutAsync` | Stored session |
| Bootstrap | `Bootstrap.GetDataAsync` | Public |
| Players | `Players.GetBootstrapAsync` | Public |
| Players | `Players.GetPlayerSummaryAsync` | Public |
| Players | `Players.GetPlayerLiveAsync` | Public |
| Players | `Players.GetGWDreamTeamAsync` | Public |
| Fixtures | `Fixtures.GetAllAsync` | Public |
| Fixtures | `Fixtures.GetByGWAsync` | Public |
| Fixtures | `Fixtures.GetByCodeAsync` | Public |
| Managers | `Managers.GetEntryAsync` | Public |
| Managers | `Managers.GetPicksAsync` | Public |
| Managers | `Managers.GetManagerTransferHistoryAsync` | Public |
| Managers | `Managers.GetMyGWHistoryAsync` | Public |
| Managers | `Managers.GetMyTeamAsync` | Authenticated |
| Managers | `Managers.GetCurrentAsync` | Authenticated |
| Leagues | `Leagues.GetMyLeagueAsync` | Public |
| Leagues | `Leagues.GetClassicStandingsAsync` | Public |
| Leagues | `Leagues.GetH2HStandingsAsync` | Public |
| Leagues | `Leagues.GetH2HFixtureAsync` | Public |
| Team | `Team.SubmitLineupAsync` | Authenticated write |
| Team | `Team.SubmitTransfersAsync` | Authenticated write |

## Detailed errors

By default, exceptions describe the failed operation without copying an upstream response body into the exception message:

```csharp
options.ExposeDetailedErrors = false;
```

An internal or development application can opt into exact upstream details:

```csharp
options.ExposeDetailedErrors = true;
```

Failed FPL requests throw `FplException`. It exposes:

- `StatusCode`
- `RequestPath`
- `ResponseBody` when detailed errors are enabled

Authentication failures throw `FplAuthenticationException`, and FPL maintenance responses throw `FplMaintenanceException`. The sample converts these exceptions to RFC 7807 `ProblemDetails` responses.

Do not expose detailed authentication errors or response bodies to untrusted callers without sanitizing them.

## Sample API

The project at `samples/FantasyPremierLeague.SampleApi` is a runnable ASP.NET Core reference application. Swagger documents all SDK operations, including login, refresh, logout, bootstrap, players, fixtures, manager picks/history/transfers, leagues, lineup submission, and transfer submission.

Run it:

```bash
dotnet run --project samples/FantasyPremierLeague.SampleApi
```

Open the Swagger URL printed by ASP.NET Core, authenticate with `POST /api/fpl/authentication/login`, then use the returned `EntryId` for authenticated sample routes.

## Error handling example

```csharp
try
{
    var picks = await client.Managers.GetPicksAsync(
        entryId,
        gameweek,
        cancellationToken);
}
catch (FplMaintenanceException)
{
    // FPL is currently being updated.
}
catch (FplAuthenticationException exception)
{
    // Login or refresh failed.
}
catch (FplException exception)
{
    logger.LogWarning(
        "FPL request {Path} failed with {StatusCode}",
        exception.RequestPath,
        exception.StatusCode);
}
```

## Project structure

```text
FantasyPremierLeague.NET
├── src
│   ├── FantasyPremierLeague
│   └── FantasyPremierLeague.Playwright
├── samples
│   └── FantasyPremierLeague.SampleApi
├── tests
├── CHANGELOG.md
├── CONTRIBUTING.md
└── ROADMAP.md
```

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md). When an FPL response changes, include a sanitized response sample and tests for the affected model or parser.

## License

Licensed under the [MIT License](LICENSE).
