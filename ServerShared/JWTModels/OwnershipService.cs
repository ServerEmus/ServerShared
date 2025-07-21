namespace ServerCore.JWTModels;

public class OwnershipService : BaseJWT
{
    public int uplay_id { get; set; }
    public int product_id { get; set; }
    public int branch_id { get; set; }
    public List<string> flags { get; set; } = [];
}
