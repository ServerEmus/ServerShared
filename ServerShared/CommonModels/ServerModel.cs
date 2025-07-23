using NetCoreServer;
using ServerShared.Interfaces;

namespace ServerShared.CommonModels;

/// <summary>
/// Represent a command.
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

    public IServer? Server { get; set; }

}