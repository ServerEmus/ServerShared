using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Ionic.Zlib;
using LzhamWrapper;
using LzhamWrapper.Enums;
using LzhamWrapper.Parameters;
using lzo.net;
using ZstdNet;

namespace ServerShared.Extensions;

/// <summary>
/// Dceompression and Compression extension for most of the compression methods.
/// </summary>
public static class DeCompExtension
{
    /// <summary>
    /// Decompress <paramref name="bytesToDecompress"/> with <paramref name="compressionType"/>.
    /// </summary>
    /// <param name="compressionType">A compression method.</param>
    /// <param name="bytesToDecompress">Data</param>
    /// <param name="outputsize">Decompressed output size.</param>
    /// <returns>Decompressed Data.</returns>
    public static byte[] Decompress(CompressionType compressionType, byte[] bytesToDecompress, uint outputsize)
    {
        ArgumentNullException.ThrowIfNull(bytesToDecompress);

        MemoryStream ms = new((int)outputsize);
        switch (compressionType)
        {
            case CompressionType.ZSTD:
                {
                    using Decompressor decompressorZstd = new();
                    return decompressorZstd.Unwrap(bytesToDecompress);
                }
            case CompressionType.Deflate:
                {
                    using InflaterInputStream decompressor = new(new MemoryStream(bytesToDecompress), new(false));
                    decompressor.CopyTo(ms);
                    return ms.ToArray();
                }
            case CompressionType.LZHAM:
                {
                    DecompressionParameters parameters = new()
                    {
                        Flags = DecompressionFlag.ComputeAdler32 | DecompressionFlag.ReadZlibStream,
                        DictionarySize = 15,
                    };
                    int len = 0;
                    uint adler = 0;
                    Lzham.DecompressMemory(parameters, ms.GetBuffer(), ref len, 0, bytesToDecompress, bytesToDecompress.Length, 0, ref adler);
                    return [..ms.ToArray().Take(len)];
                }
            case CompressionType.LZO:
                {
                    using LzoStream decompressed = new(new MemoryStream(bytesToDecompress), System.IO.Compression.CompressionMode.Decompress);
                    decompressed.CopyTo(ms);
                    return ms.ToArray();
                }
            case CompressionType.ZLIB:
                return ZlibStream.UncompressBuffer(bytesToDecompress);
            case CompressionType.None:
            default:
                return bytesToDecompress;
        }
    }

    /// <summary>
    /// Compress <paramref name="bytesToCompress"/> with <paramref name="compressionType"/>.
    /// </summary>
    /// <param name="compressionType">A compression method.</param>
    /// <param name="bytesToCompress">Data to be compressed.</param>
    /// <returns>Compressed Data</returns>
    public static byte[] Compress(CompressionType compressionType, byte[] bytesToCompress)
    {
        ArgumentNullException.ThrowIfNull(bytesToCompress);

        switch (compressionType)
        {
            case CompressionType.ZSTD:
                {
                    using Compressor compressZstd = new();
                    return compressZstd.Wrap(bytesToCompress);
                }
            case CompressionType.Deflate:
                {
                    MemoryStream ms = new();
                    Deflater defl = new(Deflater.BEST_COMPRESSION, false);
                    using DeflaterOutputStream deflate = new(ms, defl);
                    deflate.Write(bytesToCompress);
                    return ms.ToArray();
                }
            case CompressionType.LZHAM:
                {
                    CompressionParameters compressionParameters = new()
                    {
                        Flags = CompressionFlag.WriteZlibStream,
                        DictionarySize = 15,
                        Level = LzhamWrapper.Enums.CompressionLevel.Default,
                    };
                    byte[] output = new byte[bytesToCompress.Length];
                    int outlen = 0;
                    uint adler = 0;
                    Lzham.CompressMemory(compressionParameters, output, ref outlen, 0, bytesToCompress, bytesToCompress.Length, 0, ref adler);
                    return [.. output.Take(outlen)];
                }
            case CompressionType.LZO:
                {
                    using MemoryStream mem = new();
                    using LzoStream compress = new(mem, System.IO.Compression.CompressionMode.Compress);
                    compress.Write(bytesToCompress);
                    return mem.ToArray();
                }
            case CompressionType.ZLIB:
                return ZlibStream.CompressBuffer(bytesToCompress);
            case CompressionType.None:
            default:
                return bytesToCompress;
        }
    }
}
