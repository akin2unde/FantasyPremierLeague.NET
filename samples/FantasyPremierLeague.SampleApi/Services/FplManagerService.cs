using FantasyPremierLeague.Managers;
using FantasyPremierLeague.Models.Bootstrap;

namespace FantasyPremierLeague.SampleApi.Services;

/// <summary>
/// Demonstrates how an application service can wrap the SDK.
/// </summary>
public sealed class FplManagerService : IFplManagerService
{
    private readonly FplClient _fplClient;
    private readonly IFplManagerStore _managerStore;

    /// <summary>
    /// Initializes a new instance of the <see cref="FplManagerService"/> class.
    /// </summary>
    public FplManagerService(FplClient fplClient, IFplManagerStore managerStore)
    {
        _fplClient = fplClient;
        _managerStore = managerStore;
    }

    /// <inheritdoc />
    public async Task<FplManagerRecord?> LoginAsync(
        string email,
        string password,
        bool includeDetails,
        CancellationToken cancellationToken = default)
    {
        var res = await _fplClient.LoginAsync(email, password, includeDetails, cancellationToken);
        return res;
    }


    /// <inheritdoc />
    public Task<FplManagerRecord?> GetByEntryIdAsync(
        int entryId,
        CancellationToken cancellationToken = default) =>
        _managerStore.GetByEntryIdAsync(entryId, cancellationToken);

    /// <inheritdoc />
    public Task<FplBootstrapStatic> GetBoostrapAsync(
        CancellationToken cancellationToken = default) =>
        _fplClient.Boostrap.GetDataAsync(cancellationToken);


}
