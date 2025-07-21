namespace ServerShared.CommonModels;

/// <summary>
/// Represent a command.
/// </summary>
public class Command
{
    /// <summary>
    /// Name of the command.
    /// </summary>
    /// <remarks>
    /// Without the '!'. 
    /// </remarks>
    public required string Name { get; init; }

    /// <summary>
    /// A description for your commmand.
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// The action to run.
    /// </summary>
    public required Action<string[]> CommandAction { get; init; }
}