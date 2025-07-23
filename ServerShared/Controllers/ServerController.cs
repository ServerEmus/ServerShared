using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using NetCoreServer;
using Serilog;
using ServerShared.Server;
using System.Net.Sockets;
using System.Reflection;
using ServerShared.CommonModels;

namespace ServerShared.Controllers;

/// <summary>
/// Controller for <see cref="ServerModel"/>'.
/// </summary>
public static class ServerController
{
    readonly static Assembly ServerManagerAssembly = Assembly.GetAssembly(typeof(ServerController))!;
    readonly static Dictionary<string, Dictionary<HTTPAttribute, MethodInfo>> HTTP_Plugins = [];
    readonly static Dictionary<string, Dictionary<WSAttribute, MethodInfo>> WS_Plugins = [];
    readonly static Dictionary<HTTPAttribute, MethodInfo> Main_HTTP = AttributeMethodHelper.GetMethodAndAttribute<HTTPAttribute>(ServerManagerAssembly);
    readonly static Dictionary<WSAttribute, MethodInfo> Main_WS = AttributeMethodHelper.GetMethodAndAttribute<WSAttribute>(ServerManagerAssembly);
    readonly static List<ServerModel> Servers = [];

    static ServerController()
    {
        ArgumentNullException.ThrowIfNull(ServerManagerAssembly, nameof(ServerManagerAssembly));
    }

    /// <summary>
    /// Starting the servers.
    /// <param name="servers">A list of models for server creation/starting</param>
    /// </summary>
    public static void Start(List<ServerModel> servers)
    {
        foreach (ServerModel server in servers)
            Start(server);
    }

    /// <summary>
    /// Starting the servers.
    /// <param name="serverModel">A model for server creation/starting</param>
    /// </summary>
    public static void Start(ServerModel serverModel)
    {
        if (serverModel.Context != null)
        {
            CoreSecureServer srv = new(serverModel.Context, serverModel.Port);
            srv.DoReturn404IfFail = false;
            srv.ReceivedFailed += Failed;
            srv.OnSocketError += OnSocketError;
            srv.ReceivedRequestError += RecvReqError;
            srv.Context.ClientCertificateRequired = false;
            serverModel.Server = srv;
        }
        else
        {
            CoreUnsecureServer srv = new(serverModel.Port);
            srv.DoReturn404IfFail = false;
            srv.ReceivedFailed += Failed;
            srv.OnSocketError += OnSocketError;
            srv.ReceivedRequestError += RecvReqError;
            serverModel.Server = srv;
        }
        serverModel.Server.Start();
        Servers.Add(serverModel);
        Log.Information("Server started on {Port}!", serverModel.Port);
    }

    /// <summary>
    /// Stopping the server.
    /// </summary>
    public static void Stop(bool clear = false)
    {
        foreach (ServerModel serverModel in Servers)
            serverModel.Server?.Stop();
        if (clear)
            Servers.Clear();
        Log.Information("Servers stopped.");
    }

    private static void RecvReqError(object? sender, (HttpRequest request, string error) e)
    {
        Log.Error("RecvReqError! {url}, {error}", e.request.Url, e.error);
    }

    private static void OnSocketError(object? sender, SocketError e)
    {
        Log.Error("OnSocketError! {error}", e);
    }

    private static void Failed(object? sender, HttpRequest request)
    {
        File.AppendAllText("REQUESTED.txt", $"{request.Method}\n{request.Url}\n{request.Body}\n{request}");
    }
}
