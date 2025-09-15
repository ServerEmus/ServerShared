namespace ServerShared.Plugins;

/// <summary>
/// Represents a plugin which can be loaded by the <see cref="Controllers.PluginController"/>.
/// </summary>
public abstract class ServerPlugin
{
    /// <summary>
    /// Priority of the <see cref="ServerPlugin"/>.
    /// </summary>
    public abstract uint Priority { get; }

    /// <summary>
    /// Name of the <see cref="ServerPlugin"/>.
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// Called when the <see cref="ServerPlugin"/> is started.
    /// </summary>
    public abstract void Start();

    /// <summary>
    /// Called when the <see cref="ServerPlugin"/> is stopped.
    /// </summary>
    public abstract void Stop();

    /// <summary>
    /// Called before the <see cref="ServerPlugin"/> is enabled.
    /// 
    /// <para>Commonly used to load configurations, or any data files before the plugin is enabled.</para>
    /// </summary>
    public virtual void LoadConfigs() { }
}