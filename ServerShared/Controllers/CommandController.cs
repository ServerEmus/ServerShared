using ServerShared.CommonModels;

namespace ServerShared.Controllers;

/// <summary>
/// Controlller for console commands.
/// </summary>
public static class CommandController
{
    private readonly static HashSet<Command> Commands =
    [
        new()
        { 
            Name = "help",
            Description = "List all command and its description.",
            CommandAction = Help,
        }
    ];

    private readonly static List<string> CannotRemoveCommand = 
    [
        "help"
    ];

    /// <summary>
    /// Add command to <see cref="Commands"/>.
    /// </summary>
    /// <param name="command">A <see cref="Command"/> object.</param>
    /// <param name="cannotRemove">Can or cannot this command be removed with <see cref="RemoveCommand(string)"/>.</param>
    /// <returns>Success/Failure for adding into the commands.</returns>
    public static bool AddCommand(Command command, bool cannotRemove = false)
    {
        if (!Commands.Add(command))
            return false;
        if (cannotRemove)
            CannotRemoveCommand.Add(command.Name);
        return true;
    }

    /// <summary>
    /// Remove a <see cref="Command"/> from <see cref="Commands"/> by <paramref name="commandName"/>.
    /// </summary>
    /// <param name="commandName">Name of the command.</param>
    /// <returns><see langword="true"/> for command removed successfully otherwise <see langword="false"/>.</returns>
    public static bool RemoveCommand(string commandName)
    {
        if (CannotRemoveCommand.Contains(commandName))
            return false;
        Commands.RemoveWhere(command => command.Name == commandName);
        return true;
    }

    /// <summary>
    /// Run the command.
    /// </summary>
    /// <param name="commandName">The command name.</param>
    /// <param name="args">Any additional arguments.</param>
    public static void Run(string commandName, params string[] args)
    {
        Command? command = Commands.FirstOrDefault(x=>x.Name == commandName);
        if (command == null) 
            return;
        command.CommandAction.Invoke(args);
    }

    /// <summary>
    /// Run the command with readed <see cref="Console.ReadLine"/>.
    /// </summary>
    /// <param name="line">Readed line.</param>
    public static void Run(string line)
    {
        string[] splitted = line.Split(" ");
        Run(splitted[0], splitted[1..]);
    }

    /// <summary>
    /// Empty command.
    /// </summary>
    /// <param name="_"></param>
    public static void Empty(string[] _) {}

    private static void Help(string[] _)
    {
        Console.WriteLine("Commands:");
        foreach (Command item in Commands)
        {
            Console.WriteLine($"{item.Name} {item.Description}");
        }
    }
}