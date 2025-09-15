using ModdableWebServer.Interfaces;
using NetCoreServer;

namespace ServerShared.CommonModels;

/// <summary>
/// Represent a server model. (For creating a server)
/// </summary>
public class ServerModel
{
    /// <summary>
    /// A simple name for the server.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// A port to host the server on.
    /// </summary>
    public required int Port { get; init; }

    /// <summary>
    /// Null means run a simple WS server, a filled context run a WSS server.
    /// </summary>
    public SslContext? Context { get; init; }

    /// <summary>
    /// The Server if exists.
    /// </summary>
    public IServer? Server { get; set; }

    /// <summary>
    /// Gets or sets if the current server is an UDP server.
    /// </summary>
    public bool IsUdp { get; set; } = false;
}