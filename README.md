# FantasyPremierLeague.NET

[![Build](https://github.com/akin2unde/FantasyPremierLeague.NET/actions/workflows/ci.yml/badge.svg)](https://github.com/akin2unde/FantasyPremierLeague.NET/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/FantasyPremierLeague.NET.svg)](https://www.nuget.org/packages/FantasyPremierLeague.NET)
[![Downloads](https://img.shields.io/nuget/dt/FantasyPremierLeague.NET.svg)](https://www.nuget.org/packages/FantasyPremierLeague.NET)
[![Coverage](https://codecov.io/gh/akin2unde/FantasyPremierLeague.NET/graph/badge.svg)](https://codecov.io/gh/akin2unde/FantasyPremierLeague.NET)
[![License](https://img.shields.io/github/license/akin2unde/FantasyPremierLeague.NET)](LICENSE)

A modern, strongly typed **.NET 10 SDK** for interacting with the Fantasy Premier League API.

FantasyPremierLeague.NET provides a clean, dependency injection-friendly SDK that supports both public and authenticated Fantasy Premier League endpoints while allowing developers to store manager information in any database of their choice.

---

# Why FantasyPremierLeague.NET?

Most unofficial Fantasy Premier League libraries focus only on making HTTP requests.

FantasyPremierLeague.NET was designed to feel like a modern .NET SDK by providing:

* Strongly typed models
* Clean Dependency Injection
* Automatic authentication
* Automatic token reuse
* Automatic token refresh
* Pluggable persistence
* Extensible authentication providers
* Clean architecture
* XML documentation
* ASP.NET Core integration

---

# Features

* .NET 10
* Fully asynchronous API
* Strongly typed responses
* Built-in Dependency Injection
* Playwright authentication provider
* Automatic token reuse
* Automatic re-authentication
* Database-agnostic persistence
* XML documentation
* Modular architecture
* ASP.NET Core sample application
* Unit tests
* Open-source

---

# Installation

## Core SDK

```bash
dotnet add package FantasyPremierLeague.NET
```

## Playwright Authentication

```bash
dotnet add package FantasyPremierLeague.NET.Playwright
```

---

# Quick Start

Register the SDK

```csharp
builder.Services.AddFantasyPremierLeague(options =>
{
    options.BaseAddress = new Uri("https://fantasy.premierleague.com/api/");
});
```

Enable Playwright authentication

```csharp
builder.Services.AddFantasyPremierLeaguePlaywright(options =>
{
    options.Headless = true;
});
```

Register your manager persistence

```csharp
builder.Services.AddSingleton<
    IFplManagerStore,
    MongoFplManagerStore>();
```

Inject the SDK

```csharp
public class FplService
{
    private readonly FplClient _client;

    public FplService(FplClient client)
    {
        _client = client;
    }
}
```

---

# Architecture

```text
Application
      │
      ▼
 FplClient
      │
      ├──────────────┐
      │              │
 Players        Fixtures
 Managers       Leagues
 Team
      │
      ▼
 FplHttpClient
      │
      ▼
 Authentication Manager
      │
      ▼
 Manager Store
      │
      ▼
 MongoDB / SQL Server / Redis /
 PostgreSQL / Cassandra / etc.
```

---

# Authentication Flow

The SDK authenticates only when required.

```text
Login Requested
       │
       ▼
Check Manager Store
       │
       ├───────────────┐
       │               │
Found Manager?        No
       │               │
      Yes              ▼
       │         Authenticate
       │               │
Token Valid?           │
       │               │
 Yes ──┘               ▼
       │         Save Manager
       ▼               │
Reuse Token ◄──────────┘
```

Every authenticated manager is stored as an `FplManagerRecord`.

The SDK stores:

* Email
* Entry Id
* Access Token
* Expiry Date
* Manager Profile
* Entry Information

This allows the SDK to avoid unnecessary browser authentication.

---

# Example

Authenticate a manager

```csharp
await client.LoginAsync(
    "user@example.com",
    "password");
```

Retrieve bootstrap information

```csharp
var bootstrap =
    await client.Players.GetBootstrapAsync();
```

Retrieve player information

```csharp
var player =
    await client.Players.GetPlayerSummaryAsync(328);
```

Retrieve fixtures

```csharp
var fixtures =
    await client.Fixtures.GetFixturesAsync();
```

Retrieve a manager

```csharp
var manager =
    await client.Managers.GetEntryAsync(123456);
```

---

# Manager Persistence

The SDK never dictates how your application stores data.

Simply implement:

```csharp
public interface IFplManagerStore
```

The same SDK works with:

* MongoDB
* SQL Server
* PostgreSQL
* MySQL
* SQLite
* Redis
* Cassandra
* Cosmos DB
* In-Memory

---

# Projects

```text
FantasyPremierLeague.NET
│
├── src
│   ├── FantasyPremierLeague
│   └── FantasyPremierLeague.Playwright
│
├── samples
│   └── FantasyPremierLeague.SampleApi
│
├── tests
│
└── docs
```

---

# Project Structure

## FantasyPremierLeague

Contains

* Feature Clients
* Models
* HTTP Pipeline
* Authentication Contracts
* Manager Store Contracts
* Dependency Injection

This package has **no Playwright dependency**.

---

## FantasyPremierLeague.Playwright

Contains

* Playwright Login Provider
* Browser Authentication
* Token Extraction
* Authentication Registration

This package is optional.

---

## Sample API

Contains a working ASP.NET Core example demonstrating

* Dependency Injection
* Authentication
* Manager Persistence
* Controllers
* Services

---

# Current Endpoints

Public

* Bootstrap
* Players
* Fixtures
* Leagues

Authenticated

* Login
* Team
* Manager
* Transfers *(Work in Progress)*

---

# Roadmap

## v0.1

* Remaining Public Endpoints
* Better Exception Handling
* Improved XML Documentation

## v0.2

* MongoDB Package
* Redis Package
* SQL Server Package

## v0.3

* Polly Retry Policies
* Caching
* Better Logging

## v1.0

* Stable Public API
* Complete Endpoint Coverage
* NuGet Stable Release

---

# Documentation

Additional documentation is available inside the repository.

* CHANGELOG.md
* ROADMAP.md
* CONTRIBUTING.md

---

# Contributing

Contributions are welcome.

If you would like to contribute

1. Fork the repository
2. Create a feature branch
3. Submit a Pull Request

Please include tests and documentation with new features.

---

# Disclaimer

FantasyPremierLeague.NET is an unofficial SDK.

It is not affiliated with, endorsed by or sponsored by the Fantasy Premier League or the Premier League.

---

# License

MIT License

See the LICENSE file for details.

---

# Author

**Akintunde Morakinyo**

Senior Software Engineer

* .NET
* C#
* Angular
* React Native
* TypeScript

Building reusable software components and modern developer tools for the .NET ecosystem.
