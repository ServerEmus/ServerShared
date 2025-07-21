namespace ServerShared.CommandController;

// TODO: Better command creation (min arg, etc, show what its about)
public static class CommandController
{
    private static Dictionary<string, Action<string[]>> Commands = new()
    {
        { "help" , Help },
    };

    private static List<string> CannotRemoveCommand = 
    [
        "help"
    ];

    public static bool AddCommand(string commandName, Action<string[]> action, bool cannotRemove = false)
    {
        if (Commands.TryAdd(commandName, action))
        {
            if (cannotRemove)
                CannotRemoveCommand.Add(cannotRemove);
            return true;
        }
        return false;
    }

    public static bool RemoveCommand(string commandName)
    {
        if (CannotRemoveCommand.Contains(commandName))
            return false;
        Commands.Remove(commandName);
        return true;
    }

    public static void Run(string commandName, params string[] args)
    {
        if (Commands.TryGetValue(commandName, out var action))
            action(args);
    }

    public static void Run(string line)
    {
        string[] splitted = line.Split(" ");
        string CommandName = splitted[0];
        string[] Parameter = splitted[1..];
        if (Commands.TryGetValue(CommandName.Replace("!", ""), out var action))
        {
            action(Parameter);
        }
    }

    public static void Empty(string[] args) {}

    public static void Help(string[] _)
        => Console.WriteLine("Commands: " + string.Join(", ", Commands.Keys.ToList()));
}