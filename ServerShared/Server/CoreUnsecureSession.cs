using ModdableWebServer.Sessions;
using ServerShared.EventArguments;

namespace ServerShared.Server;

/// <inheritdoc/>
public class CoreUnsecureSession(CoreUnsecureServer server) : WS_Session(server)
{
    /// <summary>
    /// Bytes received from Stream.
    /// </summary>
    public static event EventHandler<SessionBytesReceivedEventArgs>? OnBytesReceived;

    /// <summary>
    /// <see cref="Guid"/> connected.
    /// </summary>
    public static event EventHandler<SessionGuidEventArgs>? OnConnectedEvent;

    /// <summary>
    /// <see cref="Guid"/> is disconnected.
    /// </summary>
    public static event EventHandler<SessionGuidEventArgs>? OnDisconnectedEvent;

    /// <summary>
    /// Is session is either HTTP or WS.
    /// </summary>
    public bool IsWebSession { get; protected set; }


    /// <inheritdoc/>
    protected override void OnConnected()
        => OnConnectedEvent?.Invoke(this, new(this, Id));

    /// <inheritdoc/>
    protected override void OnDisconnected()
        => OnDisconnectedEvent?.Invoke(this, new(this, Id));

    /// <inheritdoc/>
    protected override void OnReceived(byte[] buffer, long offset, long size)
    {
        var buf = buffer.Take((int)size).Skip((int)offset).ToArray();
        if (char.IsAsciiLetterUpper((char)buf[0]) || WebSocket.WsHandshaked)
        {
            IsWebSession = true;
            base.OnReceived(buffer, offset, size);
        }
        else
        {
            IsWebSession = false;
            OnBytesReceived?.Invoke(this, new(this, buf));
        }
    }
}