using System.Text.Json;
namespace FantasyPremierLeague.Authentication;
/// <summary>
/// Represents access and refresh tokens returned by FPL authentication.
/// </summary>
public sealed class FplSession
{
    /// <summary>
    /// Gets the bearer access token.
    /// </summary>
    public required string AccessToken { get; init; }
    /// <summary>
    /// Gets the UTC time at which the access token expires.
    /// </summary>
    public DateTimeOffset? ExpiresAt { get; init; }
    /// <summary>
    /// Gets or sets the refresh token used to obtain a new access token.
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Gets the raw token response for advanced diagnostics.
    /// </summary>

    public JsonElement? RawTokenResponse { get; init; }

    /// <summary>
    /// Gets or sets the UTC time at which the refresh token expires.
    /// </summary>
    public DateTimeOffset? RefreshTokenExpiresAt { get; set; }
}
