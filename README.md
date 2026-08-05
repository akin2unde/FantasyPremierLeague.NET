# FantasyPremierLeague.NET

A strongly typed **.NET 10** SDK for Fantasy Premier League, with optional Playwright authentication and database-agnostic manager persistence.

## Projects

- `FantasyPremierLeague` — core SDK; no Playwright dependency.
- `FantasyPremierLeague.Playwright` — optional browser authentication provider.
- `FantasyPremierLeague.SampleApi` — controller/service/persistence example.

## Authentication and manager persistence

The SDK calls your `IFplManagerStore` before authenticating:

1. Look up the manager by normalized email.
2. If the manager exists and `TokenExpiresAt` is still valid (including the configured safety buffer), reuse the saved token.
3. Otherwise authenticate through the configured provider.
4. Fetch the manager profile and entry data.
5. Save the full `FplManagerRecord`, including email, entry ID, token, token expiry, profile and entry object.

Implement `IFplManagerStore` using MongoDB, SQL Server, Cassandra, Redis, or any database of your choice, then register it:

```csharp
builder.Services
    .AddFantasyPremierLeague(options =>
        options.RefreshBeforeExpiry = TimeSpan.FromMinutes(2))
    .AddFantasyPremierLeagueManagerStore<MyMongoManagerStore>();

builder.Services.AddFantasyPremierLeaguePlaywright(options =>
    options.Headless = true);
```

## Usage from a service

```csharp
public sealed class FplManagerService(FplClient fpl)
{
    public Task<FplManagerRecord> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken) =>
        fpl.LoginAsync(email, password, cancellationToken: cancellationToken);
}
```

The first call authenticates and saves the manager. Later calls reuse the stored token until it is close to expiry.

> This is an unofficial integration. Confirm that your use complies with the relevant service terms and security requirements.
