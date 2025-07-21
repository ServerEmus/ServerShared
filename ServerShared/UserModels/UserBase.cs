namespace ServerCore.UserModels;

public abstract class UserBase
{
    [LiteDB.BsonId]
    public Guid UserId { get; set; }
}
