using NetCoreServer;
using Serilog;
using ServerShared.Server;
using System.Net.Sockets;
using ServerShared.CommonModels;
using ModdableWebServer;
using ModdableWebServer.Interfaces;

namespace ServerShared.Controllers;

/// <summary>
/// Controller for <see cref="ServerModel"/>'.
/// </summary>
public static class ServerController
{
    readonly static List<ServerModel> InternalServers = [];

    /// <summary>
    /// Starting the servers.
    /// </summary>
    /// <param name="servers">A list of models for server creation / starting.</param>
    public static void Start(List<ServerModel> servers)
    {
        foreach (ServerModel server in servers)
            Start(server);
    }

    /// <summary>
    /// Starting the servers.
    /// </summary>
    /// <param name="serverModel">A model for server creation/starting.</param>
    public static void Start(ServerModel serverModel)
    {
        if (serverModel.Context != null)
        {
            CoreSecureServer srv = new(serverModel.Context, serverModel.Port);
            srv.Context.ClientCertificateRequired = false;
            serverModel.Server = srv;
        }
        else
        {
            CoreUnsecureServer srv = new(serverModel.Port);
            serverModel.Server = srv;
        }
        serverModel.Server.DoReturn404IfFail = false;
        ServerEvents.ReceivedFailed += ServerEvents_ReceivedFailed;
        ServerEvents.SocketError += ServerEvents_SocketError;
        ServerEvents.ReceivedRequestError += ServerEvents_ReceivedRequestError;
        serverModel.Server.Start();
        InternalServers.Add(serverModel);
        Log.Information("Server started on {Port}!", serverModel.Port);
    }

    private static void ServerEvents_ReceivedRequestError(IServer arg1, HttpRequest arg2, string arg3)
    {
        Log.Error("RecvReqError! {ServerId} {ServerPort} {url}, {error}", arg1.Id, arg1.Port, arg2.Url, arg3);
    }

    private static void ServerEvents_SocketError(IServer arg1, SocketError arg2)
    {
        Log.Error("OnSocketError! {ServerId} {ServerPort} {error}", arg1.Id, arg1.Port, arg2);
    }

    private static void ServerEvents_ReceivedFailed(IServer arg1, HttpRequest request)
    {
        File.AppendAllText("REQUESTED.txt", $"{request.Method}\n{request.Url}\n{request.Body}\n{request}");
    }

    /// <summary>
    /// Stopping the server.
    /// </summary>
    public static void Stop(bool clear = false)
    {
        foreach (ServerModel serverModel in InternalServers)
            serverModel.Server?.Stop();
        if (clear)
            InternalServers.Clear();
        Log.Information("Servers stopped.");
    }

    /// <summary>
    ///List of existing <see cref="ServerModel"/>'s.
    /// </summary>
    public static IEnumerable<ServerModel> Servers => InternalServers;

    /// <summary>
    /// Checking if the <paramref name="port"/> being used by <see cref="Servers"/>.
    /// </summary>
    /// <param name="port">Port</param>
    /// <returns></returns>
    public static bool IsPortUsed(int port)
    {
        return InternalServers.Any(server => server.Port == port);
    }
}
