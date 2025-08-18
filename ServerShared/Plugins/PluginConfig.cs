using Serilog;
using ServerShared.Controllers;

namespace ServerShared.Plugins;

/// <summary>
/// Interface for accepting a configuration file as a generic type.
/// </summary>
/// <typeparam name="TConfig">The configuration of the <see cref="Plugin"/>.</typeparam>
public abstract class Plugin<TConfig> : Plugin where TConfig : new()
{
    /// <summary>
    /// The configuration of the <see cref="Plugin"/>.
    /// </summary>
    public virtual TConfig? Config { get; set; }

    /// <summary>
    /// The file name of the configuration file.
    /// </summary>
    public virtual string ConfigFileName { get; set; } = string.Empty;

    /// <inheritdoc/>
    public override void LoadConfigs()
    {
        if (string.IsNullOrEmpty(ConfigFileName))
            ConfigFileName = $"{Name}Config.json";
        Config = JsonController.Read<TConfig>(ConfigFileName);
    }

    /// <summary>
    /// Saves the configuration of the <see cref="Plugin"/> to its configuration file.
    /// </summary>
    public void SaveConfig()
    {
        if (Config is null)
        {
            Log.Warning("Failed to save the configuration file for {Name}, the configuration is null.", Name);
            return;
        }

        JsonController.Save(Config, ConfigFileName);
    }
}