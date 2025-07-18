using Serilog;
using System.Reflection;

namespace ServerShared.Plugin;

/// <summary>
/// Managing <see cref="IPlugin"/>. (Load, Unload, Calls)
/// </summary>
public static class PluginManager
{
    private static readonly Dictionary<string, IPlugin> pluginsList = [];
    
    /// <summary>
    /// Load all plugins from Plugins directory.
    /// </summary>
    public static void LoadPlugins()
    {
        string currdir = Directory.GetCurrentDirectory();
        string pluginsPath = Path.Combine(currdir, "Plugins");
        if (!Directory.Exists(pluginsPath))
            Directory.CreateDirectory(pluginsPath);

        List<IPlugin> plugins = [];
        foreach (string file in Directory.GetFiles(pluginsPath, "*.dll"))
        {
            if (file.Contains("ignore"))
                continue;
            var assemlby = Assembly.LoadFile(file);
            if (assemlby == null)
                continue;
            var type = assemlby.GetType("Plugin.Plugin");
            if (type == null)
                continue;
            IPlugin? iPlugin = (IPlugin?)Activator.CreateInstance(type);
            if (iPlugin == null)
                continue;
            plugins.Add(iPlugin);
        }
        plugins = [.. plugins.OrderBy(x => x.Priority)];
        foreach (IPlugin iPlugin in plugins)
        {
            if (pluginsList.TryAdd(iPlugin.Name, iPlugin))
                PluginInit(iPlugin);
        }
    }

    /// <summary>
    /// Unload all plugins.
    /// </summary>
    public static void UnloadPlugins()
    {
        foreach (var plugin in pluginsList)
        {
            plugin.Value.ShutDown();
            plugin.Value.Dispose();
            Log.Debug("Plugin {pluginName} is now unloaded!", plugin.Key);
        }
        pluginsList.Clear();
    }

    /// <summary>
    /// Load plugin from sepcified <paramref name="DllName"/>.
    /// </summary>
    /// <param name="DllName">Name of the dLL file</param>
    public static void LoadPlugin(string DllName)
    {
        string currdir = Directory.GetCurrentDirectory();
        var path = Path.Combine(currdir, "Plugins", $"{DllName}.dll");
        if (!File.Exists(path))
            return;
        var assemlby = Assembly.LoadFile(path);
        if (assemlby == null)
            return;
        var type = assemlby.GetType("Plugin.Plugin");
        if (type == null)
            return;
        IPlugin? iPlugin = (IPlugin?)Activator.CreateInstance(type);
        if (iPlugin == null)
            return;
        if (pluginsList.TryAdd(iPlugin.Name, iPlugin))
            PluginInit(iPlugin);
    }

    /// <summary>
    /// Unload specific <paramref name="pluginName"/>.
    /// </summary>
    /// <param name="pluginName">Name of the plugin</param>
    public static void UnloadPlugin(string pluginName)
    {
        if (!pluginsList.TryGetValue(pluginName, out var plugin))
            return;
        plugin.ShutDown();
        plugin.Dispose();
        pluginsList.Remove(pluginName);
        Log.Debug("Plugin {pluginName} is now unloaded!", pluginName);
    }

    private static void PluginInit(IPlugin iPlugin)
    {
        iPlugin.Initialize();
        Log.Debug("New Plugin Loaded! {Name}, {Priority} ", iPlugin.Name, iPlugin.Priority);
    }
}
