namespace ServerShared.Helpers.Steam;

/// <summary>
/// Helper for getting the steam AppTicket
/// </summary>
public static class TicketHelper
{
    /// <summary>
    /// Length of <see cref="AppTicketGC"/>
    /// </summary>
    public const uint GCLen = 20;

    /// <summary>
    /// Length of <see cref="AppTicketSession"/>
    /// </summary>
    public const uint SessionLen = 24;

    /// <summary>
    /// Length of the signature.
    /// </summary>
    public const uint SigLen = 128;

    /// <summary>
    /// Getting Ticket Structure from ByteArray
    /// </summary>
    /// <param name="ticket">Ticket Data</param>
    /// <returns>The Ticket Structure</returns>
    public static AuthData GetTicket(byte[] ticket)
    {

        AuthData authData = new();

        using var ms = new MemoryStream(ticket);
        using var ticketReader = new BinaryReader(ms, System.Text.Encoding.UTF8, true);

        try
        {
            uint len = ticketReader.ReadUInt32();
            if (len == GCLen)
            {
                authData.GC = new()
                {
                    GCToken = ticketReader.ReadUInt64(),
                    SteamId = ticketReader.ReadUInt64(),
                    GeneratedDate = DateTimeOffset.FromUnixTimeSeconds(ticketReader.ReadUInt32()).DateTime
                };

                len = ticketReader.ReadUInt32();
                if (len == SessionLen)
                {
                    authData.Session = new()
                    {
                        Unk1 = ticketReader.ReadUInt32(),
                        Unk2 = ticketReader.ReadUInt32(),
                        SessionExternalIP = new(ticketReader.ReadUInt32()),
                        SessionInternalIP = new(ticketReader.ReadUInt32()),
                        ClientConnectionTime = ticketReader.ReadUInt32(),
                        ClientConnectionCount = ticketReader.ReadUInt32()
                    };
                }
                len = ticketReader.ReadUInt32();
                if (len + ms.Position != ms.Length)
                {
                    throw new Exception($"Remaining length ({len}) is not the final length ({ms.Length})!");
                }
            }

            len = ticketReader.ReadUInt32();
            // remaining Len??

            authData.Ticket = new()
            {
                Version = ticketReader.ReadUInt32(),
                SteamID = ticketReader.ReadUInt64(),
                AppID = ticketReader.ReadUInt32(),
                OwnershipTicketExternalIP = new(ticketReader.ReadUInt32()),
                OwnershipTicketInternalIP = new(ticketReader.ReadUInt32()),
                OwnershipFlags = ticketReader.ReadUInt32(),
                OwnershipTicketGenerated = DateTimeOffset.FromUnixTimeSeconds(ticketReader.ReadUInt32()).DateTime,
                OwnershipTicketExpires = DateTimeOffset.FromUnixTimeSeconds(ticketReader.ReadUInt32()).DateTime,
            };

            int licenseCount = ticketReader.ReadUInt16();
            for (int i = 0; i < licenseCount; i++)
            {
                authData.Ticket.Packages.Add(ticketReader.ReadUInt32());
            }

            int dlcCount = ticketReader.ReadUInt16();
            for (int i = 0; i < dlcCount; i++)
            {
                DLC dlc = new()
                {
                    AppId = ticketReader.ReadUInt32(),
                };

                licenseCount = ticketReader.ReadUInt16();

                for (int j = 0; j < licenseCount; j++)
                {
                    dlc.Packages.Add(ticketReader.ReadUInt32());
                }

                authData.Ticket.DLCs.Add(dlc);
            }

            authData.Ticket.Padding = ticketReader.ReadUInt16();

            if (ms.Position + 128 == ms.Length)
            {
                ticketReader.ReadBytes(128);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw;
        }
        return authData;
    }
}
