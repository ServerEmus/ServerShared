using Serilog;
using ServerShared.Plugins;
using System.Reflection;
using System.Runtime.Loader;

namespace ServerShared.Controllers;

/// <summary>
/// Managing <see cref="Plugin"/>. (Load, Unload, Calls)
/// </summary>
public static class PluginController
{
    static PluginController()
    {
        MainLoadContext.Unloading += MainLoadContext_Unloading;
        MainLoadContext.Resolving += MainLoadContext_Resolving;
    }
    private static readonly AssemblyLoadContext MainLoadContext = AssemblyLoadContext.GetLoadContext(Assembly.GetExecutingAssembly()) ?? AssemblyLoadContext.Default;
    private static readonly Dictionary<string, Plugin> pluginsList = [];
    private static bool isDependenciesLoaded = false;
    private static readonly string Dir = Directory.GetCurrentDirectory();


    /// <summary>
    /// Load all plugins from Plugins directory.
    /// </summary>
    public static void LoadPlugins()
    {
        LoadDependencies();
        string pluginsPath = Path.Combine(Dir, "Plugins");
        if (!Directory.Exists(pluginsPath))
            Directory.CreateDirectory(pluginsPath);

        List<Plugin> plugins = [];
        List<Assembly> LoadedAssemblies = [];
        foreach (string file in Directory.GetFiles(pluginsPath, "*.dll"))
        {
            if (file.Contains(".ignore"))
                continue;
            var assemlby = MainLoadContext.LoadFromAssemblyPath(file);
            if (assemlby == null)
                continue;
            Log.Information("Plugin {asm} loaded!", assemlby.GetName().Name);
            LoadedAssemblies.Add(assemlby);
        }
        foreach (var assemlby in LoadedAssemblies)
        {
            foreach (Type type in assemlby.GetTypes())
            {
                if (type.IsAbstract)
                    continue;

                if (!type.IsSubclassOf(typeof(Plugin)))
                    continue;

                if (Activator.CreateInstance(type) is not Plugin plugin)
                    continue;

                plugins.Add(plugin);
            }
        }
        plugins = [.. plugins.OrderBy(x => x.Priority)];
        foreach (Plugin iPlugin in plugins)
        {
            if (pluginsList.TryAdd(iPlugin.Name, iPlugin))
                PluginInit(iPlugin);
        }
    }

    /// <summary>
    /// Stops all plugins.
    /// </summary>
    public static void StopPlugins()
    {
        foreach (var plugin in pluginsList)
        {
            plugin.Value.Stop();
            Log.Debug("Plugin {pluginName} is now disabled!", plugin.Key);
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
        var assemlby = MainLoadContext.LoadFromAssemblyPath(path);
        if (assemlby == null)
            return;
        foreach (Type type in assemlby.GetTypes())
        {
            if (type.IsAbstract)
                continue;

            if (!type.IsSubclassOf(typeof(Plugin)))
                continue;

            // We create an instance of the type and check if it was successfully created.
            if (Activator.CreateInstance(type) is not Plugin plugin)
                continue;

            if (pluginsList.TryAdd(plugin.Name, plugin))
                PluginInit(plugin);
        }
    }

    /// <summary>
    /// Stops specific <paramref name="pluginName"/>.
    /// </summary>
    /// <param name="pluginName">Name of the plugin</param>
    public static void StopPlugin(string pluginName)
    {
        if (!pluginsList.TryGetValue(pluginName, out var plugin))
            return;
        plugin.Stop();
        pluginsList.Remove(pluginName);
        Log.Debug("Plugin {pluginName} is now unloaded!", pluginName);
    }

    private static void PluginInit(Plugin iPlugin)
    {
        iPlugin.LoadConfigs();
        iPlugin.Start();
        Log.Debug("New Plugin Loaded! With name as {Name} and priority as {Priority}.", iPlugin.Name, iPlugin.Priority);
    }

    private static void LoadDependencies()
    {
        if (isDependenciesLoaded)
            return;

        string DependenciesPath = Path.Combine(Dir, "Dependencies");
        if (!Directory.Exists(DependenciesPath))
            Directory.CreateDirectory(DependenciesPath);

        foreach (string file in Directory.GetFiles(DependenciesPath, "*.dll"))
        {
            var asm = MainLoadContext.LoadFromAssemblyPath(file);
            Log.Debug("Assembly {asm} loaded as Dependency!", asm.GetName().Name);
        }

        isDependenciesLoaded = true;
    }

    private static Assembly? MainLoadContext_Resolving(AssemblyLoadContext context, AssemblyName assemblyName)
    {
        var asm = context.Assemblies.Where(x => x.GetName().FullName == assemblyName.FullName).FirstOrDefault();
        if (asm != null)
            return asm;

        Log.Warning("ERROR! You are missing a Dependency! Name: {AssemblyName}", assemblyName);
        File.WriteAllText("Context_ASM.txt", string.Join("\n", context.Assemblies.Select(x => x.GetName().FullName)));
        return null;
    }

    private static void MainLoadContext_Unloading(AssemblyLoadContext obj)
    {
        Log.Verbose("MainLoadContext_Unloading");
        if (obj.IsCollectible)
            obj.Unload();
    }

}
