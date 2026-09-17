using FantasyPremierLeague.Models.Bootstrap;

namespace FantasyPremierLeague.Clients;

/// <summary>
/// Compatibility wrapper for the original misspelled bootstrap client name.
/// </summary>
public sealed class FplBoostrapClient
{
    private readonly FplBootstrapClient _inner;

    internal FplBootstrapClient Inner => _inner;

    /// <summary>Initializes the compatibility wrapper.</summary>
    public FplBoostrapClient(FplBootstrapClient inner)
    {
        _inner = inner ?? throw new ArgumentNullException(nameof(inner));
    }

    /// <summary>Gets FPL bootstrap-static data.</summary>
    public Task<FplBootstrapStatic> GetDataAsync(
        CancellationToken cancellationToken = default) =>
        _inner.GetDataAsync(cancellationToken);
}
