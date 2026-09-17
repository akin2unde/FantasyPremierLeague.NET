# Changelog

## Unreleased

- Add OAuth refresh-token exchange through `https://account.premierleague.com/as/token`.
- Read access-token and refresh-token expiration from their JWT `exp` claims.
- Preserve or rotate refresh tokens correctly after refresh.
- Add explicit current-session refresh support on `FplClient`.
- Add configurable exact upstream error reporting through `ExposeDetailedErrors`.
- Add structured FPL HTTP failure metadata to `FplException`.
- Add a correctly named `FplBootstrapClient` and preserve the original misspelled API as a compatibility alias.
- Expose every SDK operation through the ASP.NET Core sample and Swagger.
- Restore persisted manager context before authenticated sample calls.
- Add RFC 7807 error responses to the sample.
- Expand setup, security, authentication, endpoint, persistence, and error-handling documentation.
- Add parser tests for access-token and refresh-token expiration.

## 0.2.0
- Target .NET 10.
- Add database-agnostic manager persistence.
- Split all grouped models into one class per file.
- Add service/controller sample.
