using ModdableWebServer.Interfaces;

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
    /// Gets or sets if the current server is an UDP server.
    /// </summary>
    public bool IsUdp { get; set; } = false;

    /// <summary>
    /// Gets or sets if the current server is a secure (ssl) server.
    /// </summary>
    public bool IsSecure { get; init; } = false;

    /// <summary>
    /// The Server if exists.
    /// </summary>
    public IServer? Server { get; set; }
}