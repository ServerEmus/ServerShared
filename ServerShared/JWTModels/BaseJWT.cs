using System.Text.Json.Serialization;

namespace ServerShared.JWTModels;

/// <summary>
/// Base JWT Model.
/// </summary>
public abstract class BaseJWT
{
    /// <summary>
    /// JWT Subject.
    /// </summary>
    [JsonPropertyName("sub")]
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// JWT Issuer.
    /// </summary>
    [JsonPropertyName("iss")]
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// JWT Expiration.
    /// </summary>
    [JsonPropertyName("exp")]
    public long Expiration { get; set; } = 0;
}
