using System.Text;

namespace ServerShared.Extensions;

/// <summary>
/// Simple Base64 Converter extension.
/// </summary>
public static class Base64Extension
{
    /// <summary>
    /// Convert <paramref name="plainText"/> to base64 string.
    /// </summary>
    /// <param name="plainText"></param>
    /// <returns></returns>
    public static string ToB64(this string plainText)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
    }

    /// <summary>
    /// Convert <paramref name="base64string"/> to Plain Text.
    /// </summary>
    /// <param name="base64string"></param>
    /// <returns></returns>
    public static string FromB64(this string base64string)
    {
        return Encoding.UTF8.GetString(Convert.FromBase64String(base64string));
    }
}
