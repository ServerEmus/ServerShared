using Serilog;
using ServerShared.Interfaces;
using System.Reflection;

namespace ServerShared.Controllers;

/// <summary>
/// Managing <see cref="IPlugin"/>. (Load, Unload, Calls)
/// </summary>
public static class PluginController
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
        string DependenciesPath = Path.Combine(currdir, "Dependencies");
        if (!Directory.Exists(DependenciesPath))
            Directory.CreateDirectory(DependenciesPath);

        foreach (string file in Directory.GetFiles(DependenciesPath, "*.dll"))
            Assembly.LoadFile(file);

        List<IPlugin> plugins = [];
        List<Assembly> LoadedAssemblies = [];
        foreach (string file in Directory.GetFiles(pluginsPath, "*.dll"))
        {
            if (file.Contains(".ignore"))
                continue;
            var assemlby = Assembly.LoadFile(file);
            if (assemlby == null)
                continue;
            LoadedAssemblies.Add(assemlby);
        }
        foreach (var assemlby in LoadedAssemblies)
        {
            foreach (Type type in assemlby.GetTypes())
            {
                if (!typeof(IPlugin).IsAssignableFrom(type) || type.IsAbstract)
                    continue;

                // We create an instance of the type and check if it was successfully created.
                if (Activator.CreateInstance(type) is not IPlugin plugin)
                    continue;

                plugins.Add(plugin);
            }
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
        foreach (Type type in assemlby.GetTypes())
        {
            if (!typeof(IPlugin).IsAssignableFrom(type) || type.IsAbstract)
                    continue;

            // We create an instance of the type and check if it was successfully created.
            if (Activator.CreateInstance(type) is not IPlugin plugin)
                continue;

            if (pluginsList.TryAdd(plugin.Name, plugin))
                PluginInit(plugin);
        }
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
