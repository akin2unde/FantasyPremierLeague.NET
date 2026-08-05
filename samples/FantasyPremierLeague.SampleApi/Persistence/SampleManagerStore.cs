using FantasyPremierLeague.Managers;

namespace FantasyPremierLeague.SampleApi.Persistence;

/// <summary>
/// Demonstrates the manager persistence contract using in-memory storage.
/// Replace this class with a MongoDB, SQL, Cassandra, Redis, or other implementation.
/// </summary>
public sealed class SampleManagerStore : IFplManagerStore
{
    private readonly InMemoryFplManagerStore _inner = new();

    /// <inheritdoc />
    public Task<FplManagerRecord?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default) =>
        _inner.GetByEmailAsync(email, cancellationToken);

    /// <inheritdoc />
    public Task<FplManagerRecord?> GetByEntryIdAsync(
        int entryId,
        CancellationToken cancellationToken = default) =>
        _inner.GetByEntryIdAsync(entryId, cancellationToken);

    /// <inheritdoc />
    public Task SaveAsync(
        FplManagerRecord manager,
        CancellationToken cancellationToken = default) =>
        _inner.SaveAsync(manager, cancellationToken);

    /// <inheritdoc />
    public Task RemoveAsync(
        string email,
        CancellationToken cancellationToken = default) =>
        _inner.RemoveAsync(email, cancellationToken);
}
