# FantasyPremierLeague.NET

> A modern, strongly typed **.NET 10** SDK for the Fantasy Premier League API with optional Playwright authentication, automatic token reuse, and database-agnostic manager persistence.

![.NET](https://img.shields.io/badge/.NET-10-blue)
![License](https://img.shields.io/badge/license-MIT-green)
![Status](https://img.shields.io/badge/status-Preview-orange)
![Platform](https://img.shields.io/badge/platform-Windows%20%7C%20Linux%20%7C%20macOS-lightgrey)

[![Build and Test](https://github.com/akin2unde/FantasyPremierLeague.NET/actions/workflows/ci.yml/badge.svg)](https://github.com/akin2unde/FantasyPremierLeague.NET/actions/workflows/ci.yml)

[![codecov](https://codecov.io/gh/akin2unde/FantasyPremierLeague.NET/graph/badge.svg)](https://codecov.io/gh/akin2unde/FantasyPremierLeague.NET)

[![NuGet](https://img.shields.io/nuget/v/FantasyPremierLeague.NET.svg)](https://www.nuget.org/packages/FantasyPremierLeague.NET)

[![NuGet Downloads](https://img.shields.io/nuget/dt/FantasyPremierLeague.NET.svg)](https://www.nuget.org/packages/FantasyPremierLeague.NET)

[![Playwright NuGet](https://img.shields.io/nuget/v/FantasyPremierLeague.NET.Playwright.svg)](https://www.nuget.org/packages/FantasyPremierLeague.NET.Playwright)

[![License](https://img.shields.io/github/license/akin2unde/FantasyPremierLeague.NET.svg)](LICENSE)

---

## Overview

FantasyPremierLeague.NET provides a clean, strongly typed and extensible SDK for interacting with the Fantasy Premier League platform from .NET applications.

Unlike most unofficial wrappers, this SDK has been designed around modern .NET architecture and dependency injection. Authentication, persistence and HTTP communication are separated into reusable components, allowing developers to integrate the SDK into ASP.NET Core applications with minimal configuration.

The SDK automatically reuses previously authenticated sessions, reducing unnecessary browser automation while allowing developers to store manager information in any database of their choice.

---

# Features

* Strongly typed .NET API
* Built for **.NET 10**
* Native Dependency Injection support
* Optional Playwright authentication
* Automatic access token reuse
* Automatic re-authentication when tokens expire
* Database-agnostic persistence
* Public and authenticated API endpoints
* Modular architecture
* XML documentation
* ASP.NET Core sample application
* Designed for NuGet distribution

---

# Solution Structure

```text
FantasyPremierLeague.NET
│
├── src
│   ├── FantasyPremierLeague
│   │
│   └── FantasyPremierLeague.Playwright
│
├── samples
│   └── FantasyPremierLeague.SampleApi
│
├── tests
│
└── README.md
```

## Projects

### FantasyPremierLeague

The core SDK.

Contains:

* HTTP infrastructure
* Feature clients
* Models
* Authentication abstraction
* Manager persistence abstraction
* Dependency Injection extensions

This project has **no Playwright dependency**.

---

### FantasyPremierLeague.Playwright

Provides browser-based authentication using Microsoft Playwright.

This package is completely optional.

Install it only if your application requires authenticated Fantasy Premier League operations.

---

### FantasyPremierLeague.SampleApi

A complete ASP.NET Core sample demonstrating:

* Dependency Injection
* Authentication
* Manager persistence
* Service layer
* Controller layer

---

# Architecture

```text
Application
      │
      ▼
FplClient
      │
      ├── Players
      ├── Fixtures
      ├── Managers
      ├── Leagues
      └── Team
             │
             ▼
      FplHttpClient
             │
             ▼
IFplAuthenticationManager
             │
             ▼
IFplManagerStore
             │
             ▼
MongoDB / SQL Server / Redis / Cassandra / Any Database
```

---

# Authentication Flow

The SDK authenticates only when necessary.

```text
Login Requested
        │
        ▼
Check IFplManagerStore
        │
        ├───────────────┐
        │               │
 Manager Found?        No
        │               │
       Yes              ▼
        │         Authenticate
        │               │
Token Still Valid?      │
        │               │
   Yes ─┘               ▼
        │        Save Manager
        ▼               │
Reuse Stored Token ◄────┘
```

Every authenticated manager is stored as an `FplManagerRecord` containing:

* Email
* Entry ID
* Access Token
* Token Expiry
* Manager Profile
* Entry Information

This allows applications to avoid repeated browser logins until the stored token expires.

---

# Getting Started

## Register the SDK

```csharp
builder.Services.AddFantasyPremierLeague(options =>
{
    options.RefreshBeforeExpiry = TimeSpan.FromMinutes(2);
});
```

## Enable Playwright Authentication

```csharp
builder.Services.AddFantasyPremierLeaguePlaywright(options =>
{
    options.Headless = true;
});
```

## Register Your Manager Store

```csharp
builder.Services.AddSingleton<
    IFplManagerStore,
    MyMongoManagerStore>();
```

Replace `MyMongoManagerStore` with your own implementation backed by MongoDB, SQL Server, Redis, PostgreSQL, Cassandra, or any other persistence technology.

---

# Example

```csharp
public sealed class FplManagerService
{
    private readonly FplClient _fpl;

    public FplManagerService(FplClient fpl)
    {
        _fpl = fpl;
    }

    public Task<FplManagerRecord> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken)
    {
        return _fpl.LoginAsync(
            email,
            password,
            cancellationToken);
    }
}
```

The SDK will:

1. Look up the manager by email.
2. Reuse the stored token if it is still valid.
3. Authenticate only when necessary.
4. Update the stored manager record.
5. Use the stored token for authenticated requests.

---

# Persistence

The SDK never assumes how data should be stored.

Implement the `IFplManagerStore` interface using any persistence technology.

Examples include:

* MongoDB
* SQL Server
* PostgreSQL
* MySQL
* SQLite
* Redis
* Cassandra
* Cosmos DB

---

# Roadmap

Current version:

**v0.0.1 (Preview)**

Planned improvements include:

* Additional Fantasy Premier League endpoints
* Improved transfer APIs
* League administration
* Automatic retry policies
* Improved caching
* Source Generator support
* Native AOT compatibility
* NuGet publication
* Additional authentication providers
* Comprehensive unit and integration tests

---

# Contributing

Contributions, suggestions and issue reports are welcome.

If you would like to contribute:

1. Fork the repository.
2. Create a feature branch.
3. Submit a pull request.

Please ensure new features include tests and appropriate documentation.

---

# Disclaimer

FantasyPremierLeague.NET is an unofficial community SDK.

It is not affiliated with, endorsed by, or sponsored by the Fantasy Premier League or the Premier League.

Users are responsible for ensuring their usage complies with the Fantasy Premier League terms of service.

---

# License

This project is licensed under the MIT License.

See the `LICENSE` file for details.

---

## Author

**Akintunde Morakinyo**

Senior Software Engineer | .NET | C# | Angular | React Native | TypeScript

Building modern, reusable developer tools and scalable enterprise software.
