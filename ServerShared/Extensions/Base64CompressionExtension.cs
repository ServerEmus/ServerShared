using System.Text;

namespace ServerShared.Extensions;

/// <summary>
/// Extension for Compressing with <see cref="string"/>.
/// </summary>
public static class Base64CompressionExtension
{
    /// <summary>
    /// Compress string with <paramref name="compressionType"/>.
    /// </summary>
    /// <param name="str">To Compress</param>
    /// <param name="compressionType">Compression Type</param>
    /// <returns>String as Base64</returns>
    public static string ToCompressedBase64(string str, CompressionType compressionType = CompressionType.Zstd)
    {
        return ToCompressedBase64(Encoding.UTF8.GetBytes(str), compressionType);
    }

    /// <summary>
    /// Compress bytes with <paramref name="compressionType"/>.
    /// </summary>
    /// <param name="bytes">To Compress</param>
    /// <param name="compressionType">Compression Type</param>
    /// <returns>String as Base64</returns>
    public static string ToCompressedBase64(byte[] bytes, CompressionType compressionType = CompressionType.Zstd)
    {
        return Convert.ToBase64String(DeCompExtension.Compress(compressionType, bytes));
    }

    /// <summary>
    /// Decompress string with <paramref name="compressionType"/>.
    /// </summary>
    /// <param name="str">To Decompress</param>
    /// <param name="compressionType">Compression Type</param>
    /// <returns>String as Base64</returns>
    public static string FromCompressedBase64(string str, CompressionType compressionType = CompressionType.Zstd)
    {
        return FromCompressedBase64(Convert.FromBase64String(str), compressionType);
    }

    /// <summary>
    /// Decompress bytes with <paramref name="compressionType"/>.
    /// </summary>
    /// <param name="bytes">To Decompress</param>
    /// <param name="compressionType">Compression Type</param>
    /// <returns>String as Base64</returns>
    public static string FromCompressedBase64(byte[] bytes, CompressionType compressionType = CompressionType.Zstd)
    {
        return Encoding.UTF8.GetString(DeCompExtension.Decompress(compressionType, bytes, 0));
    }
}
