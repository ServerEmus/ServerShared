namespace ServerShared;

/// <summary>
/// Type of the compression.
/// </summary>
public enum CompressionType
{
    /// <summary>
    /// No compression used.
    /// </summary>
    None,
    /// <summary>
    /// ZSTD Compression used.
    /// </summary>
    Zstd,
    /// <summary>
    /// Deflate Compression used.
    /// </summary>
    Deflate,
    /// <summary>
    /// Uplay related LZHAM Compression used.
    /// </summary>
    /// <remarks>
    /// Compressing does not work!
    /// </remarks>
    UplayLzham,
    /// <summary>
    /// LZHAM Compression used.
    /// </summary>
    Lzham,
    /// <summary>
    /// LZO Compression used.
    /// </summary>
    LZO,
    /// <summary>
    /// ZLIB Compression used.
    /// </summary>
    Zlib,
}