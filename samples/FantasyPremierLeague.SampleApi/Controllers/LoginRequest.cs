namespace FantasyPremierLeague.SampleApi.Controllers;

/// <summary>
/// Represents a request to authenticate an FPL manager.
/// </summary>
/// <param name="Email">The manager email address.</param>
/// <param name="Password">The manager password.</param>
/// <param name="ForceRefresh">When true, bypasses reuse of a still-valid access token and refreshes or signs in again.</param>
/// <param name="IncludeDetails">When true, loads the manager profile and entry details after authentication.</param>
public sealed record LoginRequest(string Email, string Password, bool ForceRefresh = false, bool IncludeDetails = false);
