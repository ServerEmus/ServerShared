namespace ServerShared.UserModels;

public abstract class UserBase
{
    [LiteDB.BsonId]
    public Guid UserId { get; set; }
}
