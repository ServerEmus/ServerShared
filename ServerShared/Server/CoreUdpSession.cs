using System.Net;

namespace ServerShared.Server;

/// <inheritdoc/>
public class CoreUdpSession(EndPoint endPoint, CoreUdpServer server)
{
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
    public CoreUdpServer Server { get; } = server;

    /// <summary>
    /// Handle buffer received notification
    /// </summary>
    /// <param name="bytes">Received buffer</param>
    /// <remarks>
    /// Notification is called when another part of buffer was received from the client
    /// </remarks>
    public virtual void Process(ReadOnlySpan<byte> bytes)
    {

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
    /// Handle datagram sent notification
    /// </summary>
    /// <param name="sent">Size of sent datagram buffer</param>
    /// <remarks>
    /// Notification is called when a datagram was sent to the client.
    /// This handler could be used to send another datagram to the client for instance when the pending size is zero.
    /// </remarks>
    public virtual void OnSent(long sent)
    {

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
