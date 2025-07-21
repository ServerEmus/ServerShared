namespace ServerCore.JWTModels;

public abstract class BaseJWT
{
    public string sub { get; set; } = string.Empty;
    public string iss { get; set; } = string.Empty;
    public long exp { get; set; } = 0;
}
