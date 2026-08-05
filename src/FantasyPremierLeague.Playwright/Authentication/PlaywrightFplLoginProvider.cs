using System.Text.RegularExpressions;
using FantasyPremierLeague.Authentication;
using FantasyPremierLeague.Exceptions;
using FantasyPremierLeague.Playwright.Internal;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;

namespace FantasyPremierLeague.Playwright.Authentication;
/// <summary>
/// Provides the PlaywrightFplLoginProvider member.
/// </summary>

public sealed class PlaywrightFplLoginProvider : IFplLoginProvider, IAsyncDisposable
{
    private static readonly string[] CookieAcceptSelectors =
    [
        "#onetrust-accept-btn-handler",
        "button:has-text('Accept All Cookies')",
        "button:has-text('Accept all')",
        "button:has-text('I Accept')"
    ];

    private readonly FplPlaywrightOptions _options;
    private readonly SemaphoreSlim _initializationLock = new(1, 1);
    private IPlaywright? _playwright;

    /// <summary>
    /// Describes the PlaywrightFplLoginProvider member.
    /// </summary>
    public PlaywrightFplLoginProvider(IOptions<FplPlaywrightOptions> options) =>
        _options = options.Value;
    /// <summary>
    /// Provides the LoginAsync member.
    /// </summary>

    public async Task<FplSession> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        var playwright = await GetPlaywrightAsync(cancellationToken);
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = _options.Headless,
            Args = ["--no-sandbox", "--disable-setuid-sandbox"]
        });
        await using var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            UserAgent = _options.UserAgent
        });
        var page = await context.NewPageAsync();
        page.SetDefaultNavigationTimeout((float)_options.NavigationTimeout.TotalMilliseconds);
        page.SetDefaultTimeout((float)_options.InteractionTimeout.TotalMilliseconds);

        using var cancellationRegistration = cancellationToken.Register(() =>
        {
            _ = page.CloseAsync(new PageCloseOptions { RunBeforeUnload = false });
        });

        try
        {
            await page.GotoAsync("https://fantasy.premierleague.com/", new PageGotoOptions
            {
                WaitUntil = WaitUntilState.DOMContentLoaded
            });

            // Polls until either the cookie dialog or the "Log in" control is actually
            // visible — no blind one-shot check and no arbitrary Task.Delay. FPL renders
            // both the cookie banner and the nav client-side, so DOMContentLoaded can fire
            // before either exists in the DOM.
            var loginLink = await WaitUntilLoginReadyAsync(page, _options.NavigationTimeout);

            // Start the URL wait before clicking (same pattern as the token-response wait
            // below) so the navigation can't complete before we're listening for it.
            var accountUrlTask = page.WaitForURLAsync(url =>
                url.Contains("account.premierleague.com", StringComparison.OrdinalIgnoreCase));
            await loginLink.ClickAsync();
            await accountUrlTask;

            await page.Locator("input[name='username']").FillAsync(email);
            await page.Locator("input[name='password']").FillAsync(password);

            var submit = page.Locator("button[type='submit']").Filter(new LocatorFilterOptions
            {
                HasTextRegex = new Regex("^\\s*Sign In\\s*$", RegexOptions.IgnoreCase)
            });

            var tokenResponseTask = page.WaitForResponseAsync(response =>
                response.Url.Contains("account.premierleague.com/as/token", StringComparison.OrdinalIgnoreCase) &&
                response.Ok);

            await submit.ClickAsync();
            await page.WaitForURLAsync(url =>
                url.Contains("fantasy.premierleague.com/en/", StringComparison.OrdinalIgnoreCase));

            var response = await tokenResponseTask;
            return FplTokenResponseParser.Parse(await response.TextAsync(), DateTimeOffset.UtcNow);
        }
        catch (OperationCanceledException) { throw; }
        catch (PlaywrightException exception)
        {
            throw new FplAuthenticationException(
                "FPL login failed. The page structure, selector, token endpoint, or an interstitial may have changed.",
                exception);
        }
    }

    /// <summary>
    /// Repeatedly checks for the cookie dialog and the "Log in" control, clicking through
    /// the dialog whenever it appears, until "Log in" is genuinely visible and clickable.
    /// Bounded by <paramref name="overallTimeout"/> so a broken/changed page fails fast
    /// with a clear error instead of hanging.
    /// </summary>
    private static async Task<ILocator> WaitUntilLoginReadyAsync(IPage page, TimeSpan overallTimeout)
    {
        var loginCandidates = page.GetByText("Log in", new() { Exact = true });
        var deadline = DateTime.UtcNow + overallTimeout;

        while (true)
        {
            // Check the cookie dialog FIRST, every iteration. IsVisibleAsync only reports
            // CSS visibility (non-zero size, not display:none/visibility:hidden) — it says
            // nothing about whether a modal overlay sits on top. FPL's cookie dialog often
            // renders over a "Log in" link that is already technically visible underneath,
            // so checking login-visibility first would return "ready" while it's still
            // covered, and the later ClickAsync() would just hang waiting for the overlay
            // to clear (which it never does, since we'd have skipped clicking it).
            await Task.Delay(200);
            var cookieButton = await FirstVisibleCookieButtonAsync(page);
            if (cookieButton is not null)
            {
                await cookieButton.ClickAsync();
                continue; // dialog dismissed — re-check immediately rather than burning a poll cycle
            }

            // Only trust "Log in" visibility once we've confirmed no cookie dialog is
            // currently covering the page.
            await Task.Delay(200);
            var visibleLogin = await FirstVisibleAsync(loginCandidates);
            if (visibleLogin is not null)
            {
                return visibleLogin;
            }

            if (overallTimeout != TimeSpan.Zero && DateTime.UtcNow >= deadline)
            {
                throw new FplAuthenticationException(
                    "Timed out waiting for the FPL 'Log in' control to render. The page may not have " +
                    "finished loading, or the cookie dialog/login markup may have changed.");
            }

            await Task.Delay(250); // page is still client-rendering — brief pause before the next poll
        }
    }

    /// <summary>
    /// Returns the first element matched by <paramref name="locator"/> that is actually
    /// visible, or null if none currently are. Sites often render more than one match for
    /// the same text (e.g. desktop nav + mobile nav), and only one is on-screen at a time.
    /// </summary>
    private static async Task<ILocator?> FirstVisibleAsync(ILocator locator)
    {
        var count = await locator.CountAsync();
        for (var i = 0; i < count; i++)
        {
            var candidate = locator.Nth(i);
            if (await candidate.IsVisibleAsync())
            {
                return candidate;
            }
        }
        return null;
    }

    private static async Task<ILocator?> FirstVisibleCookieButtonAsync(IPage page)
    {
        foreach (var selector in CookieAcceptSelectors)
        {
            var candidate = page.Locator(selector).First;
            if (await candidate.CountAsync() > 0 && await candidate.IsVisibleAsync())
            {
                return candidate;
            }
        }
        return null;
    }

    private async Task<IPlaywright> GetPlaywrightAsync(CancellationToken cancellationToken)
    {
        if (_playwright is not null) return _playwright;

        await _initializationLock.WaitAsync(cancellationToken);
        try
        {
            _playwright ??= await Microsoft.Playwright.Playwright.CreateAsync();
            return _playwright;
        }
        finally
        {
            _initializationLock.Release();
        }
    }
    /// <summary>
    /// Provides the DisposeAsync member.
    /// </summary>

    public ValueTask DisposeAsync()
    {
        _playwright?.Dispose();
        _playwright = null;
        _initializationLock.Dispose();
        return ValueTask.CompletedTask;
    }
}
