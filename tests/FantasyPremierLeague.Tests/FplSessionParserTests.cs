using System.Text;
using System.Text.Json;
using FantasyPremierLeague.Managers;

namespace FantasyPremierLeague.Tests;

public sealed class FplSessionParserTests
{
    [Fact]
    public void Parse_ReadsAccessAndRefreshTokenExpirationsFromJwtClaims()
    {
        var accessExpiration = DateTimeOffset.Parse("2026-09-17T12:00:00Z");
        var refreshExpiration = DateTimeOffset.Parse("2027-03-17T12:00:00Z");
        var body = JsonSerializer.Serialize(new
        {
            access_token = CreateUnsignedJwt(accessExpiration),
            refresh_token = CreateUnsignedJwt(refreshExpiration),
            expires_in = 60
        });

        var session = FplSessionParser.Parse(
            body,
            DateTimeOffset.Parse("2026-09-17T10:00:00Z"));

        Assert.Equal(accessExpiration, session.ExpiresAt);
        Assert.Equal(refreshExpiration, session.RefreshTokenExpiresAt);
    }

    [Fact]
    public void Parse_UsesExpiresInWhenAccessTokenHasNoExpClaim()
    {
        var now = DateTimeOffset.Parse("2026-09-17T10:00:00Z");
        var body = JsonSerializer.Serialize(new
        {
            access_token = CreateUnsignedJwt(null),
            expires_in = 3600
        });

        var session = FplSessionParser.Parse(body, now);

        Assert.Equal(now.AddHours(1), session.ExpiresAt);
        Assert.Null(session.RefreshTokenExpiresAt);
    }

    private static string CreateUnsignedJwt(DateTimeOffset? expiration)
    {
        var header = EncodeBase64Url("{\"alg\":\"none\"}");
        var payload = expiration is null
            ? "{}"
            : JsonSerializer.Serialize(
                new { exp = expiration.Value.ToUnixTimeSeconds() });

        return $"{header}.{EncodeBase64Url(payload)}.";
    }

    private static string EncodeBase64Url(string value) =>
        Convert.ToBase64String(Encoding.UTF8.GetBytes(value))
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
}
