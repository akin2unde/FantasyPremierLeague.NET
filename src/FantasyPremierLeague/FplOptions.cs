namespace FantasyPremierLeague;
/// <summary>
/// Provides the FplOptions member.
/// </summary>

public sealed class FplOptions
{
    /// <summary>
    /// Describes the BaseAddress member.
    /// </summary>
    public Uri BaseAddress { get; set; } = new("https://fantasy.premierleague.com/api/");
    /// <summary>
    /// Provides the 2 member.
    /// </summary>
    public string UserAgent { get; set; } = "FantasyPremierLeague.NET/0.2 (+https://github.com/akin2unde/FantasyPremierLeague.NET)";
    /// <summary>
    /// Describes the Timeout member.
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);
    /// <summary>
    /// Describes the RefreshBeforeExpiry member.
    /// </summary>
    public TimeSpan RefreshBeforeExpiry { get; set; } = TimeSpan.FromMinutes(2);
    /// <summary>
    /// Describes the ReuseStoredToken member.
    /// </summary>
    public bool ReuseStoredToken { get; set; } = true;

    /// <summary>
    /// Describes the Load Profile and Entry after Login.
    /// </summary>
    public bool LoadProfileAfterLogin { get; set; } = false;
}
