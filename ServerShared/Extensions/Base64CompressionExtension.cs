using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System.Text;
using ZstdNet;

namespace ServerShared.Extensions;

/// <summary>
/// Extension for Compressing with <see cref="string"/>.
/// </summary>
public static class Base64CompressionExtension
{
    #region ZSTD
    /// <summary>
    /// Compress string with ZSTD
    /// </summary>
    /// <param name="str">To Compress</param>
    /// <returns>String as Base64</returns>
    public static string GetZstdB64(string str)
    {
        return GetZstdB64(Encoding.UTF8.GetBytes(str));
    }

    /// <summary>
    /// Compress bytes with ZSTD
    /// </summary>
    /// <param name="bytes">To Compress</param>
    /// <returns>String as Base64</returns>
    public static string GetZstdB64(byte[] bytes)
    {
        using MemoryStream mem = new();
        using Compressor compressorZstd = new();
        mem.Write(compressorZstd.Wrap(bytes));
        return Convert.ToBase64String(mem.ToArray());
    }

    /// <summary>
    /// Decompress string with ZSTD
    /// </summary>
    /// <param name="str">To Decompress</param>
    /// <returns>String as Base64</returns>
    public static string GetUnZstdB64(string str)
    {
        return GetUnZstdB64(Encoding.UTF8.GetBytes(str));
    }

    /// <summary>
    /// Decompress bytes with ZSTD
    /// </summary>
    /// <param name="bytes">To Decompress</param>
    /// <returns>String as Base64</returns>
    public static string GetUnZstdB64(byte[] bytes)
    {
        using MemoryStream mem = new();
        using Decompressor decompressor = new();
        mem.Write(decompressor.Unwrap(bytes));
        return Convert.ToBase64String(mem.ToArray());
    }
    #endregion
    #region Deflate
    /// <summary>
    /// Compress string with Deflate
    /// </summary>
    /// <param name="str">To Compress</param>
    /// <returns>String as Base64</returns>
    public static string GetDeflateB64(string str)
    {
        return GetDeflateB64(Encoding.UTF8.GetBytes(str));
    }

    /// <summary>
    /// Compress bytes with Deflate
    /// </summary>
    /// <param name="bytes">To Compress</param>
    /// <returns>String as Base64</returns>
    public static string GetDeflateB64(byte[] bytes)
    {
        using MemoryStream mem = new();
        using DeflaterOutputStream deflate = new(mem, new(Deflater.BEST_COMPRESSION, false));
        deflate.Write(bytes);
        return Convert.ToBase64String(mem.ToArray());
    }

    /// <summary>
    /// Decompress string with Deflate
    /// </summary>
    /// <param name="str">To Decompress</param>
    /// <returns>String as Base64</returns>
    public static string GetUnDeflateB64(string str)
    {
        return GetUnDeflateB64(Encoding.UTF8.GetBytes(str));
    }

    /// <summary>
    /// Decompress bytes with Deflate
    /// </summary>
    /// <param name="bytes">To Decompress</param>
    /// <returns>String as Base64</returns>
    public static string GetUnDeflateB64(byte[] bytes)
    {
        using MemoryStream mem = new();
        using InflaterInputStream inf = new(mem);
        inf.ReadExactly(bytes);
        return Convert.ToBase64String(mem.ToArray());
    }
    #endregion
}
