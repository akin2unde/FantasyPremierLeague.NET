using System.Text.RegularExpressions;
using FantasyPremierLeague.Authentication;
using FantasyPremierLeague.Exceptions;
using FantasyPremierLeague.Playwright.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;

namespace FantasyPremierLeague.Playwright.Authentication;

/// <summary>
/// Provides Playwright-based authentication for Fantasy Premier League.
/// </summary>
/// <remarks>
/// The provider:
/// <list type="number">
/// <item>Opens Fantasy Premier League.</item>
/// <item>Dismisses the optional cookie consent dialog.</item>
/// <item>Opens the Premier League account login page.</item>
/// <item>Submits the manager's credentials.</item>
/// <item>Waits for either a successful token response or a login error.</item>
/// <item>Returns the authenticated FPL session.</item>
/// </list>
/// </remarks>
public sealed class PlaywrightFplLoginProvider :
    IFplLoginProvider,
    IAsyncDisposable
{
    private const string FantasyPremierLeagueUrl =
        "https://fantasy.premierleague.com/";

    private const string PremierLeagueAccountHost =
        "account.premierleague.com";

    private const string FantasyPremierLeagueHost =
        "fantasy.premierleague.com";

    private const string TokenEndpointFragment =
        "/as/token";

    private static readonly string[] CookieAcceptSelectors =
    [
        "#onetrust-accept-btn-handler",
        "button:has-text('Accept All Cookies')",
        "button:has-text('Accept all')",
        "button:has-text('I Accept')"
    ];

    private static readonly string[] LoginErrorMessages =
    [
        "Invalid username and/or password",
        "Incorrect username or password",
        "Invalid email or password",
        "Incorrect email or password"
    ];

    private readonly FplPlaywrightOptions _options;
    private readonly ILogger<PlaywrightFplLoginProvider> _logger;

    private readonly SemaphoreSlim _initializationLock =
        new(1, 1);

    private IPlaywright? _playwright;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="PlaywrightFplLoginProvider"/> class.
    /// </summary>
    /// <param name="options">
    /// The Playwright authentication options.
    /// </param>
    /// <param name="logger">
    /// The logger used to record authentication progress.
    /// </param>
    public PlaywrightFplLoginProvider(
        IOptions<FplPlaywrightOptions> options,
        ILogger<PlaywrightFplLoginProvider> logger)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _options = options.Value;
        _logger = logger;
    }

    /// <summary>
    /// Authenticates a Fantasy Premier League manager using Playwright.
    /// </summary>
    /// <param name="email">
    /// The manager's Premier League account email address.
    /// </param>
    /// <param name="password">
    /// The manager's Premier League account password.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the authentication operation.
    /// </param>
    /// <returns>
    /// The authenticated Fantasy Premier League session.
    /// </returns>
    /// <exception cref="FplAuthenticationException">
    /// Thrown when authentication fails.
    /// </exception>
    public async Task<FplSession> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        LogInformation(
            "Starting FPL authentication for {Email}.",
            email);

        var navigationTimeout =
            GetNavigationTimeout();

        var interactionTimeout =
            GetInteractionTimeout();

        var playwright =
            await GetPlaywrightAsync(
                cancellationToken);

        await using var browser =
            await playwright.Chromium.LaunchAsync(
                new BrowserTypeLaunchOptions
                {
                    Headless = _options.Headless,

                    Args =
                    [
                        "--no-sandbox",
                        "--disable-setuid-sandbox"
                    ]
                });

        await using var context =
            await browser.NewContextAsync(
                new BrowserNewContextOptions
                {
                    UserAgent = _options.UserAgent
                });

        var page =
            await context.NewPageAsync();

        page.SetDefaultNavigationTimeout(
            (float)navigationTimeout.TotalMilliseconds);

        page.SetDefaultTimeout(
            (float)interactionTimeout.TotalMilliseconds);

        using var cancellationRegistration =
            cancellationToken.Register(() =>
            {
                _ = page.CloseAsync(
                    new PageCloseOptions
                    {
                        RunBeforeUnload = false
                    });
            });

        try
        {
            /*
             * STEP 1
             * Open Fantasy Premier League.
             */

            LogInformation(
                "Navigating to the Fantasy Premier League website.");

            await page.GotoAsync(
                FantasyPremierLeagueUrl,
                new PageGotoOptions
                {
                    WaitUntil =
                        WaitUntilState.DOMContentLoaded,

                    Timeout =
                        (float)navigationTimeout.TotalMilliseconds
                });

            LogInformation(
                "Fantasy Premier League page loaded. Current URL: {Url}",
                page.Url);

            /*
             * STEP 2
             * Wait until Log in is available.
             *
             * Cookie consent is optional.
             */

            var loginLink =
                await WaitUntilLoginReadyAsync(
                    page,
                    navigationTimeout,
                    cancellationToken);

            /*
             * STEP 3
             * Open Premier League account page.
             */

            LogInformation(
                "FPL Log in control is ready. Clicking Log in.");

            await ClickLoginAndWaitForAccountPageAsync(
                page,
                loginLink,
                navigationTimeout,
                interactionTimeout,
                cancellationToken);

            /*
             * STEP 4
             * Enter credentials.
             */

            LogInformation(
                "Premier League account page is ready. Entering credentials.");

            var usernameInput =
                page.Locator(
                    "input[name='username']");

            var passwordInput =
                page.Locator(
                    "input[name='password']");

            await usernameInput.FillAsync(
                email,
                new LocatorFillOptions
                {
                    Timeout =
                        (float)interactionTimeout.TotalMilliseconds
                });

            await passwordInput.FillAsync(
                password,
                new LocatorFillOptions
                {
                    Timeout =
                        (float)interactionTimeout.TotalMilliseconds
                });

            var submit =
                page
                    .Locator(
                        "button[type='submit']")
                    .Filter(
                        new LocatorFilterOptions
                        {
                            HasTextRegex =
                                new Regex(
                                    "^\\s*Sign In\\s*$",
                                    RegexOptions.IgnoreCase)
                        });

            /*
             * STEP 5
             * Start listening for the token BEFORE
             * clicking Sign In.
             *
             * Do not require response.Ok here.
             *
             * We want to capture the token endpoint even
             * when it returns a failure status.
             */

            var tokenResponseTask =
                page.WaitForResponseAsync(
                    response =>
                        response.Url.Contains(
                            TokenEndpointFragment,
                            StringComparison.OrdinalIgnoreCase),
                    new PageWaitForResponseOptions
                    {
                        Timeout =
                            (float)navigationTimeout.TotalMilliseconds
                    });

            /*
             * Also start looking for visible login errors.
             */

            var loginErrorTask =
                WaitForLoginErrorAsync(
                    page,
                    navigationTimeout,
                    cancellationToken);

            LogInformation(
                "Credentials entered. Clicking Sign In.");

            await submit.ClickAsync(
                new LocatorClickOptions
                {
                    Timeout =
                        (float)interactionTimeout.TotalMilliseconds
                });

            LogInformation(
                "Sign In button clicked. Waiting for authentication result.");

            /*
             * STEP 6
             * Wait for whichever occurs first:
             *
             * - authentication token response
             * - visible invalid-credentials/login error
             */

            var completedTask =
                await Task.WhenAny(
                    tokenResponseTask,
                    loginErrorTask);

            /*
             * A login error appeared first.
             */
            if (completedTask == loginErrorTask)
            {
                var loginError =
                    await loginErrorTask;

                if (!string.IsNullOrWhiteSpace(
                        loginError))
                {
                    LogWarning(
                        "Premier League rejected the supplied credentials for {Email}. " +
                        "Message: {Message}",
                        email,
                        loginError);

                    throw new FplAuthenticationException("Incorrect email or password");
                }
            }

            /*
             * If no login error was detected,
             * wait for the token response.
             */

            var response =
                await tokenResponseTask;

            LogInformation(
                "Authentication endpoint responded with HTTP {Status}. URL: {Url}",
                response.Status,
                response.Url);

            var responseText =
                await response.TextAsync();

            /*
             * We intentionally check response.Ok AFTER capturing
             * the response so failures do not look like timeouts.
             */

            if (!response.Ok)
            {
                var visibleLoginError =
                    await GetLoginErrorAsync(page);

                if (!string.IsNullOrWhiteSpace(
                        visibleLoginError))
                {
                    LogWarning(
                        "Premier League authentication failed for {Email}. " +
                        "HTTP {Status}. Message: {Message}",
                        email,
                        response.Status,
                        visibleLoginError);

                    throw new FplAuthenticationException(
                        "Fantasy Premier League authentication failed because " +
                        "the Premier League account rejected the supplied credentials.");
                }

                LogWarning(
                    "Premier League token endpoint returned HTTP {Status}.",
                    response.Status);

                throw new FplAuthenticationException(
                    $"Fantasy Premier League authentication failed. " +
                    $"The token endpoint returned HTTP {response.Status}.");
            }

            /*
             * STEP 7
             * Parse the successful token response.
             */

            LogInformation(
                "Authentication token response received successfully.");

            var session =
                FplTokenResponseParser.Parse(
                    responseText,
                    DateTimeOffset.UtcNow);

            /*
             * STEP 8
             * The final redirect back to Fantasy is useful,
             * but receiving the token is the primary indication
             * that authentication succeeded.
             */

            await TryWaitForFantasyRedirectAsync(
                page,
                cancellationToken);

            LogInformation(
                "FPL authentication completed successfully for {Email}.",
                email);

            return session;
        }
        catch (OperationCanceledException)
        {
            LogWarning(
                "FPL authentication was cancelled for {Email}.",
                email);

            throw;
        }
        catch (FplAuthenticationException)
        {
            /*
             * Preserve meaningful authentication exceptions
             * such as invalid username/password.
             */

            throw;
        }
        catch (TimeoutException exception)
        {
            /*
             * Before reporting a timeout, perform one final
             * check for a visible Premier League login error.
             */

            var loginError =
                await GetLoginErrorAsync(page);

            if (!string.IsNullOrWhiteSpace(
                    loginError))
            {
                LogWarning(
                    "Premier League rejected the supplied credentials for {Email}. " +
                    "Message: {Message}",
                    email,
                    loginError);

                throw new FplAuthenticationException(
                    "Fantasy Premier League authentication failed because " +
                    "the Premier League account rejected the supplied username or password.",
                    exception);
            }

            LogError(
                exception,
                "FPL authentication timed out for {Email}. Current URL: {Url}",
                email,
                page.Url);

            throw new FplAuthenticationException(
                $"FPL login timed out while waiting for the browser authentication flow. " +
                $"Current URL: {page.Url}",
                exception);
        }
        catch (PlaywrightException exception)
        {
            LogError(
                exception,
                "FPL Playwright authentication failed for {Email}. Current URL: {Url}",
                email,
                page.Url);

            throw new FplAuthenticationException(
                "FPL login failed. The page structure, selector, token endpoint, " +
                "navigation flow, or an interstitial may have changed.",
                exception);
        }
    }

    /// <summary>
    /// Waits until the Fantasy Premier League Log in control becomes usable.
    /// </summary>
    /// <remarks>
    /// Cookie consent is optional. If the dialog appears it is dismissed.
    /// If no dialog appears and Log in is visible, this method continues
    /// immediately.
    /// </remarks>
    private async Task<ILocator> WaitUntilLoginReadyAsync(
        IPage page,
        TimeSpan overallTimeout,
        CancellationToken cancellationToken)
    {
        var loginCandidates =
            page.GetByText(
                "Log in",
                new PageGetByTextOptions
                {
                    Exact = true
                });

        var deadline =
            DateTimeOffset.UtcNow.Add(
                overallTimeout);

        while (DateTimeOffset.UtcNow < deadline)
        {
            cancellationToken
                .ThrowIfCancellationRequested();

            /*
             * Cookie dialog is optional.
             */

            var cookieButton =
                await FirstVisibleCookieButtonAsync(
                    page);

            if (cookieButton is not null)
            {
                LogInformation(
                    "Cookie consent dialog detected. Clicking accept.");

                var dismissed =
                    await TryDismissCookieDialogAsync(
                        cookieButton,
                        cancellationToken);

                if (dismissed)
                {
                    LogInformation(
                        "Cookie consent dialog removed.");
                }
                else
                {
                    LogWarning(
                        "Cookie consent dialog could not be confirmed as removed. " +
                        "The page will be checked again.");
                }

                await Task.Delay(
                    TimeSpan.FromMilliseconds(150),
                    cancellationToken);

                continue;
            }

            /*
             * No cookie dialog is blocking the page.
             */

            var visibleLogin =
                await FirstVisibleAsync(
                    loginCandidates);

            if (visibleLogin is not null)
            {
                LogInformation(
                    "No cookie dialog is blocking the page. " +
                    "FPL Log in control is visible and ready.");

                return visibleLogin;
            }

            await Task.Delay(
                TimeSpan.FromMilliseconds(250),
                cancellationToken);
        }

        throw new FplAuthenticationException(
            $"Timed out after {overallTimeout.TotalSeconds:0} seconds waiting " +
            "for the FPL Log in control.");
    }

    /// <summary>
    /// Attempts to dismiss the cookie consent dialog.
    /// </summary>
    private async Task<bool> TryDismissCookieDialogAsync(
        ILocator cookieButton,
        CancellationToken cancellationToken)
    {
        cancellationToken
            .ThrowIfCancellationRequested();

        try
        {
            /*
             * First attempt a normal click.
             */

            await cookieButton.ClickAsync(
                new LocatorClickOptions
                {
                    Timeout = 5_000
                });

            LogInformation(
                "Cookie consent accept button clicked.");

            return await WaitForCookieToDisappearAsync(
                cookieButton,
                cancellationToken);
        }
        catch (TimeoutException)
        {
            LogWarning(
                "Normal cookie consent click timed out. Trying forced click.");

            try
            {
                await cookieButton.ClickAsync(
                    new LocatorClickOptions
                    {
                        Force = true,
                        Timeout = 5_000
                    });

                LogInformation(
                    "Cookie consent accept button force-clicked.");

                return await WaitForCookieToDisappearAsync(
                    cookieButton,
                    cancellationToken);
            }
            catch (TimeoutException)
            {
                LogWarning(
                    "Forced cookie consent click also timed out.");

                return false;
            }
        }
        catch (PlaywrightException exception)
        {
            /*
             * The dialog may have been removed/re-rendered
             * between detection and clicking.
             */

            LogWarning(
                exception,
                "Cookie consent dialog changed while attempting to dismiss it.");

            return false;
        }
    }

    /// <summary>
    /// Waits for the cookie consent button to disappear.
    /// </summary>
    private async Task<bool> WaitForCookieToDisappearAsync(
        ILocator cookieButton,
        CancellationToken cancellationToken)
    {
        cancellationToken
            .ThrowIfCancellationRequested();

        try
        {
            await cookieButton.WaitForAsync(
                new LocatorWaitForOptions
                {
                    State =
                        WaitForSelectorState.Hidden,

                    Timeout = 5_000
                });

            return true;
        }
        catch (TimeoutException)
        {
            return false;
        }
        catch (PlaywrightException)
        {
            /*
             * If the element was detached from the DOM,
             * the dialog is effectively gone.
             */

            return true;
        }
    }

    /// <summary>
    /// Clicks the Fantasy Premier League Log in control and waits for
    /// the Premier League account login page.
    /// </summary>
    private async Task ClickLoginAndWaitForAccountPageAsync(
        IPage page,
        ILocator loginLink,
        TimeSpan navigationTimeout,
        TimeSpan interactionTimeout,
        CancellationToken cancellationToken)
    {
        /*
         * Start listening before clicking so we cannot
         * miss a fast redirect.
         */

        var accountUrlTask =
            WaitForAccountPageWithProgressAsync(
                page,
                navigationTimeout,
                cancellationToken);

        await loginLink.ClickAsync(
            new LocatorClickOptions
            {
                Timeout =
                    (float)interactionTimeout.TotalMilliseconds
            });

        LogInformation(
            "Log in button clicked. Waiting for Premier League account redirect.");

        await accountUrlTask;

        LogInformation(
            "Redirected to Premier League account login page: {Url}",
            page.Url);
    }

    /// <summary>
    /// Waits for the redirect to the Premier League account page while
    /// periodically logging progress.
    /// </summary>
    private async Task WaitForAccountPageWithProgressAsync(
        IPage page,
        TimeSpan overallTimeout,
        CancellationToken cancellationToken)
    {
        var deadline =
            DateTimeOffset.UtcNow.Add(
                overallTimeout);

        const int progressIntervalSeconds = 20;

        var attempt = 1;

        while (DateTimeOffset.UtcNow < deadline)
        {
            cancellationToken
                .ThrowIfCancellationRequested();

            var remaining =
                deadline -
                DateTimeOffset.UtcNow;

            var waitFor =
                remaining <
                TimeSpan.FromSeconds(
                    progressIntervalSeconds)
                    ? remaining
                    : TimeSpan.FromSeconds(
                        progressIntervalSeconds);

            if (waitFor <= TimeSpan.Zero)
            {
                break;
            }

            try
            {
                LogInformation(
                    "Waiting for Premier League account redirect. " +
                    "Attempt {Attempt}. Current URL: {Url}",
                    attempt,
                    page.Url);

                await page.WaitForURLAsync(
                    url =>
                        url.Contains(
                            PremierLeagueAccountHost,
                            StringComparison.OrdinalIgnoreCase),
                    new PageWaitForURLOptions
                    {
                        Timeout =
                            (float)waitFor.TotalMilliseconds
                    });

                return;
            }
            catch (TimeoutException)
            {
                LogWarning(
                    "Premier League account redirect has not completed yet. " +
                    "Current URL: {Url}",
                    page.Url);

                attempt++;
            }
        }

        throw new FplAuthenticationException(
            $"FPL did not redirect to the Premier League account page within " +
            $"{overallTimeout.TotalSeconds:0} seconds. Current URL: {page.Url}");
    }

    /// <summary>
    /// Waits for a visible login error from the Premier League account page.
    /// </summary>
    /// <param name="page">
    /// The Premier League account page.
    /// </param>
    /// <param name="timeout">
    /// The maximum amount of time to wait.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the operation.
    /// </param>
    /// <returns>
    /// The visible login error, or <see langword="null"/> when no known
    /// login error appears before the timeout.
    /// </returns>
    private static async Task<string?> WaitForLoginErrorAsync(
        IPage page,
        TimeSpan timeout,
        CancellationToken cancellationToken)
    {
        var deadline =
            DateTimeOffset.UtcNow.Add(
                timeout);

        while (DateTimeOffset.UtcNow < deadline)
        {
            cancellationToken
                .ThrowIfCancellationRequested();

            var error =
                await GetLoginErrorAsync(
                    page);

            if (!string.IsNullOrWhiteSpace(
                    error))
            {
                return error;
            }

            await Task.Delay(
                TimeSpan.FromMilliseconds(250),
                cancellationToken);
        }

        return null;
    }

    /// <summary>
    /// Looks for known authentication errors displayed by the
    /// Premier League account website.
    /// </summary>
    private static async Task<string?> GetLoginErrorAsync(
        IPage page)
    {
        foreach (var message in LoginErrorMessages)
        {
            var candidates =
                page.GetByText(
                    message,
                    new PageGetByTextOptions
                    {
                        Exact = false
                    });

            var visible =
                await FirstVisibleAsync(
                    candidates);

            if (visible is not null)
            {
                var text =
                    await visible.InnerTextAsync();

                return text.Trim();
            }
        }

        return null;
    }

    /// <summary>
    /// Attempts to confirm the final redirect back to Fantasy Premier League.
    /// </summary>
    /// <remarks>
    /// Once a valid authentication token has been received, failure of this
    /// secondary redirect does not invalidate the authentication session.
    /// </remarks>
    private async Task TryWaitForFantasyRedirectAsync(
        IPage page,
        CancellationToken cancellationToken)
    {
        cancellationToken
            .ThrowIfCancellationRequested();

        try
        {
            LogInformation(
                "Authentication token received. Waiting briefly for redirect " +
                "back to Fantasy Premier League.");

            await page.WaitForURLAsync(
                url =>
                    url.Contains(
                        FantasyPremierLeagueHost,
                        StringComparison.OrdinalIgnoreCase),
                new PageWaitForURLOptions
                {
                    Timeout = 30_000
                });

            LogInformation(
                "Successfully redirected back to Fantasy Premier League: {Url}",
                page.Url);
        }
        catch (TimeoutException)
        {
            LogWarning(
                "Authentication token was received successfully, but the browser " +
                "did not redirect back to Fantasy Premier League within 30 seconds. " +
                "Authentication will still be considered successful. Current URL: {Url}",
                page.Url);
        }
        catch (PlaywrightException exception)
        {
            LogWarning(
                exception,
                "Authentication token was received, but the final Fantasy Premier League " +
                "redirect could not be confirmed.");
        }
    }

    /// <summary>
    /// Returns the first visible element matched by the supplied locator.
    /// </summary>
    private static async Task<ILocator?> FirstVisibleAsync(
        ILocator locator)
    {
        var count =
            await locator.CountAsync();

        for (var i = 0; i < count; i++)
        {
            var candidate =
                locator.Nth(i);

            if (await candidate.IsVisibleAsync())
            {
                return candidate;
            }
        }

        return null;
    }

    /// <summary>
    /// Finds the first visible supported cookie-consent button.
    /// </summary>
    private static async Task<ILocator?> FirstVisibleCookieButtonAsync(
        IPage page)
    {
        foreach (var selector in CookieAcceptSelectors)
        {
            var candidate =
                page.Locator(
                    selector)
                .First;

            if (await candidate.CountAsync() > 0 &&
                await candidate.IsVisibleAsync())
            {
                return candidate;
            }
        }

        return null;
    }

    /// <summary>
    /// Returns a bounded navigation timeout.
    /// </summary>
    /// <remarks>
    /// Zero or negative values fall back to 90 seconds so browser
    /// navigation cannot wait indefinitely.
    /// </remarks>
    private TimeSpan GetNavigationTimeout()
    {
        return _options.NavigationTimeout <=
               TimeSpan.Zero
            ? TimeSpan.FromSeconds(90)
            : _options.NavigationTimeout;
    }

    /// <summary>
    /// Returns a bounded interaction timeout.
    /// </summary>
    /// <remarks>
    /// Zero or negative values fall back to 15 seconds so individual
    /// page interactions cannot wait indefinitely.
    /// </remarks>
    private TimeSpan GetInteractionTimeout()
    {
        return _options.InteractionTimeout <=
               TimeSpan.Zero
            ? TimeSpan.FromSeconds(15)
            : _options.InteractionTimeout;
    }

    /// <summary>
    /// Gets or creates the shared Playwright instance.
    /// </summary>
    private async Task<IPlaywright> GetPlaywrightAsync(
        CancellationToken cancellationToken)
    {
        if (_playwright is not null)
        {
            return _playwright;
        }

        await _initializationLock.WaitAsync(
            cancellationToken);

        try
        {
            _playwright ??=
                await Microsoft.Playwright.Playwright
                    .CreateAsync();

            return _playwright;
        }
        finally
        {
            _initializationLock.Release();
        }
    }

    /// <summary>
    /// Logs an informational message when SDK logging is enabled.
    /// </summary>
    private void LogInformation(
        string message,
        params object?[] args)
    {
        if (_options.ShowLog)
        {
            _logger.LogInformation(
                message,
                args);
        }
    }

    /// <summary>
    /// Logs a warning when SDK logging is enabled.
    /// </summary>
    private void LogWarning(
        string message,
        params object?[] args)
    {
        if (_options.ShowLog)
        {
            _logger.LogWarning(
                message,
                args);
        }
    }

    /// <summary>
    /// Logs a warning with an exception when SDK logging is enabled.
    /// </summary>
    private void LogWarning(
        Exception exception,
        string message,
        params object?[] args)
    {
        if (_options.ShowLog)
        {
            _logger.LogWarning(
                exception,
                message,
                args);
        }
    }

    /// <summary>
    /// Logs an error with an exception when SDK logging is enabled.
    /// </summary>
    private void LogError(
        Exception exception,
        string message,
        params object?[] args)
    {
        if (_options.ShowLog)
        {
            _logger.LogError(
                exception,
                message,
                args);
        }
    }

    /// <summary>
    /// Releases Playwright resources owned by this provider.
    /// </summary>
    public ValueTask DisposeAsync()
    {
        _playwright?.Dispose();

        _playwright = null;

        _initializationLock.Dispose();

        return ValueTask.CompletedTask;
    }
}