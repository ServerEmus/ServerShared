using ModdableWebServer.Interfaces;
using System.Net;
using System.Text;

namespace ServerShared.Server;

/// <inheritdoc/>
public class CoreSslUdpSession(EndPoint endPoint, CoreSslUdpServer server) : ISession
{
    /// <summary>
    /// Bytes received from Stream.
    /// </summary>
    public static event EventHandler<byte[]>? OnBytesReceived;

    /// <summary>
    /// Session Id
    /// </summary>
    public Guid Id { get; } = Guid.NewGuid();

    /// <summary>
    /// Endpoint.
    /// </summary>
    public EndPoint EndPoint { get; } = endPoint;

    /// <summary>
    /// Server
    /// </summary>
    public CoreSslUdpServer Server { get; } = server;

    /// <inheritdoc/>
    public IServer IServer => Server;

    /// <inheritdoc/>
    public bool IsConnected => Server.Sessions.ContainsKey(EndPoint);

    /// <inheritdoc/>
    public bool IsDisposed => !IsConnected;

    /// <summary>
    /// Handle buffer received notification
    /// </summary>
    /// <param name="bytes">Received buffer</param>
    /// <remarks>
    /// Notification is called when another part of buffer was received from the client
    /// </remarks>
    public virtual void Process(ReadOnlySpan<byte> bytes)
    {
        OnBytesReceived?.Invoke(this, bytes.ToArray());
    }

    /// <summary>
    /// Send datagram to the given endpoint (synchronous)
    /// </summary>
    /// <param name="bytes">Datagram buffer to send as a span of bytes</param>
    /// <returns>Size of sent datagram</returns>
    public virtual long Send(ReadOnlySpan<byte> bytes)
    {
        return Server.Send(EndPoint, bytes);
    }

    /// <inheritdoc/>
    public long Send(string text)
    {
        return Server.Send(EndPoint, Encoding.UTF8.GetBytes(text));
    }

    /// <summary>
    /// Send datagram to the given endpoint (asynchronous)
    /// </summary>
    /// <param name="bytes">Datagram buffer to send as a span of bytes</param>
    /// <returns>'true' if the datagram was successfully sent, 'false' if the datagram was not sent</returns>
    public virtual bool SendAsync(ReadOnlySpan<byte> bytes)
    {
        return Server.SendAsync(EndPoint, bytes);
    }

    /// <summary>
    /// Handle server stopped notification
    /// </summary>
    public virtual bool Disconnect()
    {
        Server.Sessions.Remove(EndPoint, out _);
        return true;
    }
}
