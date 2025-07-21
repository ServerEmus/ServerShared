namespace ServerCore.ProductModels;

/// <summary>
/// Connecting a <see cref="ProductId" /> ot a specific <see cref="BranchId" />.
/// </summary>
public class ProductBranch
{
    [LiteDB.BsonId]
    public uint ProductId { get; set; }
    public uint BranchId { get; set; } = 0;
    public string BranchName { get; set; } = string.Empty;
    public string BranchPassword { get; set; } = string.Empty;
    public string LatestManifest { get; set; } = string.Empty;
    public string EncryptionKey { get; set; } = string.Empty;
}
