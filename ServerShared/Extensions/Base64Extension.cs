using System.Text;

namespace ServerShared.Extensions;

public static class Base64Extension
{
    public static string ToB64(this string plainText)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
    }

    public static string FromB64(this string plainText)
    {
        return Encoding.UTF8.GetString(Convert.FromBase64String(plainText));
    }
}
