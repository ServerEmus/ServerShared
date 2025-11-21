namespace ServerShared.IO;

/// <summary>
/// Endianness reading/writing definition.
/// </summary>
public enum Endianness
{
    /// <summary>
    /// Using the same as this Computer has been made with.
    /// </summary>
    Default,
    /// <summary>
    /// Use Little Endian.
    /// </summary>
    Little,
    /// <summary>
    /// Use Big Endian.
    /// </summary>
    Big,
}
