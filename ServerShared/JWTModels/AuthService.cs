namespace ServerCore.JWTModels;

public class AuthService : BaseJWT
{
    public string session { get; set; } = string.Empty;
    public string app { get; set; } = string.Empty;
    public string env { get; set; } = string.Empty;
}
