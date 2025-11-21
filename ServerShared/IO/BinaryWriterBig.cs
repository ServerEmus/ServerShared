using System.Text;

namespace ServerShared.IO;

/// <summary>
/// <see cref="BinaryWriter"/> but writing as Big Endian.
/// </summary>
/// <inheritdoc/>
public class BinaryWriterBig(Stream output, Encoding encoding, bool leaveOpen = false) : EndiannessWriter(output, encoding, leaveOpen, Endianness.Big)
{
    /// <inheritdoc/>
    public BinaryWriterBig(Stream output) : this(output, Encoding.UTF8) { }
}
