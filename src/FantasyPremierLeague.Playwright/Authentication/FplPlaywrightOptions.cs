namespace FantasyPremierLeague.Playwright.Authentication;
/// <summary>
/// Configures Playwright-based FPL authentication.
/// </summary>

public sealed class FplPlaywrightOptions
{
    /// <summary>
    /// Gets or sets whether authentication progress is written through the configured logger.
    /// </summary>
    public bool ShowLog { get; set; } = false;
    /// <summary>
    /// Gets or sets whether Chromium runs without a visible window.
    /// </summary>
    public bool Headless { get; set; } = true;
    /// <summary>
    /// Gets or sets the maximum duration for page navigation and token capture.
    /// </summary>
    public TimeSpan NavigationTimeout { get; set; } = TimeSpan.FromSeconds(45);
    /// <summary>
    /// Gets or sets the maximum duration for individual page interactions.
    /// </summary>
    public TimeSpan InteractionTimeout { get; set; } = TimeSpan.FromSeconds(30);
    /// <summary>
    /// Gets or sets the browser User-Agent used during authentication.
    /// </summary>
    public string UserAgent { get; set; } =
        "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/150.0.0.0 Safari/537.36";
}
