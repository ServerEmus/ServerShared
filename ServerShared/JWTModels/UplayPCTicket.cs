namespace ServerShared.JWTModels;

public class UplayPCTicket : BaseJWT
{
    public int uplay_id { get; set; }
    public int platform { get; set; }
}
