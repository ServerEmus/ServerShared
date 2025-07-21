using ServerCore.CommonModels;

namespace ServerCore.ProductModels;

public class ProductConfig
{
    [LiteDB.BsonId]
    public uint ProductId { get; set; }
    public uint ConfigVersion { get; set; } = 0;
    public uint DownloadVersion { get; set; } = 0;
    public string ProductName { get; set; } = string.Empty;
    public Guid SpaceId { get; set; } = Guid.Empty;
    public Guid AppId { get; set; } = Guid.Empty;
    public string Configuration { get; set; } = string.Empty;
    public string StoreConfiguration { get; set; } = string.Empty;
    public string StoreReference { get; set; } = string.Empty;
    public string GameCode { get; set; } = string.Empty;
    public bool Staging { get; set; } = false;
    public List<uint> Associations { get; set; } = [];
    public List<AppFlags> AppFlags { get; set; } = [];
    public Uplay.Ownership.GetUplayPCTicketReq.Types.Platform Platform { get; set; } = Uplay.Ownership.GetUplayPCTicketReq.Types.Platform.Normal;
    public Uplay.Ownership.OwnedGame.Types.ProductType ProductType { get; set; } = Uplay.Ownership.OwnedGame.Types.ProductType.Game;
    public Uplay.Ownership.OwnedGame.Types.State ProductState { get; set; } = Uplay.Ownership.OwnedGame.Types.State.Playable;

    //  Custom Non Ownership related
    public RichPresence RichPresence { get; set; } = new();
    public SessionMax SessionMax { get; set; } = new();
}
