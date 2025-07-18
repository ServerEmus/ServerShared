namespace ServerCore.Models.User;

public abstract class UserBase
{
    [LiteDB.BsonId]
    public Guid UserId { get; set; }
}
