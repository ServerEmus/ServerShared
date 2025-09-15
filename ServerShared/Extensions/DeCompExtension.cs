using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using LzhamWrapper;
using LzhamWrapper.Enums;
using lzo.net;
using ZstdNet;
using Ionic.Zlib;

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
        MemoryStream ms = new((int)outputsize);
        switch (compressionType)
        {
            case CompressionType.Zstd:
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
            case CompressionType.UplayLzham:
                {
                    DecompressionParameters parameters = new()
                    {
                        Flags = DecompressionFlag.ComputeAdler32,
                        DictionarySize = 26,
                        UpdateRate = TableUpdateRate.Default
                    };
                    using LzhamStream lzhamStream = new(new MemoryStream(bytesToDecompress), parameters);
                    lzhamStream.CopyTo(ms);
                    return ms.ToArray();
                }
            case CompressionType.Lzham:
                {
                    DecompressionParameters parameters = new()
                    {
                        Flags = DecompressionFlag.ComputeAdler32 | DecompressionFlag.ReadZlibStream,
                        DictionarySize = 15,
                        UpdateRate = TableUpdateRate.Default
                    };
                    using LzhamStream lzhamStream = new(new MemoryStream(bytesToDecompress), parameters);
                    lzhamStream.CopyTo(ms);
                    return ms.ToArray();
                }
            case CompressionType.LZO:
                {
                    using LzoStream decompressed = new(new MemoryStream(bytesToDecompress), System.IO.Compression.CompressionMode.Decompress);
                    decompressed.CopyTo(ms);
                    return ms.ToArray();
                }
            case CompressionType.Zlib:
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
        switch (compressionType)
        {
            case CompressionType.Zstd:
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
            case CompressionType.UplayLzham:
                return [];
            case CompressionType.Lzham:
                {
                    CompressionParameters compressionParameters = new()
                    {
                        Flags = 0,
                        DictionarySize = 26,
                        UpdateRate = TableUpdateRate.Default,
                        Level = LzhamWrapper.Enums.CompressionLevel.Uber
                    };
                    MemoryStream ms = new();
                    using LzhamStream lzhamStream = new(ms, compressionParameters);
                    lzhamStream.Write(bytesToCompress);
                    return ms.ToArray();
                }
            case CompressionType.LZO:
                {
                    using MemoryStream mem = new();
                    using LzoStream compress = new(mem, System.IO.Compression.CompressionMode.Compress);
                    compress.Write(bytesToCompress);
                    return mem.ToArray();
                }
            case CompressionType.Zlib:
                return ZlibStream.CompressBuffer(bytesToCompress);
            case CompressionType.None:
            default:
                return bytesToCompress;
        }
    }
}
