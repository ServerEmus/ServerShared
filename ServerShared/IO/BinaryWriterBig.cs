using ServerShared.Types;
using System.Buffers.Binary;

namespace ServerShared.IO;

/// <summary>
/// <see cref="BinaryWriter"/> but writing as Big Endian.
/// </summary>
public class BinaryWriterBig(Stream output) : BinaryWriter(output)
{
    /// <inheritdoc/>
    public override void Write(Half value)
    {
        Span<byte> buffer = stackalloc byte[sizeof(ushort)];
        BinaryPrimitives.WriteHalfBigEndian(buffer, value);
        OutStream.Write(buffer);
    }

    /// <inheritdoc/>
    public override void Write(float value)
    {
        Span<byte> buffer = stackalloc byte[sizeof(float)];
        BinaryPrimitives.WriteSingleBigEndian(buffer, value);
        OutStream.Write(buffer);
    }

    /// <inheritdoc/>
    public override void Write(double value)
    {
        Span<byte> buffer = stackalloc byte[sizeof(double)];
        BinaryPrimitives.WriteDoubleBigEndian(buffer, value);
        OutStream.Write(buffer);
    }

    /// <inheritdoc/>
    public override void Write(decimal value)
    {
        base.Write(value);
        Span<byte> buffer = stackalloc byte[sizeof(decimal)];
        foreach (int bit in decimal.GetBits(value))
        {
            BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(bit)).CopyTo(buffer);
        }
        OutStream.Write(buffer);
    }

    /// <inheritdoc/>
    public override void Write(short value)
    {
        Span<byte> buffer = stackalloc byte[sizeof(short)];
        BinaryPrimitives.WriteInt16BigEndian(buffer, value);
        OutStream.Write(buffer);
    }

    /// <summary>
    /// Writes a three-byte signed integer to the current stream and advances the current position of the stream by three bytes.
    /// </summary>
    /// <param name="value">The three-byte signed integer to write.</param>
    public void Write(Int24 value)
    {
        OutStream.Write([ ..value.ToBytes().Reverse()]);
    }

    /// <inheritdoc/>
    public override void Write(int value)
    {
        Span<byte> buffer = stackalloc byte[sizeof(int)];
        BinaryPrimitives.WriteInt32BigEndian(buffer, value);
        OutStream.Write(buffer);
    }

    /// <summary>
    /// Writes a six-byte signed integer to the current stream and advances the current position of the stream by six bytes.
    /// </summary>
    /// <param name="value">The six-byte signed integer to write.</param>
    public void Write(Int48 value)
    {
        OutStream.Write([.. value.ToBytes().Reverse()]);
    }

    /// <inheritdoc/>
    public override void Write(long value)
    {
        Span<byte> buffer = stackalloc byte[sizeof(long)];
        BinaryPrimitives.WriteInt64BigEndian(buffer, value);
        OutStream.Write(buffer);
    }

    /// <inheritdoc/>
    public override void Write(ushort value)
    {
        Span<byte> buffer = stackalloc byte[sizeof(ushort)];
        BinaryPrimitives.WriteUInt16BigEndian(buffer, value);
        OutStream.Write(buffer);
    }

    /// <summary>
    /// Writes a three-byte unsigned integer to the current stream and advances the current position of the stream by three bytes.
    /// </summary>
    /// <param name="value">The three-byte unsigned integer to write.</param>
    public void Write(UInt24 value)
    {
        OutStream.Write([.. value.ToBytes().Reverse()]);
    }

    /// <inheritdoc/>
    public override void Write(uint value)
    {
        Span<byte> buffer = stackalloc byte[sizeof(uint)];
        BinaryPrimitives.WriteUInt32BigEndian(buffer, value);
        OutStream.Write(buffer);
    }

    /// <summary>
    /// Writes a six-byte unsigned integer to the current stream and advances the current position of the stream by six bytes.
    /// </summary>
    /// <param name="value">The six-byte unsigned integer to write.</param>
    public void Write(UInt48 value)
    {
        OutStream.Write([.. value.ToBytes().Reverse()]);
    }

    /// <inheritdoc/>
    public override void Write(ulong value)
    {
        Span<byte> buffer = stackalloc byte[sizeof(ulong)];
        BinaryPrimitives.WriteUInt64BigEndian(buffer, value);
        OutStream.Write(buffer);
    }
}
