using ModdableWebServer.Servers;
using NetCoreServer;
using ServerShared.EventArguments;
using System.Collections.Concurrent;
using System.Net;

namespace ServerShared.Server;

/// <inheritdoc/>
public class CoreSecureServer(SslContext context, int port) : WSS_Server(context, IPAddress.Any, port)
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

    private void Session_OnConnected(object? sender, SessionGuidEventArgs sessionGuid)
    {
        CoreSessions.TryAdd(sessionGuid.Id, (CoreSecureSession)sender!);
    }

    private void Session_OnDisconnected(object? sender, SessionGuidEventArgs sessionGuid)
    {
        CoreSessions.Remove(sessionGuid.Id, out _);
    }
}