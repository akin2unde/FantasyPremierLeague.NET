using System.Text.Json;

namespace FantasyPremierLeague.Results;

/// <summary>
/// Represents fpl operation result.
/// </summary>
public sealed class FplOperationResult
{
    /// <summary>
    /// Gets or sets the payload on initialization .
    /// </summary>
    public JsonElement Payload { get; init; }

}
