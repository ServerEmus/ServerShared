using System.Collections.ObjectModel;

namespace ServerShared.Helpers.Steam;

/// <summary>
/// Represent a DLC in steam.
/// </summary>
public class DLC
{
    /// <summary>
    /// AppId of the DLC.
    /// </summary>
    public uint AppId { get; set; }

    /// <summary>
    /// Packages this AppId come from.
    /// </summary>
    public Collection<uint> Packages { get; } = [];
}