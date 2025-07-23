using NetCoreServer;
using ModdableWebServer.Servers;
using System.Collections.Concurrent;
using System.Net;
using ServerShared.Interfaces;

namespace ServerShared.Server;

/// <inheritdoc/>
public class CoreSecureServer(SslContext context, int port) : WSS_Server(context, IPAddress.Any, port), IServer
{
    /// <summary>
    /// Thread safe <see cref="CoreSecureSession"/> list.
    /// </summary>
    public readonly static ConcurrentDictionary<Guid, CoreSecureSession> CoreSessions = [];

    /// <inheritdoc/>
    public override bool Start()
    {
        CoreSecureSession.OnConnectedEvent += Session_OnConnected;
        CoreSecureSession.OnDisconnectedEvent += Session_OnDisconnected;
        return base.Start();
    }

    /// <inheritdoc/>
    public override bool Stop()
    {
        CoreSecureSession.OnConnectedEvent -= Session_OnConnected;
        CoreSecureSession.OnDisconnectedEvent -= Session_OnDisconnected;
        return base.Stop();
    }

    /// <inheritdoc/>
    protected override SslSession CreateSession()
    {
        return new CoreSecureSession(this);
    }

    private void Session_OnConnected(object? sender, Guid e)
    {
        CoreSessions.TryAdd(e, (CoreSecureSession)sender!);
    }

    private void Session_OnDisconnected(object? sender, Guid e)
    {
        CoreSessions.Remove(e, out _);
    }
}