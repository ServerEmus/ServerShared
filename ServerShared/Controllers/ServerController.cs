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
/// Controller for <see cref="CoreServer"/>.
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
    /// Starting the server.
    /// </summary>
    public static void Start(List<ServerModel> servers)
    {
        foreach (ServerModel server in servers)
            Start(server);
    }

    public static void Start(ServerModel serverModel)
    {
        if (serverModel.Context != null)
        {
            serverModel.Server = new CoreSecureServer(serverModel.Context, serverModel.Port);
            CoreSecureServer srv = serverModel.Server as CoreSecureServer;
            if (srv == null)
                return;
            srv.DoReturn404IfFail = false;
            srv.ReceivedFailed += Failed;
            srv.OnSocketError += OnSocketError;
            srv.ReceivedRequestError += RecvReqError;
            srv.Context.ClientCertificateRequired = false;
        }
        else
        {
            serverModel.Server = new CoreUnsecureServer(serverModel.Port);
            CoreUnsecureServer srv = serverModel.Server as CoreUnsecureServer;
            if (srv == null)
                return;
            srv.DoReturn404IfFail = false;
            srv.ReceivedFailed += Failed;
            srv.OnSocketError += OnSocketError;
            srv.ReceivedRequestError += RecvReqError;
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
