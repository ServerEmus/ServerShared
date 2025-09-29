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
    ZSTD,
    /// <summary>
    /// Deflate Compression used.
    /// </summary>
    Deflate,
    /// <summary>
    /// LZHAM Compression used.
    /// </summary>
    LZHAM,
    /// <summary>
    /// LZO Compression used.
    /// </summary>
    LZO,
    /// <summary>
    /// ZLIB Compression used.
    /// </summary>
    ZLIB,
}