using System.Text.Json;
using FantasyPremierLeague.Authentication;
using FantasyPremierLeague.Exceptions;

namespace FantasyPremierLeague.Playwright.Internal;

internal static class FplTokenResponseParser
{
    /// <summary>
    /// Provides the Parse member.
    /// </summary>
    public static FplSession Parse(string body, DateTimeOffset now)
    {
        using var document = JsonDocument.Parse(body);
        var root = document.RootElement.Clone();

        if (!root.TryGetProperty("access_token", out var accessTokenElement) ||
            string.IsNullOrWhiteSpace(accessTokenElement.GetString()))
            throw new FplAuthenticationException("The FPL token response did not contain an access_token.");

        DateTimeOffset? expiresAt = null;
        if (TryReadAbsolute(root, "expired_at", out var expiredAt) ||
            TryReadAbsolute(root, "expires_at", out expiredAt))
        {
            expiresAt = expiredAt;
        }
        else if (root.TryGetProperty("expires_in", out var expiresIn) && expiresIn.ValueKind == JsonValueKind.Number)
        {
            expiresAt = now.AddSeconds(expiresIn.GetInt32());
        }

        return new FplSession
        {
            AccessToken = accessTokenElement.GetString()!,
            ExpiresAt = expiresAt,
            RefreshToken = root.TryGetProperty("refresh_token", out var refresh) ? refresh.GetString() : null,
            RawTokenResponse = root
        };
    }

    private static bool TryReadAbsolute(JsonElement root, string property, out DateTimeOffset value)
    {
        value = default;
        if (!root.TryGetProperty(property, out var element)) return false;

        if (element.ValueKind == JsonValueKind.Number && element.TryGetInt64(out var unix))
        {
            value = DateTimeOffset.FromUnixTimeSeconds(unix);
            return true;
        }

        return element.ValueKind == JsonValueKind.String &&
               DateTimeOffset.TryParse(element.GetString(), out value);
    }
}
