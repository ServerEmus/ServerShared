using System.Collections.ObjectModel;
using System.Net;

namespace ServerShared.Helpers.Steam;

/// <summary>
/// Represent the ticket.
/// </summary>
public class AppTicket
{
    /// <summary>
    /// Ticket version.
    /// </summary>
    public uint Version { get; set; }

    /// <summary>
    /// SteamId the token is for.
    /// </summary>
    public ulong SteamID { get; set; }

    /// <summary>
    /// AppId the token is generated with.
    /// </summary>
    public uint AppID { get; set; }

    /// <summary>
    /// External Ip
    /// </summary>
    public IPAddress OwnershipTicketExternalIP { get; set; } = IPAddress.Any;

    /// <summary>
    /// Internal Ip
    /// </summary>
    public IPAddress OwnershipTicketInternalIP { get; set; } = IPAddress.Any;

    /// <summary>
    /// Ownership Flags. (Usually 0)
    /// </summary>
    public uint OwnershipFlags { get; set; }

    /// <summary>
    /// Date when a ticket got generated.
    /// </summary>
    public DateTime OwnershipTicketGenerated { get; set; }

    /// <summary>
    /// Date when a ticket will expire.
    /// </summary>
    public DateTime OwnershipTicketExpires { get; set; }

    /// <summary>
    /// Packages this AppId/DLCs activated for.
    /// </summary>
    public Collection<uint> Packages { get; } = [];

    /// <summary>
    /// DLC collection.
    /// </summary>
    public Collection<DLC> DLCs { get; } = [];

    /// <summary>
    /// Padding 0 byte.
    /// </summary>
    public uint Padding { get; set; }
}