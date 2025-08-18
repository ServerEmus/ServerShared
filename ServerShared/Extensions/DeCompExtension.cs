using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using LzhamWrapper;
using lzo.net;
using System.IO.Compression;
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
        MemoryStream ms = new((int)outputsize);
        switch (compressionType)
        {
            case CompressionType.None:
                return bytesToDecompress;
            case CompressionType.Zstd:
                Decompressor decompressorZstd = new();
                byte[] returner = decompressorZstd.Unwrap(bytesToDecompress);
                decompressorZstd.Dispose();
                return returner;
            case CompressionType.Deflate:
                InflaterInputStream decompressor = new(new MemoryStream(bytesToDecompress), new(false));
                decompressor.CopyTo(ms);
                decompressor.Dispose();
                return ms.ToArray();
            case CompressionType.UplayLzham:
                {
                    DecompressionParameters parameters = new()
                    {
                        Flags = LzhamWrapper.Enums.DecompressionFlag.ComputeAdler32,
                        DictionarySize = 26,
                        UpdateRate = LzhamWrapper.Enums.TableUpdateRate.Default
                    };
                    LzhamStream lzhamStream = new(new MemoryStream(bytesToDecompress), parameters);
                    lzhamStream.CopyTo(ms);
                    lzhamStream.Dispose();
                    return ms.ToArray();
                }
            case CompressionType.Lzham:
                {
                    DecompressionParameters parameters = new()
                    {
                        Flags = LzhamWrapper.Enums.DecompressionFlag.ComputeAdler32 | LzhamWrapper.Enums.DecompressionFlag.ReadZlibStream,
                        DictionarySize = 15,
                        UpdateRate = LzhamWrapper.Enums.TableUpdateRate.Default
                    };
                    using LzhamStream lzhamStream = new(new MemoryStream(bytesToDecompress), parameters);
                    lzhamStream.CopyTo(ms);
                    lzhamStream.Dispose();
                    return ms.ToArray();
                }
            case CompressionType.LZO:
                {
                    using LzoStream decompressed = new(new MemoryStream(bytesToDecompress), CompressionMode.Decompress);
                    decompressed.CopyTo(ms);
                    return ms.ToArray();
                }
            case CompressionType.Zlib:
                return Ionic.Zlib.ZlibStream.UncompressBuffer(bytesToDecompress);
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
            case CompressionType.None:
                return bytesToCompress;
            case CompressionType.Zstd:
                {
                    Compressor compressZstd = new();
                    byte[] returner = compressZstd.Wrap(bytesToCompress);
                    compressZstd.Dispose();
                    return returner;
                }
            case CompressionType.Deflate:
                {
                    MemoryStream ms = new();
                    Deflater defl = new(Deflater.BEST_COMPRESSION, false);
                    DeflaterOutputStream deflate = new(ms, defl);
                    deflate.Write(bytesToCompress);
                    deflate.Close();
                    return ms.ToArray();
                }
            case CompressionType.UplayLzham:
                return [];
            case CompressionType.Lzham:
                {
                    uint adler = 0;
                    byte[] output = new byte[bytesToCompress.Length * 2];
                    int outsize = output.Length;
                    Lzham.CompressMemory(new()
                    {
                        Flags = 0,
                        DictionarySize = 26,
                        UpdateRate = LzhamWrapper.Enums.TableUpdateRate.Default,
                        Level = LzhamWrapper.Enums.CompressionLevel.Uber
                    }, bytesToCompress, bytesToCompress.Length, 0, output, ref outsize, 0, ref adler);
                    return [.. output.Take(outsize)];
                }
            case CompressionType.LZO:
                {
                    using MemoryStream mem = new();
                    using LzoStream compress = new(mem, CompressionMode.Compress);
                    compress.Write(bytesToCompress);
                    compress.Close();
                    return mem.ToArray();
                }
            case CompressionType.Zlib:
                return Ionic.Zlib.ZlibStream.CompressBuffer(bytesToCompress);
            default:
                return bytesToCompress;
        }
    }
}
