using System.Collections.Concurrent;

namespace FantasyPremierLeague.Managers;

/// <summary>
/// Stores FPL manager records in application memory.
/// This implementation is intended for development and testing.
/// </summary>
public sealed class InMemoryFplManagerStore : IFplManagerStore
{
    private readonly ConcurrentDictionary<string, FplManagerRecord>
        _managersByEmail = new(StringComparer.OrdinalIgnoreCase);

    private readonly ConcurrentDictionary<int, FplManagerRecord>
        _managersByEntryId = new();

    /// <inheritdoc />
    public Task<FplManagerRecord?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        cancellationToken.ThrowIfCancellationRequested();

        _managersByEmail.TryGetValue(
            NormalizeEmail(email),
            out var manager);

        return Task.FromResult(manager);
    }

    /// <inheritdoc />
    public Task<FplManagerRecord?> GetByEntryIdAsync(
        int entryId,
        CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(entryId);
        cancellationToken.ThrowIfCancellationRequested();

        _managersByEntryId.TryGetValue(entryId, out var manager);

        return Task.FromResult(manager);
    }

    /// <inheritdoc />
    public Task SaveAsync(
        FplManagerRecord manager,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(manager);
        ArgumentException.ThrowIfNullOrWhiteSpace(manager.Email);
        cancellationToken.ThrowIfCancellationRequested();

        var normalizedEmail = NormalizeEmail(manager.Email);

        _managersByEmail[normalizedEmail] = manager;

        if (manager.EntryId > 0)
        {
            _managersByEntryId[manager.EntryId] = manager;
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task RemoveAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        cancellationToken.ThrowIfCancellationRequested();

        if (_managersByEmail.TryRemove(
                NormalizeEmail(email),
                out var manager) &&
            manager.EntryId > 0)
        {
            _managersByEntryId.TryRemove(
                manager.EntryId,
                out _);
        }

        return Task.CompletedTask;
    }

    private static string NormalizeEmail(string email)
    {
        return email.Trim().ToLowerInvariant();
    }
}