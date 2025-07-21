namespace ServerCore.Models.Auth;

public class AuthModels
{
    [LiteDB.BsonId]
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public TokenType type { get; set; }
}
