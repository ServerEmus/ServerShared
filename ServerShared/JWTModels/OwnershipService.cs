using System.Text.Json.Serialization;

namespace ServerShared.JWTModels;

public class OwnershipService : BaseJWT
{
    [JsonPropertyName("uplay_id")]
    public int UplayId { get; set; }

    [JsonPropertyName("product_id")]
    public int ProductId { get; set; }

    [JsonPropertyName("branch_id")]
    public int BranchId { get; set; }

    [JsonPropertyName("flags")]
    public List<string> Flags { get; set; } = [];
}
