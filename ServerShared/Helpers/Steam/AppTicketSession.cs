using System.Net;

namespace ServerShared.Helpers.Steam;

/// <summary>
/// Represent a Session in steam.
/// </summary>
public class AppTicketSession
{
    /// <summary>
    /// Unknown value.
    /// </summary>
    public uint Unk1 { get; set; }

    /// <summary>
    /// Unknown value.
    /// </summary>
    public uint Unk2 { get; set; }

    /// <summary>
    /// External Ip
    /// </summary>
    public IPAddress SessionExternalIP { get; set; } = IPAddress.Any;

    /// <summary>
    /// Internal Ip
    /// </summary>
    public IPAddress SessionInternalIP { get; set; } = IPAddress.Any;

    /// <summary>
    /// Time of connection with steam client.
    /// </summary>
    public uint ClientConnectionTime { get; set; }

    /// <summary>
    /// How many times a ticket generated.
    /// </summary>
    public uint ClientConnectionCount { get; set; }
}