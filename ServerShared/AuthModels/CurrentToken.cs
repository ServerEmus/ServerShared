namespace ServerShared.AuthModels;

public class CurrentToken
{
    [LiteDB.BsonId]
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public TokenType Type { get; set; }
}
