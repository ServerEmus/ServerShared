using NetCoreServer;
using ModdableWebServer.Servers;
using System.Collections.Concurrent;
using System.Net;

namespace ServerShared.Server;

/// <inheritdoc/>
public class CoreServer(SslContext context) : WSS_Server(context, IPAddress.Any, 433)
{
    /// <summary>
    /// Thread safe <see cref="CoreSession"/> list.
    /// </summary>
    public readonly static ConcurrentDictionary<Guid, CoreSession> CoreSessions = [];

    /// <inheritdoc/>
    public override bool Start()
    {
        CoreSession.OnConnectedEvent += Session_OnConnected;
        CoreSession.OnDisconnectedEvent += Session_OnDisconnected;
        return base.Start();
    }

    /// <inheritdoc/>
    public override bool Stop()
    {
        CoreSession.OnConnectedEvent -= Session_OnConnected;
        CoreSession.OnDisconnectedEvent -= Session_OnDisconnected;
        return base.Stop();
    }

    /// <inheritdoc/>
    protected override SslSession CreateSession()
    {
        return new CoreSession(this);
    }

    private void Session_OnConnected(object? sender, Guid e)
    {
        CoreSessions.TryAdd(e, (CoreSession)sender!);
    }

    private void Session_OnDisconnected(object? sender, Guid e)
    {
        CoreSessions.Remove(e, out _);
    }
}