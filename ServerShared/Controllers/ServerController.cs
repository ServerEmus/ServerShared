using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using NetCoreServer;
using Serilog;
using ServerShared.CommonModels;
using ServerShared.Server;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Authentication;

namespace ServerShared.Controllers;

/// <summary>
/// Controller for <see cref="UplayServer"/>.
/// </summary>
public static class ServerController
{
    readonly static Assembly ServerManagerAssembly = Assembly.GetAssembly(typeof(ServerController))!;
    readonly static Dictionary<string, Dictionary<HTTPAttribute, MethodInfo>> HTTP_Plugins = [];
    readonly static Dictionary<string, Dictionary<WSAttribute, MethodInfo>> WS_Plugins = [];
    readonly static Dictionary<HTTPAttribute, MethodInfo> Main_HTTP = AttributeMethodHelper.GetMethodAndAttribute<HTTPAttribute>(ServerManagerAssembly);
    readonly static Dictionary<WSAttribute, MethodInfo> Main_WS = AttributeMethodHelper.GetMethodAndAttribute<WSAttribute>(ServerManagerAssembly);
    static UplayServer? server;

    /// <summary>
    /// Getting the <see cref="UplayServer"/>.
    /// </summary>
    public static UplayServer? Server
        => server;

    /// <summary>
    /// Starting the server.
    /// </summary>
    public static void Start()
    {
        Directory.CreateDirectory("logs");
        DebugPrinter.EnableLogs = true;
        //DebugPrinter.PrintToConsole = true;
        ArgumentNullException.ThrowIfNull(ServerManagerAssembly, nameof(ServerManagerAssembly));
        SslContext? context = CertHelper.GetContextNoValidate(SslProtocols.Tls12, $"cert/services.pfx", ServerConfig.Instance.Cert.ServicesCertPassword);
        server = new UplayServer(context);
        AddRoutes(ServerManagerAssembly);
        server.DoReturn404IfFail = false;
        server.ReceivedFailed += Failed;
        server.OnSocketError += OnSocketError;
        server.ReceivedRequestError += RecvReqError;
        server.Context.ClientCertificateRequired = false;
        server.Start();
        Log.Information("Server started on {Address}!", server.Address);
    }

    /// <summary>
    /// Stopping the server.
    /// </summary>
    public static void Stop()
    {
        if (server != null)
        {
            server.Stop();
            server = null;
        }

        Log.Information("Server stopped.");
    }

    /// <summary>
    /// Add Web Routes to <see cref="server"/>.
    /// </summary>
    /// <param name="assembly"></param>
    public static void AddRoutes(Assembly assembly)
    {
        if (server == null)
            return;
        var name = assembly.GetName().FullName;
        HTTP_Plugins.Add(name, AttributeMethodHelper.GetMethodAndAttribute<HTTPAttribute>(assembly));
        WS_Plugins.Add(name, AttributeMethodHelper.GetMethodAndAttribute<WSAttribute>(assembly));
        server.MergeWSAttribute(assembly);
        server.MergeAttribute(assembly);
    }

    /// <summary>
    /// Remove Web routes from <see cref="server"/>.
    /// </summary>
    /// <param name="assembly"></param>
    public static void RemoveRoutes(Assembly assembly)
    {
        var name = assembly.GetName().FullName;
        HTTP_Plugins.Remove(name);
        WS_Plugins.Remove(name);
        if (server == null)
            return;
        server.HTTP_AttributeToMethods = Main_HTTP;
        server.WS_AttributeToMethods = Main_WS.ToDictionary(static x => x.Key.url, static x => x.Value);
        foreach (var plugin in HTTP_Plugins)
        {
            if (plugin.Key == name)
                continue;

            foreach (var item in plugin.Value)
            {
                server.HTTP_AttributeToMethods.TryAdd(item.Key, item.Value);
            }
        }
        foreach (var plugin in WS_Plugins)
        {
            if (plugin.Key == name)
                continue;

            foreach (var item in plugin.Value)
            {
                server.WS_AttributeToMethods.TryAdd(item.Key.url, item.Value);
            }
        }
    }

    private static void RecvReqError(object? sender, (HttpRequest request, string error) e)
    {
        Log.Information("RecvReqError! {url}, {error}", e.request.Url, e.error);
    }

    private static void OnSocketError(object? sender, SocketError e)
    {
        Log.Information("OnSocketError! {error}", e);
    }

    private static void Failed(object? sender, HttpRequest request)
    {
        File.AppendAllText("REQUESTED.txt", $"{request.Method}\n{request.Url}\n{request.Body}\n{request}");
    }
}
