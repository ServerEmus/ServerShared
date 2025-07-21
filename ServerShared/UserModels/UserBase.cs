namespace ServerShared.UserModels;

/// <summary>
/// Basic of the User Model.
/// </summary>
public abstract class UserBase
{
    /// <summary>
    /// User's Id.
    /// </summary>
    [LiteDB.BsonId]
    public Guid UserId { get; set; }
}
