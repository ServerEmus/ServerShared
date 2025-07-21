namespace ServerShared.CommonModels;

public class CDKey
{
    [LiteDB.BsonId]
    public uint ProductId { get; set; }
    public string Key { get; set; } = string.Empty;
    public bool IsUsed { get; set; }
}
