using ServerShared.Types;
using System.Buffers.Binary;
using System.Text;

namespace ServerShared.IO;

/// <summary>
/// <see cref="BinaryWriter"/> but writing as choosen endianness.
/// </summary>
/// <inheritdoc/>
public class EndiannessWriter(Stream output, Encoding encoding, bool leaveOpen = false, Endianness endianness = Endianness.Default) : BinaryWriter(output, encoding, leaveOpen)
{
    /// <summary>
    /// Gets or sets the endianness of this writer.
    /// </summary>
    public Endianness Endianness { get; set; } = endianness;

    /// <inheritdoc/>
    public EndiannessWriter(Stream output, Endianness endianness = Endianness.Default) : this(output, Encoding.UTF8, endianness: endianness) { }

    /// <inheritdoc/>
    public override void Write(Half value)
    {
        if (Endianness == Endianness.Default)
        {
            base.Write(value);
            return;
        }
        Span<byte> buffer = stackalloc byte[sizeof(ushort)];
        if (Endianness == Endianness.Little)
            BinaryPrimitives.WriteHalfLittleEndian(buffer, value);
        else
            BinaryPrimitives.WriteHalfBigEndian(buffer, value);
        OutStream.Write(buffer);
    }

    /// <inheritdoc/>
    public override void Write(float value)
    {
        if (Endianness == Endianness.Default)
        {
            base.Write(value);
            return;
        }
        Span<byte> buffer = stackalloc byte[sizeof(float)];
        if (Endianness == Endianness.Little)
            BinaryPrimitives.WriteSingleLittleEndian(buffer, value);
        else
            BinaryPrimitives.WriteSingleBigEndian(buffer, value);
        OutStream.Write(buffer);
    }

    /// <inheritdoc/>
    public override void Write(double value)
    {
        if (Endianness == Endianness.Default)
        {
            base.Write(value);
            return;
        }
        Span<byte> buffer = stackalloc byte[sizeof(double)];
        if (Endianness == Endianness.Little)
            BinaryPrimitives.WriteDoubleLittleEndian(buffer, value);
        else
            BinaryPrimitives.WriteDoubleBigEndian(buffer, value);
        OutStream.Write(buffer);
    }

    /// <inheritdoc/>
    public override void Write(decimal value)
    {
        if (Endianness == Endianness.Default)
        {
            base.Write(value);
            return;
        }
        Span<byte> buffer = stackalloc byte[sizeof(decimal)];
        int[] bits = decimal.GetBits(value);
        if (Endianness == Endianness.Little)
        {
            BinaryPrimitives.WriteInt32LittleEndian(buffer, bits[0]);
            BinaryPrimitives.WriteInt32LittleEndian(buffer[4..], bits[1]);
            BinaryPrimitives.WriteInt32LittleEndian(buffer[8..], bits[2]);
            BinaryPrimitives.WriteInt32LittleEndian(buffer[12..], bits[3]);
        }
        else
        {
            BinaryPrimitives.WriteInt32BigEndian(buffer, bits[0]);
            BinaryPrimitives.WriteInt32BigEndian(buffer[4..], bits[1]);
            BinaryPrimitives.WriteInt32BigEndian(buffer[8..], bits[2]);
            BinaryPrimitives.WriteInt32BigEndian(buffer[12..], bits[3]);
        }
        OutStream.Write(buffer);
    }

    /// <inheritdoc/>
    public override void Write(short value)
    {
        if (Endianness == Endianness.Default)
        {
            base.Write(value);
            return;
        }
        Span<byte> buffer = stackalloc byte[sizeof(short)];
        if (Endianness == Endianness.Little)
            BinaryPrimitives.WriteInt16LittleEndian(buffer, value);
        else
            BinaryPrimitives.WriteInt16BigEndian(buffer, value);
        OutStream.Write(buffer);
    }

    /// <summary>
    /// Writes a three-byte signed integer to the current stream and advances the current position of the stream by three bytes.
    /// </summary>
    /// <param name="value">The three-byte signed integer to write.</param>
    public void WriteInt24(Int24 value)
    {
        Span<byte> buffer = value.ToBytes();
        if (Endianness == Endianness.Big)
            buffer.Reverse();
        OutStream.Write(buffer);
    }

