namespace FantasyPremierLeague.SampleApi.Controllers;

/// <summary>
/// Represents a request to authenticate an FPL manager.
/// </summary>
/// <param name="Email">The manager email address.</param>
/// <param name="Password">The manager password.</param>
/// <param name="IncludeDetails">This will allow Entry and Profile object to be loaded after login .</param>
public sealed record LoginRequest(string Email, string Password, bool IncludeDetails = false);
