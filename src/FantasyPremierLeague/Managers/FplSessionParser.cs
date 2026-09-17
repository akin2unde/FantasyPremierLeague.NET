using System.Text.Json;
using FantasyPremierLeague.Authentication;
using FantasyPremierLeague.Exceptions;

/// <summary>
/// Parses FPL OAuth token responses into <see cref="FplSession"/> instances.
/// </summary>
/// <remarks>
/// Access-token expiration is read from the access token's JWT
/// <c>exp</c> claim. If that claim cannot be read, absolute expiration
/// properties and then <c>expires_in</c> are used as fallbacks.
///
/// Refresh-token expiration is read from the refresh token's own JWT
/// <c>exp</c> claim.
/// </remarks>
public static class FplSessionParser
{
    /// <summary>
    /// Parses an FPL OAuth token response.
    /// </summary>
    /// <param name="body">
    /// The JSON response returned by the FPL token endpoint.
    /// </param>
    /// <param name="now">
    /// The current UTC time used when access-token expiration must be
    /// calculated from <c>expires_in</c>.
    /// </param>
    /// <returns>
    /// An <see cref="FplSession"/> containing the access token, refresh
    /// token, their expiration times, and the raw token response.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="body"/> is null, empty, or whitespace.
    /// </exception>
    /// <exception cref="JsonException">
    /// Thrown when <paramref name="body"/> is not valid JSON.
    /// </exception>
    /// <exception cref="FplAuthenticationException">
    /// Thrown when the response does not contain a usable
    /// <c>access_token</c>.
    /// </exception>
    public static FplSession Parse(
        string body,
        DateTimeOffset now)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(body);

        using var document = JsonDocument.Parse(body);
        var root = document.RootElement.Clone();

        if (!root.TryGetProperty(
                "access_token",
                out var accessTokenElement) ||
            string.IsNullOrWhiteSpace(accessTokenElement.GetString()))
        {
            throw new FplAuthenticationException(
                "The FPL token response did not contain an access_token.");
        }

        var accessToken = accessTokenElement.GetString()!;

        var refreshToken =
            root.TryGetProperty(
                "refresh_token",
                out var refreshTokenElement)
                ? refreshTokenElement.GetString()
                : null;

        var accessTokenExpiresAt = ResolveAccessTokenExpiration(
            root,
            accessToken,
            now);

        DateTimeOffset? refreshTokenExpiresAt = null;

        if (!string.IsNullOrWhiteSpace(refreshToken) &&
            TryReadTokenExpiration(
                refreshToken,
                out var parsedRefreshTokenExpiresAt))
        {
            refreshTokenExpiresAt = parsedRefreshTokenExpiresAt;
        }

        return new FplSession
        {
            AccessToken = accessToken,
            ExpiresAt = accessTokenExpiresAt,
            RefreshToken = refreshToken,
            RefreshTokenExpiresAt = refreshTokenExpiresAt,
            RawTokenResponse = root
        };
    }

    /// <summary>
    /// Resolves the access-token expiration using the JWT <c>exp</c>
    /// claim followed by response-level expiration fallbacks.
    /// </summary>
    /// <param name="root">
    /// The root JSON element from the token response.
    /// </param>
    /// <param name="accessToken">
    /// The access token returned by FPL.
    /// </param>
    /// <param name="now">
    /// The time used to calculate expiration from <c>expires_in</c>.
    /// </param>
    /// <returns>
    /// The resolved access-token expiration, or <see langword="null"/>
    /// when no supported expiration value is available.
    /// </returns>
    private static DateTimeOffset? ResolveAccessTokenExpiration(
        JsonElement root,
        string accessToken,
        DateTimeOffset now)
    {
        if (TryReadTokenExpiration(
                accessToken,
                out var jwtExpiresAt))
        {
            return jwtExpiresAt;
        }

        if (TryReadAbsolute(
                root,
                "expires_at",
                out var absoluteExpiresAt) ||
            TryReadAbsolute(
                root,
                "expired_at",
                out absoluteExpiresAt))
        {
            return absoluteExpiresAt;
        }

        if (root.TryGetProperty(
                "expires_in",
                out var expiresInElement) &&
            expiresInElement.ValueKind == JsonValueKind.Number &&
            expiresInElement.TryGetInt64(
                out var expiresInSeconds))
        {
            return now.AddSeconds(expiresInSeconds);
        }

        return null;
    }

    /// <summary>
    /// Attempts to read the expiration time from a JWT token's
    /// <c>exp</c> claim.
    /// </summary>
    /// <param name="token">
    /// The encoded JWT access token or refresh token.
    /// </param>
    /// <param name="expiresAt">
    /// When successful, contains the expiration as a UTC
    /// <see cref="DateTimeOffset"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the token contains a valid
    /// <c>exp</c> claim; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// This method decodes the JWT payload only. It does not validate
    /// the token's signature, issuer, audience, or other claims.
    /// </remarks>
    private static bool TryReadTokenExpiration(
        string token,
        out DateTimeOffset expiresAt)
    {
        expiresAt = default;

        try
        {
            var parts = token.Split('.');

            if (parts.Length != 3 ||
                string.IsNullOrWhiteSpace(parts[1]))
            {
                return false;
            }

            var payloadBytes = DecodeBase64Url(parts[1]);

            using var document =
                JsonDocument.Parse(payloadBytes);

            if (!document.RootElement.TryGetProperty(
                    "exp",
                    out var expElement))
            {
                return false;
            }

            long expiration;

            if (expElement.ValueKind == JsonValueKind.Number)
            {
                if (!expElement.TryGetInt64(out expiration))
                    return false;
            }
            else if (expElement.ValueKind == JsonValueKind.String)
            {
                if (!long.TryParse(
                        expElement.GetString(),
                        out expiration))
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            expiresAt =
                DateTimeOffset.FromUnixTimeSeconds(expiration);

            return true;
        }
        catch (FormatException)
        {
            return false;
        }
        catch (JsonException)
        {
            return false;
        }
        catch (ArgumentOutOfRangeException)
        {
            return false;
        }
    }

    /// <summary>
    /// Attempts to read an absolute expiration from a JSON property.
    /// </summary>
    private static bool TryReadAbsolute(
        JsonElement root,
        string propertyName,
        out DateTimeOffset expiresAt)
    {
        expiresAt = default;

        if (!root.TryGetProperty(
                propertyName,
                out var element))
        {
            return false;
        }

        if (element.ValueKind == JsonValueKind.String &&
            DateTimeOffset.TryParse(
                element.GetString(),
                out expiresAt))
        {
            return true;
        }

        if (element.ValueKind == JsonValueKind.Number &&
            element.TryGetInt64(out var unixSeconds))
        {
            try
            {
                expiresAt =
                    DateTimeOffset.FromUnixTimeSeconds(
                        unixSeconds);

                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }

        return false;
    }

    /// <summary>
    /// Decodes a Base64Url-encoded value.
    /// </summary>
    private static byte[] DecodeBase64Url(string value)
    {
        var base64 = value
            .Replace('-', '+')
            .Replace('_', '/');

        base64 = (base64.Length % 4) switch
        {
            0 => base64,
            2 => base64 + "==",
            3 => base64 + "=",
            _ => throw new FormatException(
                "Invalid Base64Url value.")
        };

        return Convert.FromBase64String(base64);
    }
}