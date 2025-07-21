using System.Text.Json.Serialization;

namespace ServerShared.JWTModels;

public class AuthService : BaseJWT
{
    [JsonPropertyName("session")]
    public string Session { get; set; } = string.Empty;

    [JsonPropertyName("app")]
    public string App { get; set; } = string.Empty;

    [JsonPropertyName("env")]
    public string Enviroment { get; set; } = string.Empty;
}
