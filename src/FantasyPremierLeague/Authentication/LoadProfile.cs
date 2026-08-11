using System.Net.Http.Json;
using FantasyPremierLeague.Exceptions;
using FantasyPremierLeague.Http;
using FantasyPremierLeague.Managers;
using FantasyPremierLeague.Models.Managers;
namespace FantasyPremierLeague.Authentication;

internal sealed class LoadProfile : ILoadProfile
{
    private readonly IHttpClientFactory _httpClientFactory;
    public LoadProfile(
      IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    /// <summary>
    /// Provides the SetProfileAsync member.
    /// </summary>
    public async Task SetProfileAsync(FplManagerRecord fplManagerRecord, CancellationToken cancellationToken = default)
    {
        var client = _httpClientFactory
            .CreateClient("FantasyPremierLeague");

        using var profileRequest = new HttpRequestMessage(
            HttpMethod.Get,
            $"me/");

        profileRequest.Headers.Remove(FplHeaders.Authorization);

        profileRequest.Headers.TryAddWithoutValidation(
            FplHeaders.Authorization,
            $"Bearer {fplManagerRecord.AccessToken}");

        using var profileResponse = await client.SendAsync(
            profileRequest,
            cancellationToken);

        profileResponse.EnsureSuccessStatusCode();
        var profile =
            await profileResponse.Content
                .ReadFromJsonAsync<FplMe>(
                    cancellationToken: cancellationToken);

        fplManagerRecord.Profile = profile?.Player;
        if (fplManagerRecord.Profile is not null)
        {
            fplManagerRecord.EntryId = fplManagerRecord.Profile.EntryId;
            var entry =
          await client.GetFromJsonAsync<FplEntry>(
              $"entry/{fplManagerRecord.EntryId}/",
              cancellationToken);
            fplManagerRecord.Entry = entry;
        }
    }


}

