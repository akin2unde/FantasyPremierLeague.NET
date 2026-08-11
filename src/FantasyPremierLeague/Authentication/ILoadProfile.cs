using FantasyPremierLeague.Managers;
using FantasyPremierLeague.Models.Managers;

namespace FantasyPremierLeague.Authentication;
/// <summary>
/// Provides the SetProfile member.
/// </summary>
public interface ILoadProfile
{
    /// <summary>
    /// Describes the SetProfileAsync member.
    /// </summary>
    Task SetProfileAsync(FplManagerRecord fplManagerRecord, CancellationToken cancellationToken = default);
}
