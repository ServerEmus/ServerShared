using ModdableWebServer.Servers;
using ServerShared.Interfaces;

namespace ServerShared.Server;

/// <inheritdoc/>
public class CoreUnsecureSession(CoreUnsecureServer server) : WS_Server.Session(server), ISession
{
    /// <summary>
    /// Bytes received as SSL Stream.
    /// </summary>
    public static event EventHandler<byte[]>? OnSSLReceived;

    /// <summary>
    /// <see cref="Guid"/> connected.
    /// </summary>
    public static event EventHandler<Guid>? OnConnectedEvent;

    /// <summary>
    /// <see cref="Guid"/> is disconnected.
    /// </summary>
    public static event EventHandler<Guid>? OnDisconnectedEvent;

    /// <summary>
    /// Is session is SSL not HTTP.
    /// </summary>
    public bool IsSSL { get; protected set; }

    /// <summary>
    /// Session is closed.
    /// </summary>
    public bool IsClosed { get; internal set; }

    /// <inheritdoc/>
    public IServer GetServer()
    {
        return server as CoreUnsecureServer;
    }

    /// <inheritdoc/>
    public override void OnConnected()
    {
        OnConnectedEvent?.Invoke(this, Id);
        IsClosed = false;
    }

    /// <inheritdoc/>
    public override void OnDisconnected()
    {
        OnDisconnectedEvent?.Invoke(this, Id);
        IsClosed = true;
    }

    /// <inheritdoc/>
    public override void OnReceived(byte[] buffer, long offset, long size)
    {
        var buf = buffer.Take((int)size).Skip((int)offset).ToArray();
        if (char.IsAsciiLetterUpper((char)buf[0]) || this.WebSocket.WsHandshaked)
            base.OnReceived(buffer, offset, size);
        else
        {
            IsSSL = true;
            OnSSLReceived?.Invoke(this, buf);
        }
    }
}