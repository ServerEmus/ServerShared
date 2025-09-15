using Serilog;
using ServerShared.Controllers;

namespace ServerShared.Plugins;

/// <summary>
/// Interface for accepting a configuration file as a generic type.
/// </summary>
/// <typeparam name="TConfig">The configuration of the <see cref="ServerPlugin"/>.</typeparam>
public abstract class ServerPlugin<TConfig> : ServerPlugin where TConfig : new()
{
    /// <summary>
    /// The configuration of the <see cref="ServerPlugin"/>.
    /// </summary>
    public virtual TConfig Config { get; set; } = new();

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
    /// Saves the configuration of the <see cref="ServerPlugin"/> to its configuration file.
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