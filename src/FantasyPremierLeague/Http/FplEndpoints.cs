namespace FantasyPremierLeague.Http;

internal static class FplEndpoints
{
    /// <summary>
    /// Provides the member member.
    /// </summary>
    public const string Bootstrap = "bootstrap-static/";
    /// <summary>
    /// Describes the member member.
    /// </summary>
    public const string Me = "me/";
    /// <summary>
    /// Describes the member member.
    /// </summary>
    public const string Entry = "entry/{0}/";
    /// <summary>
    /// Describes the member member.
    /// </summary>
    public const string EntryPicks = "entry/{0}/event/{1}/picks/";
    /// <summary>
    /// Provides the member member.
    /// </summary>
    public const string ElementSummary = "element-summary/{0}/";
    /// <summary>
    /// Describes the member member.
    /// </summary>
    public const string Fixtures = "fixtures/";
    /// <summary>
    /// Describes the member member.
    /// </summary>
    public const string FixturesByEvent = "fixtures/?event={0}";
    /// <summary>
    /// Describes the member member.
    /// </summary>
    public const string MyTeam = "my-team/{0}/";
    /// <summary>
    /// Provides the member member.
    /// </summary>
    public const string Transfers = "transfers/";
    /// <summary>
    /// Describes the member member.
    /// </summary>
    public const string ClassicLeague = "leagues-classic/{0}/standings/?page_standings={1}";
}
