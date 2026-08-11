using FantasyPremierLeague.Models.Managers;

namespace FantasyPremierLeague.Managers;

/// <summary>
/// Represents fpl manager record.
/// </summary>
public sealed class FplManagerRecord
{
    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    public string? Password { get; set; }
    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// Gets or sets the entry id.
    /// </summary>
    public int EntryId { get; set; }

    /// <summary>
    /// Gets or sets the access token.
    /// </summary>
    public required string AccessToken { get; set; }

    /// <summary>
    /// Gets or sets the token expires at.
    /// </summary>
    public DateTimeOffset? TokenExpiresAt { get; set; }

    /// <summary>
    /// Gets or sets the refresh token.
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Gets or sets the profile.
    /// </summary>
    public FplMePlayer? Profile { get; set; }

    /// <summary>
    /// Gets or sets the entry.
    /// </summary>
    public FplEntry? Entry { get; set; }

    /// <summary>
    /// Gets or sets the updated at.
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Determines whether the stored access token can be reused.
    /// </summary>
    /// <param name="refreshBeforeExpiry">The period before expiry during which the token should be refreshed.</param>
    /// <returns><see langword="true"/> when the token is present and remains usable; otherwise, <see langword="false"/>.</returns>
    public bool HasUsableToken(TimeSpan refreshBeforeExpiry) =>
        !string.IsNullOrWhiteSpace(AccessToken) &&
        (TokenExpiresAt is null || TokenExpiresAt > DateTimeOffset.UtcNow.Add(refreshBeforeExpiry));
}
