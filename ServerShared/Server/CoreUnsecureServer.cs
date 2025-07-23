using NetCoreServer;
using ModdableWebServer.Servers;
using System.Collections.Concurrent;
using System.Net;
using ServerShared.Interfaces;

namespace ServerShared.Server;

/// <inheritdoc/>
public class CoreUnsecureServer(int port) : WS_Server(IPAddress.Any, port), IServer
{
    /// <summary>
    /// Thread safe <see cref="CoreUnsecureSession"/> list.
    /// </summary>
    public readonly static ConcurrentDictionary<Guid, CoreUnsecureSession> CoreSessions = [];

    /// <inheritdoc/>
    public override bool Start()
    {
        CoreUnsecureSession.OnConnectedEvent += Session_OnConnected;
        CoreUnsecureSession.OnDisconnectedEvent += Session_OnDisconnected;
        return base.Start();
    }

    /// <inheritdoc/>
    public override bool Stop()
    {
        CoreUnsecureSession.OnConnectedEvent -= Session_OnConnected;
        CoreUnsecureSession.OnDisconnectedEvent -= Session_OnDisconnected;
        return base.Stop();
    }

    /// <inheritdoc/>
    protected override TcpSession CreateSession()
    {
        return new CoreUnsecureSession(this);
    }

    private void Session_OnConnected(object? sender, Guid e)
    {
        CoreSessions.TryAdd(e, (CoreUnsecureSession)sender!);
    }

    private void Session_OnDisconnected(object? sender, Guid e)
    {
        CoreSessions.Remove(e, out _);
    }
}