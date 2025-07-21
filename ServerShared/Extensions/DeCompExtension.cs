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
    /// Decompress <paramref name="bytesToDecompress"/> with <paramref name="CompressionMethod"/>.
    /// </summary>
    /// <param name="IsCompressed">Indicate the <paramref name="bytesToDecompress"/> is compressed.</param>
    /// <param name="IsCustomLzham">Only for custom lzham handling!</param>
    /// <param name="CompressionMethod">A compression method.</param>
    /// <param name="bytesToDecompress">Data</param>
    /// <param name="outputsize">Decompressed output size.</param>
    /// <returns>Decompressed Data.</returns>
    public static byte[] Decompress(bool IsCompressed, bool IsCustomLzham, string CompressionMethod, byte[] bytesToDecompress, uint outputsize)
    {
        if (!IsCompressed)
            return bytesToDecompress;

        switch (CompressionMethod.ToLower()) // check compression method
        {
            case "zstd":
                Decompressor decompressorZstd = new();
                byte[] returner = decompressorZstd.Unwrap(bytesToDecompress);
                decompressorZstd.Dispose();
                return returner;
            case "deflate":
                InflaterInputStream decompressor = new(new MemoryStream(bytesToDecompress), new(false));
                MemoryStream ms = new((int)outputsize);
                decompressor.CopyTo(ms);
                decompressor.Dispose();
                return ms.ToArray();
            case "lzham":
                {
                    DecompressionParameters parameters = new()
                    {
                        Flags = LzhamWrapper.Enums.DecompressionFlag.ComputeAdler32,
                        DictionarySize = 26,
                        UpdateRate = LzhamWrapper.Enums.TableUpdateRate.Default
                    };
                    if (!IsCustomLzham)
                    {
                        parameters = new()
                        {
                            Flags = LzhamWrapper.Enums.DecompressionFlag.ComputeAdler32 | LzhamWrapper.Enums.DecompressionFlag.ReadZlibStream,
                            DictionarySize = 15,
                            UpdateRate = LzhamWrapper.Enums.TableUpdateRate.Default
                        };
                    }

                    MemoryStream mem = new((int)outputsize);
                    LzhamStream lzhamStream = new(new MemoryStream(bytesToDecompress), parameters);
                    lzhamStream.CopyTo(mem);
                    lzhamStream.Dispose();
                    return mem.ToArray();
                }
            case "lzo":
                {
                    using MemoryStream mem = new();
                    using LzoStream decompressed = new(new MemoryStream(bytesToDecompress), CompressionMode.Decompress);
                    decompressed.CopyTo(mem);
                    return mem.ToArray();
                }
            case "zlib":
                return Ionic.Zlib.ZlibStream.UncompressBuffer(bytesToDecompress);
        }
        return bytesToDecompress;
    }

    /// <summary>
    /// Compress <paramref name="bytesToCompress"/> with <paramref name="CompressionMethod"/>.
    /// </summary>
    /// <param name="IsCustomLzham">Only for custom lzham handling!</param>
    /// <param name="CompressionMethod">A compression method.</param>
    /// <param name="bytesToCompress">Data to be compressed.</param>
    /// <returns>Compressed Data</returns>
    public static byte[] Compress(bool IsCustomLzham, string CompressionMethod, byte[] bytesToCompress)
    {
        switch (CompressionMethod.ToLower()) // check compression method
        {
            case "zstd":
                Compressor compressZstd = new();
                byte[] returner = compressZstd.Wrap(bytesToCompress);
                compressZstd.Dispose();
                return returner;
            case "deflate":
                MemoryStream ms = new();
                Deflater defl = new(Deflater.BEST_COMPRESSION, false);
                DeflaterOutputStream deflate = new(ms, defl);
                deflate.Write(bytesToCompress);
                deflate.Close();
                return ms.ToArray();
            case "lzham":
                //50 MB Limit!
                if (IsCustomLzham)
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
                else
                {
                    //return nothing to indicate we have issues!
                    return [];
                }
            case "lzo":
                {
                    using MemoryStream mem = new();
                    using LzoStream compress = new(mem, CompressionMode.Compress);
                    compress.Write(bytesToCompress);
                    compress.Close();
                    return mem.ToArray();
                }
            case "zlib":
                return Ionic.Zlib.ZlibStream.CompressBuffer(bytesToCompress);
        }
        return bytesToCompress;
    }
}
