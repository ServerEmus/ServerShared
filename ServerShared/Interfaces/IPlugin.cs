namespace ServerShared.Interfaces;

/// <summary>
/// Interface for loading Server Plugins.
/// </summary>
public interface IPlugin : IDisposable
{
    /// <summary>
    /// Priority of the Plugin
    /// </summary>
    uint Priority { get; }

    /// <summary>
    /// Name of the plugin
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Initalize the Plugin
    /// </summary>
    void Initialize();

    /// <summary>
    /// Shutdown the Plugin releated resources.
    /// </summary>
    void ShutDown();
}