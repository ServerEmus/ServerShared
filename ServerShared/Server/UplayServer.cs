using NetCoreServer;
using ModdableWebServer.Servers;
using System.Collections.Concurrent;
using System.Net;

namespace ServerShared.Server;

/// <inheritdoc/>
public class UplayServer(SslContext context) : WSS_Server(context, IPAddress.Any, 433)
{
    /// <summary>
    /// Thread safe <see cref="UplaySession"/> list.
    /// </summary>
    public readonly static ConcurrentDictionary<Guid, UplaySession> UplaySessions = [];

    /// <inheritdoc/>
    public override bool Start()
    {
        UplaySession.OnConnectedEvent += Session_OnConnected;
        UplaySession.OnDisconnectedEvent += Session_OnDisconnected;
        return base.Start();
    }

    /// <inheritdoc/>
    public override bool Stop()
    {
        UplaySession.OnConnectedEvent -= Session_OnConnected;
        UplaySession.OnDisconnectedEvent -= Session_OnDisconnected;
        return base.Stop();
    }

    /// <inheritdoc/>
    protected override SslSession CreateSession()
    {
        return new UplaySession(this);
    }

    private void Session_OnConnected(object? sender, Guid e)
    {
        UplaySessions.TryAdd(e, (UplaySession)sender!);
    }

    private void Session_OnDisconnected(object? sender, Guid e)
    {
        UplaySessions.Remove(e, out _);
    }
}