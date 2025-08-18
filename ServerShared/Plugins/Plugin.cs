namespace ServerShared.Plugins;

/// <summary>
/// Represents a plugin which can be loaded by the <see cref="Controllers.PluginController"/>.
/// </summary>
public abstract class Plugin
{
    /// <summary>
    /// Priority of the <see cref="Plugin"/>.
    /// </summary>
    public abstract uint Priority { get; }

    /// <summary>
    /// Name of the <see cref="Plugin"/>.
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// Called when the <see cref="Plugin"/> is enabled.
    /// </summary>
    public abstract void Enable();

    /// <summary>
    /// Called when the <see cref="Plugin"/> is disabled.
    /// </summary>
    public abstract void Disable();

    /// <summary>
    /// Called before the <see cref="Plugin"/> is enabled.
    /// 
    /// <para>Commonly used to load configurations, or any data files before the plugin is enabled.</para>
    /// </summary>
    public virtual void LoadConfigs() { }
}