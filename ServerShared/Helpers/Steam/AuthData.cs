using System.Collections.ObjectModel;

namespace ServerShared.Helpers.Steam;

/// <summary>
/// Represent an Auth Data for a steam ticket.
/// </summary>
public class AuthData
{
    /// <summary>
    /// GC Section.
    /// </summary>
    /// <remarks>
    /// Can be <see langword="null"/> !
    /// </remarks>
    public AppTicketGC? GC { get; set; }

    /// <summary>
    /// Session Section.
    /// </summary>
    /// <remarks>
    /// Can be <see langword="null"/> !
    /// </remarks>
    public AppTicketSession? Session { get; set; }

    /// <summary>
    /// The actual Ticket.
    /// </summary>
    public AppTicket Ticket { get; set; } = new();
}