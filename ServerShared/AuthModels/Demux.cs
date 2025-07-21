namespace ServerCore.AuthModels;

public class Demux
{
    [LiteDB.BsonId]
    public Guid UserId { get; set; }
    public uint ConnectionId { get; set; }
    public string ConnectionName { get; set; } = string.Empty;
}
