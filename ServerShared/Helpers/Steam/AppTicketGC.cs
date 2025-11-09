namespace ServerShared.Helpers.Steam;

/// <summary>
/// Represent some "Game Connector" or something.
/// </summary>
public class AppTicketGC
{
    /// <summary>
    /// One time token.
    /// </summary>
    public ulong GCToken { get; set; }

    /// <summary>
    /// SteamId the token is for.
    /// </summary>
    public ulong SteamId { get; set; }

    /// <summary>
    /// Date when <see cref="GCToken"/> created.
    /// </summary>
    public DateTime GeneratedDate { get; set; }
}
