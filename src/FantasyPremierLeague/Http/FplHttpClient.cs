using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FantasyPremierLeague.Authentication;
using FantasyPremierLeague.Exceptions;

namespace FantasyPremierLeague.Http;
/// <summary>
/// Provides Fantasy Premier League http request client abstraction.
/// </summary>
public sealed class FplHttpClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly HttpClient _httpClient;
    private readonly IFplAuthenticationManager _authenticationManager;

    /// <summary>
    /// Describes the FplHttpClient member.
    /// </summary>
    public FplHttpClient(HttpClient httpClient, IFplAuthenticationManager authenticationManager)
    {
        _httpClient = httpClient;
        _authenticationManager = authenticationManager;
    }

    /// <summary>
    /// Describes the member member.
    /// </summary>
    public Task<T> GetPublicAsync<T>(string path, CancellationToken cancellationToken) =>
        SendAsync<T>(HttpMethod.Get, path, null, false, cancellationToken);

    /// <summary>
    /// Describes the member member.
    /// </summary>
    public Task<T> GetAuthenticatedAsync<T>(string path, CancellationToken cancellationToken) =>
        SendAsync<T>(HttpMethod.Get, path, null, true, cancellationToken);

    /// <summary>
    /// Describes the member member.
    /// </summary>
    public Task<T> PostAuthenticatedAsync<TBody, T>(string path, TBody body, CancellationToken cancellationToken) =>
        SendAsync<T>(HttpMethod.Post, path, body, true, cancellationToken);

    private async Task<T> SendAsync<T>(
        HttpMethod method,
        string path,
        object? body,
        bool authenticated,
        CancellationToken cancellationToken)
    {
        using var response = await SendOnceAsync(method, path, body, authenticated, cancellationToken);
        if (authenticated && response.StatusCode == HttpStatusCode.Unauthorized)
        {
            var refreshedToken = await _authenticationManager.RefreshCurrentAsync(cancellationToken);
            using var retryResponse = await SendOnceAsync(
                method, path, body, authenticated, cancellationToken, refreshedToken);
            return await ReadResponseAsync<T>(retryResponse, path, cancellationToken);
        }
        return await ReadResponseAsync<T>(response, path, cancellationToken);
    }

    private async Task<HttpResponseMessage> SendOnceAsync(
        HttpMethod method,
        string path,
        object? body,
        bool authenticated,
        CancellationToken cancellationToken,
        string? accessToken = null)
    {
        using var request = new HttpRequestMessage(method, path);
        if (body is not null)
            request.Content = JsonContent.Create(body, options: JsonOptions);

        if (authenticated)
        {
            accessToken ??= await _authenticationManager.GetCurrentAccessTokenAsync(cancellationToken);
            request.Headers.TryAddWithoutValidation(FplHeaders.Authorization, $"Bearer {accessToken}");
        }

        return await _httpClient.SendAsync(request, cancellationToken);
    }

    private static async Task<T> ReadResponseAsync<T>(
        HttpResponseMessage response,
        string path,
        CancellationToken cancellationToken)
    {
        if (!response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new FplException(
                $"FPL returned {(int)response.StatusCode} ({response.ReasonPhrase}) for '{path}'. Body: {responseBody}");
        }
        try
        {
            return await response.Content.ReadFromJsonAsync<T>(JsonOptions, cancellationToken) ?? throw new FplException($"FPL returned an empty response for '{path}'.");

        }
        catch (Exception ex)
        {
            Console.Write($"{ex.Message}");
            throw;

        }
    }
}
