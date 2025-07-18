using System.Text.Json;

namespace ServerCore.Models;

public class ServerConfig
{
    private static ServerConfig? _Instance;
    public static ServerConfig Instance
    {
        get
        {
            _Instance ??= LoadConfig();
            return _Instance;
        }
        set { _Instance = value; }
    }

    public _cert Cert { get; set; } = new();
    public class _cert
    {
        public string ServicesCertPassword { get; set; } = "CustomUplay";
    }

    public DMX Demux { get; set; } = new();
    public class DMX
    {
        public string ServerFilesPath { get; set; } = "ServerFiles/";
        public string DownloadGamePath { get; set; } = "ServerFiles/Download/";
        public string DefaultCountryCode { get; set; } = "HU";
        public string DefaultContinentCode { get; set; } = "EU";
        public bool GlobalOwnerShipCheck { get; set; } = true;
        public OwnershipClass Ownership { get; set; } = new();
        public class OwnershipClass
        {
            public bool EnableManifestRequest { get; set; } = true;
            public bool EnableConfigRequest { get; set; } = true;
        }
    }
    public _sql SQL { get; set; } = new();
    public class _sql
    {
        public string AuthSalt { get; set; } = "_CUSTOMDEMUX";
    }


    /// <summary>
    /// Loading existing server config
    /// or if not exist it makes one
    /// </summary>
    /// <returns>New ServerConfig</returns>
    public static ServerConfig LoadConfig()
    {
        if (!File.Exists("ServerConfig.json"))
        {
            _Instance = new();
            File.WriteAllText("ServerConfig.json", JsonSerializer.Serialize(_Instance));
            return _Instance;
        }
        _Instance = JsonSerializer.Deserialize<ServerConfig>(File.ReadAllText("ServerConfig.json"))!;
        return _Instance;
    }
}