using FantasyPremierLeague.Managers;
namespace FantasyPremierLeague.Tests;
public sealed class ManagerStoreTests
{
    [Fact] public async Task SavesAndFindsManagerByEmailAndEntryId()
    { var store=new InMemoryFplManagerStore(); var manager=new FplManagerRecord{Email="TEST@EXAMPLE.COM",EntryId=42,AccessToken="token",TokenExpiresAt=DateTimeOffset.UtcNow.AddHours(1)}; await store.SaveAsync(manager); Assert.NotNull(await store.GetByEmailAsync("test@example.com")); Assert.NotNull(await store.GetByEntryIdAsync(42)); }
}
