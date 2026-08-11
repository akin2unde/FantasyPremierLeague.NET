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
/// The provider opens the Fantasy Premier League website, handles optional
/// cookie-consent dialogs, redirects to the Premier League account website,
/// submits the supplied credentials, captures the authentication token,
/// and returns an authenticated FPL session.
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
    /// Authenticates a Fantasy Premier League manager.
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
    /// The authenticated FPL session.
    /// </returns>
    /// <exception cref="FplAuthenticationException">
    /// Thrown when authentication cannot be completed.
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
             * Wait until Log in is usable.
             *
             * If the FPL cookie dialog appears, dismiss it.
             * If it does not appear, continue normally.
             */

            var loginLink =
                await WaitUntilLoginReadyAsync(
                    page,
                    navigationTimeout,
                    cancellationToken);

            /*
             * STEP 3
             * Redirect to Premier League account login.
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
             * The Premier League account page may show its own
             * OneTrust cookie dialog.
             */

            LogInformation(
                "Premier League account page reached. Checking for cookie consent.");

            await DismissCookieIfPresentAsync(
                page,
                cancellationToken);

            /*
             * STEP 5
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
             * STEP 6
             * Start listening for the token BEFORE clicking Sign In.
             *
             * Do not require response.Ok here because we want to
             * capture unsuccessful token responses too.
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
             * At the same time, monitor the page for an explicit
             * authentication error such as invalid credentials.
             */

            var loginErrorTask =
                WaitForLoginErrorAsync(
                    page,
                    navigationTimeout,
                    cancellationToken);

            LogInformation(
                "Credentials entered. Clicking Sign In.");

            /*
             * OneTrust sometimes appears AFTER the account page
             * has already rendered, so check again immediately
             * before clicking Sign In.
             */

            await ClickSignInAsync(
                page,
                submit,
                interactionTimeout,
                cancellationToken);

            LogInformation(
                "Sign In button clicked. Waiting for authentication result.");

            /*
             * STEP 7
             * Wait for either:
             *
             * - token response
             * - visible login error
             */

            var completedTask =
                await Task.WhenAny(
                    tokenResponseTask,
                    loginErrorTask);

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

                    throw new FplAuthenticationException(
                        "Fantasy Premier League authentication failed because " +
                        "the Premier League account rejected the supplied username or password.");
                }
            }

            /*
             * No visible login error was found first.
             * Await the token endpoint response.
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
             * Evaluate HTTP success after capturing the response.
             */

            if (!response.Ok)
            {
                var visibleLoginError =
                    await GetLoginErrorAsync(
                        page);

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

                throw new FplAuthenticationException(
                    $"Fantasy Premier League authentication failed. " +
                    $"The token endpoint returned HTTP {response.Status}.");
            }

            /*
             * STEP 8
             * Parse the token response.
             */

            LogInformation(
                "Authentication token response received successfully.");

            var session =
                FplTokenResponseParser.Parse(
                    responseText,
                    DateTimeOffset.UtcNow);

            /*
             * STEP 9
             * The final redirect is secondary.
             *
             * Once the token is valid, a slow final browser redirect
             * should not invalidate authentication.
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
            throw;
        }
        catch (TimeoutException exception)
        {
            /*
             * Before reporting a generic timeout, check one
             * final time for an explicit login error.
             */

            var loginError =
                await GetLoginErrorAsync(
                    page);

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
    /// <param name="page">
    /// The current Playwright page.
    /// </param>
    /// <param name="overallTimeout">
    /// The maximum amount of time to wait.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the operation.
    /// </param>
    /// <returns>
    /// The visible FPL Log in control.
    /// </returns>
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
    /// Dismisses a visible cookie dialog if one is currently present.
    /// </summary>
    private async Task DismissCookieIfPresentAsync(
        IPage page,
        CancellationToken cancellationToken)
    {
        cancellationToken
            .ThrowIfCancellationRequested();

        var cookieButton =
            await FirstVisibleCookieButtonAsync(
                page);

        if (cookieButton is null)
        {
            LogInformation(
                "No cookie consent dialog detected on {Url}.",
                page.Url);

            return;
        }

        LogInformation(
            "Cookie consent dialog detected on {Url}. Clicking accept.",
            page.Url);

        var dismissed =
            await TryDismissCookieDialogAsync(
                cookieButton,
                cancellationToken);

        if (dismissed)
        {
            LogInformation(
                "Cookie consent dialog removed from {Url}.",
                page.Url);
        }
        else
        {
            LogWarning(
                "Cookie consent dialog could not be confirmed as removed from {Url}.",
                page.Url);
        }
    }

    /// <summary>
    /// Attempts to dismiss the supplied cookie-consent control.
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
             * Try a normal Playwright click first.
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
            /*
             * If normal Playwright actionability checks fail,
             * try forcing the cookie button only.
             */

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
             * OneTrust may re-render between detection and clicking.
             */

            LogWarning(
                exception,
                "Cookie consent dialog changed while attempting to dismiss it.");

            return false;
        }
    }

    /// <summary>
    /// Waits briefly for the cookie consent control to disappear.
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
             * If the element disappeared from the DOM completely,
             * consider the dialog successfully removed.
             */

            return true;
        }
    }

    /// <summary>
    /// Clicks the FPL Log in control and waits for the Premier League
    /// account page.
    /// </summary>
    private async Task ClickLoginAndWaitForAccountPageAsync(
        IPage page,
        ILocator loginLink,
        TimeSpan navigationTimeout,
        TimeSpan interactionTimeout,
        CancellationToken cancellationToken)
    {
        /*
         * Start waiting before clicking so a fast redirect
         * cannot occur before the waiter starts.
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
    /// Waits for the Premier League account redirect while periodically
    /// reporting progress.
    /// </summary>
    private async Task WaitForAccountPageWithProgressAsync(
        IPage page,
        TimeSpan overallTimeout,
        CancellationToken cancellationToken)
    {
        var deadline =
            DateTimeOffset.UtcNow.Add(
                overallTimeout);

        const int intervalSeconds = 20;

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
                TimeSpan.FromSeconds(intervalSeconds)
                    ? remaining
                    : TimeSpan.FromSeconds(intervalSeconds);

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
    /// Clicks Sign In while handling a cookie dialog that may appear
    /// asynchronously after the Premier League account page has loaded.
    /// </summary>
    private async Task ClickSignInAsync(
        IPage page,
        ILocator submit,
        TimeSpan interactionTimeout,
        CancellationToken cancellationToken)
    {
        cancellationToken
            .ThrowIfCancellationRequested();

        /*
         * One final check immediately before clicking.
         */

        await DismissCookieIfPresentAsync(
            page,
            cancellationToken);

        try
        {
            await submit.ClickAsync(
                new LocatorClickOptions
                {
                    Timeout =
                        (float)interactionTimeout.TotalMilliseconds
                });

            return;
        }
        catch (TimeoutException)
        {
            /*
             * The stack trace you encountered shows exactly this case:
             * OneTrust can render after our previous check and intercept
             * pointer events while Playwright is trying to click Sign In.
             */

            var cookieButton =
                await FirstVisibleCookieButtonAsync(
                    page);

            if (cookieButton is null)
            {
                throw;
            }

            LogWarning(
                "Sign In click was blocked by a cookie consent dialog. " +
                "Dismissing the dialog and retrying.");

            await DismissCookieIfPresentAsync(
                page,
                cancellationToken);

            await submit.ClickAsync(
                new LocatorClickOptions
                {
                    Timeout =
                        (float)interactionTimeout.TotalMilliseconds
                });

            LogInformation(
                "Sign In click succeeded after removing the cookie dialog.");
        }
    }

    /// <summary>
    /// Waits for a visible login error from the Premier League account page.
    /// </summary>
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
    /// Returns a known authentication error displayed by the Premier League
    /// account website.
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
    /// Once a valid token has been received, a slow final redirect does not
    /// invalidate the authenticated session.
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
                "Authentication token was received, but the final FPL redirect " +
                "could not be confirmed.");
        }
    }

    /// <summary>
    /// Returns the first visible element matched by a locator.
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
    /// Returns the first visible supported cookie consent button.
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
    /// Gets a bounded browser navigation timeout.
    /// </summary>
    private TimeSpan GetNavigationTimeout()
    {
        return _options.NavigationTimeout <= TimeSpan.Zero
            ? TimeSpan.FromSeconds(90)
            : _options.NavigationTimeout;
    }

    /// <summary>
    /// Gets a bounded browser interaction timeout.
    /// </summary>
    private TimeSpan GetInteractionTimeout()
    {
        return _options.InteractionTimeout <= TimeSpan.Zero
            ? TimeSpan.FromSeconds(15)
            : _options.InteractionTimeout;
    }

    /// <summary>
    /// Gets or creates the Playwright instance.
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
    /// Logs an informational message when logging is enabled.
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
    /// Logs a warning when logging is enabled.
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
    /// Logs a warning containing an exception when logging is enabled.
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
    /// Logs an error containing an exception when logging is enabled.
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