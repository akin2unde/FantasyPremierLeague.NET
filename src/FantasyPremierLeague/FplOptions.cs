namespace FantasyPremierLeague;
/// <summary>
/// Configures FPL API access, session reuse, token refresh, and error reporting.
/// </summary>

public sealed class FplOptions
{
    /// <summary>
    /// Gets or sets the base address of the FPL API.
    /// </summary>
    public Uri BaseAddress { get; set; } = new("https://fantasy.premierleague.com/api/");
    /// <summary>
    /// Gets or sets the User-Agent sent with FPL API requests.
    /// </summary>
    public string UserAgent { get; set; } = "FantasyPremierLeague.NET/0.2 (+https://github.com/akin2unde/FantasyPremierLeague.NET)";
    /// <summary>
    /// Gets or sets the timeout applied to FPL API requests.
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);
    /// <summary>
    /// Gets or sets how early an access token is treated as expired and refreshed.
    /// </summary>
    public TimeSpan RefreshBeforeExpiry { get; set; } = TimeSpan.FromMinutes(2);
    /// <summary>
    /// Gets or sets whether a still-usable persisted access token can be reused.
    /// </summary>
    public bool ReuseStoredToken { get; set; } = true;

    /// <summary>
    /// Describes the Load Profile and Entry after Login.
    /// </summary>
    public bool LoadProfileAfterLogin { get; set; } = false;

    /// <summary>
    /// Gets or sets whether upstream FPL response bodies and underlying exception
    /// messages are included in exceptions returned to the consuming application.
    /// </summary>
    /// <remarks>
    /// The default is <see langword="false"/> because upstream responses can
    /// contain diagnostic or authentication information. Enable this only when
    /// the consuming application intentionally handles and protects those details.
    /// </remarks>
    public bool ExposeDetailedErrors { get; set; }

    /// <summary>
    /// Gets or sets the Premier League OAuth token endpoint used to refresh sessions.
    /// </summary>
    public Uri TokenEndpoint { get; set; } =
        new("https://account.premierleague.com/as/token");

    /// <summary>
    /// Gets or sets the public Premier League OAuth client identifier.
    /// </summary>
    public string AuthenticationClientId { get; set; } =
        "bfcbaf69-aade-4c1b-8f00-c1cb8a193030";

    /// <summary>
    /// Gets or sets the scopes requested when an access token is refreshed.
    /// </summary>
    public string AuthenticationScope { get; set; } = "openid profile email";
}
