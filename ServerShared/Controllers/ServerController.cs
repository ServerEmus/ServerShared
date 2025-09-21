using ModdableWebServer;
using ModdableWebServer.Interfaces;
using NetCoreServer;
using Serilog;
using ServerShared.CommonModels;
using ServerShared.Server;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace ServerShared.Controllers;

/// <summary>
/// Controller for <see cref="ServerModel"/>'.
/// </summary>
public static class ServerController
{
    readonly static List<ServerModel> InternalServers = [];

    private static SslContext Context { get; }

    /// <summary>
    /// Main and fallback certificate (use for most of localhost)
    /// </summary>
    public static X509Certificate? MainCertificate { get; set; }

    /// <summary>
    /// Collection of certificates to use on websites.
    /// </summary>
    public static List<X509Certificate2> Certificates { get; private set; } = [];

    static ServerController()
    {
#pragma warning disable SYSLIB0039 // Type or member is obsolete
        Context = new()
        {
            ClientCertificateRequired = false,
            RevocationMode = X509RevocationMode.NoCheck,
            CertificateValidationCallback = new(NoValidation),
            Protocols = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13,
            CertificateSelectionCallback = new(CertificateSelect),
        };
#pragma warning restore SYSLIB0039 // Type or member is obsolete
    }

    private static bool NoValidation(object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }

    private static X509Certificate CertificateSelect(object sender, string? hostName)
    {
        if (string.IsNullOrEmpty(hostName))
        {
            Log.Error("We cannot recognise HostName fallback to MainCert");
            if (MainCertificate != null)
                return MainCertificate;
            ArgumentNullException.ThrowIfNull(MainCertificate, nameof(MainCertificate));
#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }
        Log.Information("Certificate search and selecto for HostName: {hostname}", hostName);
        foreach (var cert in Certificates)
        {
            Log.Information("Certificate FN: {fn}, Subject: {subject}", cert.FriendlyName, cert.Subject);
            foreach (var extension in cert.Extensions)
            {
                if (extension is X509SubjectAlternativeNameExtension alternativeNameExtension)
                {
                    foreach (var dns in alternativeNameExtension.EnumerateDnsNames())
                    {
                        Log.Information("Cert DNS name: {dns}", dns);
                        if (dns.Contains(hostName))
                        {
                            Log.Information("Dns contain hostname, returning cert!");
                            return cert;
                        }
                            
                    }
                }
            }
        }
        Log.Error("Certificate not found for {hostname} fallback to MainCert", hostName);
        if (MainCertificate != null)
            return MainCertificate;
        ArgumentNullException.ThrowIfNull(MainCertificate, nameof(MainCertificate));
#pragma warning disable CS8603 // Possible null reference return.
        return null;
#pragma warning restore CS8603 // Possible null reference return.
    }

     

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
        if (serverModel.IsSecure && !serverModel.IsUdp)
        {
            serverModel.Server = new CoreSecureServer(Context, serverModel.Port);
        }
        else if (!serverModel.IsUdp)
        {
            serverModel.Server = new CoreUnsecureServer(serverModel.Port);
        }
        else if (serverModel.IsSecure && serverModel.IsUdp)
        {
            serverModel.Server = new CoreSslUdpServer(Context, serverModel.Port);
        }
        else
        {
            serverModel.Server = new CoreUdpServer(serverModel.Port);
        }
        if (serverModel.Server is IHttpServer server)
        {
            server.DoReturn404IfFail = false;
        }
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

    private static void ServerEvents_SocketError(IServer arg1, Exception? ex, SocketError arg2)
    {
        Log.Error("OnSocketError! {ServerId} {ServerPort} {error} {ex}", arg1.Id, arg1.Port, arg2, ex);
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
