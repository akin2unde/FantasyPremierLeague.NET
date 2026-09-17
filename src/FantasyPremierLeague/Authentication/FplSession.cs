using System.Text.Json;
namespace FantasyPremierLeague.Authentication;
/// <summary>
/// Provides the FplSession member.
/// </summary>
public sealed class FplSession
{
    /// <summary>
    /// Describes the AccessToken member.
    /// </summary>
    public required string AccessToken { get; init; }
    /// <summary>
    /// Describes the ExpiresAt member.
    /// </summary>
    public DateTimeOffset? ExpiresAt { get; init; }
    /// <summary>
    /// Provides the member member.
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Describes the RawTokenResponse member.
    /// </summary>

    public JsonElement? RawTokenResponse { get; init; }

    /// <summary>
    /// Describes the RefreshTokenExpiresAt member.
    /// </summary>
    public DateTimeOffset? RefreshTokenExpiresAt { get; set; }
}
