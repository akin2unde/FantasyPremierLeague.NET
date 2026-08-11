namespace FantasyPremierLeague.Playwright.Authentication;
/// <summary>
/// Provides the FplPlaywrightOptions member.
/// </summary>

public sealed class FplPlaywrightOptions
{
    /// <summary>
    /// Describes the ShowLog member.
    /// </summary>
    public bool ShowLog { get; set; } = false;
    /// <summary>
    /// Describes the Headless member.
    /// </summary>
    public bool Headless { get; set; } = true;
    /// <summary>
    /// Provides the FromSeconds member.
    /// </summary>
    public TimeSpan NavigationTimeout { get; set; } = TimeSpan.FromSeconds(45);
    /// <summary>
    /// Describes the InteractionTimeout member.
    /// </summary>
    public TimeSpan InteractionTimeout { get; set; } = TimeSpan.FromSeconds(30);
    /// <summary>
    /// Describes the UserAgent member.
    /// </summary>
    public string UserAgent { get; set; } =
        "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/150.0.0.0 Safari/537.36";
}
