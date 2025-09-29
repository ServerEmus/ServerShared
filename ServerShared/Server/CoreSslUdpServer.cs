using ModdableWebServer.Interfaces;
using NetCoreServer;
using System.Collections.Concurrent;
using System.Net;

namespace ServerShared.Server;

/// <inheritdoc/>
public class CoreSslUdpServer(SslContext context, int port) : UdpServer(IPAddress.Any, port) , IServer
{
    /// <summary>
    /// SSL context
    /// </summary>
    public SslContext Context { get; } = context;

    /// <summary>
    /// Connected sessions.
    /// </summary>
    public readonly ConcurrentDictionary<EndPoint, CoreSslUdpSession> Sessions = [];

    /// <inheritdoc/>
    protected override void OnStarted()
    {
        ReceiveAsync();
    }

    /// <inheritdoc/>
    protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
    {
        base.OnReceived(endpoint, buffer, offset, size);
        if (!Sessions.ContainsKey(endpoint))
            Sessions[endpoint] = CreateSession(endpoint);
        CoreSslUdpSession session = Sessions[endpoint];
        session.Process(buffer.AsSpan((int)offset, (int)size));
        ReceiveAsync();
    }

    /// <inheritdoc/>
    public virtual CoreSslUdpSession CreateSession(EndPoint endpoint)
    {
        return new(endpoint, this);
    }

    /// <inheritdoc/>
    public bool DisconnectAll()
    {
        return true;
    }

    bool IServer.Multicast(ReadOnlySpan<byte> buffer)
    {
        return Multicast(buffer) != buffer.Length;
    }

    bool IServer.Multicast(ReadOnlySpan<char> text)
    {
        return Multicast(text) != text.Length;
    }
}
