using ModdableWebServer.Interfaces;
using NetCoreServer;
using System.Collections.Concurrent;
using System.Net;

namespace ServerShared.Server;

/// <inheritdoc/>
public class CoreUdpServer(int port) : UdpServer(IPAddress.Any, port) , IServer
{
    /// <summary>
    /// Connected sessions.
    /// </summary>
    public readonly ConcurrentDictionary<EndPoint, CoreUdpSession> Sessions = [];

    /// <inheritdoc/>
    public bool DoReturn404IfFail { get; set; } // TODO: Move this out.

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
        CoreUdpSession session = Sessions[endpoint];
        session.Process(buffer.AsSpan((int)offset, (int)size));
        ReceiveAsync();
    }

    /// <inheritdoc/>
    public virtual CoreUdpSession CreateSession(EndPoint endpoint)
    {
        return new(endpoint, this);
    }

    /// <inheritdoc/>
    protected override void OnSent(EndPoint endpoint, long sent)
    {
        if (!Sessions.TryGetValue(endpoint, out CoreUdpSession? session))
            return;
        session.OnSent(sent);
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