    /// <inheritdoc/>
    public override void Write(int value)
    {
        if (Endianness == Endianness.Default)
        {
            base.Write(value);
            return;
        }
        Span<byte> buffer = stackalloc byte[sizeof(int)];
        if (Endianness == Endianness.Little)
            BinaryPrimitives.WriteInt32LittleEndian(buffer, value);
        else
            BinaryPrimitives.WriteInt32BigEndian(buffer, value);
        OutStream.Write(buffer);
    }

    /// <summary>
    /// Writes a six-byte signed integer to the current stream and advances the current position of the stream by six bytes.
    /// </summary>
    /// <param name="value">The six-byte signed integer to write.</param>
    public void WriteInt48(Int48 value)
    {
        Span<byte> buffer = value.ToBytes();
        if (Endianness == Endianness.Big)
            buffer.Reverse();
        OutStream.Write(buffer);
    }

    /// <inheritdoc/>
    public override void Write(long value)
    {
        if (Endianness == Endianness.Default)
        {
            base.Write(value);
            return;
        }
        Span<byte> buffer = stackalloc byte[sizeof(long)];
        if (Endianness == Endianness.Little)
            BinaryPrimitives.WriteInt64LittleEndian(buffer, value);
        else
            BinaryPrimitives.WriteInt64BigEndian(buffer, value);
        OutStream.Write(buffer);
    }

    /// <inheritdoc/>
    public override void Write(ushort value)
    {
        if (Endianness == Endianness.Default)
        {
            base.Write(value);
            return;
        }
        Span<byte> buffer = stackalloc byte[sizeof(ushort)];
        if (Endianness == Endianness.Little)
            BinaryPrimitives.WriteUInt16LittleEndian(buffer, value);
        else
            BinaryPrimitives.WriteUInt16BigEndian(buffer, value);
        OutStream.Write(buffer);
    }

    /// <summary>
    /// Writes a three-byte unsigned integer to the current stream and advances the current position of the stream by three bytes.
    /// </summary>
    /// <param name="value">The three-byte unsigned integer to write.</param>
    public void WriteUInt24(UInt24 value)
    {
        Span<byte> buffer = value.ToBytes();
        if (Endianness == Endianness.Big)
            buffer.Reverse();
        OutStream.Write(buffer);
    }

    /// <inheritdoc/>
    public override void Write(uint value)
    {
        if (Endianness == Endianness.Default)
        {
            base.Write(value);
            return;
        }
        Span<byte> buffer = stackalloc byte[sizeof(uint)];
        if (Endianness == Endianness.Little)
            BinaryPrimitives.WriteUInt32LittleEndian(buffer, value);
        else
            BinaryPrimitives.WriteUInt32BigEndian(buffer, value);
        OutStream.Write(buffer);
    }

    /// <summary>
    /// Writes a six-byte unsigned integer to the current stream and advances the current position of the stream by six bytes.
    /// </summary>
    /// <param name="value">The six-byte unsigned integer to write.</param>
    public void WriteUInt48(UInt48 value)
    {
        Span<byte> buffer = value.ToBytes();
        if (Endianness == Endianness.Big)
            buffer.Reverse();
        OutStream.Write(buffer);
    }

    /// <inheritdoc/>
    public override void Write(ulong value)
    {
        if (Endianness == Endianness.Default)
        {
            base.Write(value);
            return;
        }
        Span<byte> buffer = stackalloc byte[sizeof(ulong)];
        if (Endianness == Endianness.Little)
            BinaryPrimitives.WriteUInt64LittleEndian(buffer, value);
        else
            BinaryPrimitives.WriteUInt64BigEndian(buffer, value);
        OutStream.Write(buffer);
    }

    /// <summary>
    /// Writes a <typeparamref name="T"/> to the current stream and advances the current position of the stream.
    /// </summary>
    /// <returns>A <typeparamref name="T"/> to write.</returns>
    public void WriteSerializable<T>(T t) where T : ICustomSerializable
    {
        t.Serialize(this);
    }
}
