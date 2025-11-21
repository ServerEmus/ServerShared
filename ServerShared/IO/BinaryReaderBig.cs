using System.Text;

namespace ServerShared.IO;

/// <summary>
/// <see cref="BinaryReader"/> but reading as Big Endian.
/// </summary>
/// <inheritdoc/>
public class BinaryReaderBig(Stream input, Encoding encoding, bool leaveOpen = false) : EndiannessReader(input, encoding, leaveOpen, Endianness.Big)
{
    /// <inheritdoc/>
    public BinaryReaderBig(Stream input) : this(input, Encoding.UTF8) { }
}