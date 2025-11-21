using ServerShared.Types;
using System.Buffers.Binary;
using System.Text;

namespace ServerShared.IO;

/// <summary>
/// <see cref="BinaryReader"/> but reading as choosen endianness.
/// </summary>
/// <inheritdoc/>
public class EndiannessReader(Stream input, Encoding encoding, bool leaveOpen = false, Endianness endianness = Endianness.Default) : BinaryReader(input, encoding, leaveOpen)
{
    /// <summary>
    /// Gets or sets the endianness of this reader.
    /// </summary>
    public Endianness Endianness { get; set; } = endianness;

    /// <inheritdoc/>
    public EndiannessReader(Stream input, Endianness endianness = Endianness.Default) : this(input, Encoding.UTF8, endianness: endianness) { }

    /// <inheritdoc/>
    public override Half ReadHalf()
    {
        const int readByte = 2;
        return Endianness switch
        {
            Endianness.Little => BinaryPrimitives.ReadHalfLittleEndian(ReadBytes(readByte)),
            Endianness.Big => BinaryPrimitives.ReadHalfBigEndian(ReadBytes(readByte)),
            _ => base.ReadHalf(),
        };
    }

    /// <inheritdoc/>
    public override float ReadSingle()
    {
        const int readByte = 4;
        return Endianness switch
        {
            Endianness.Little => BinaryPrimitives.ReadSingleLittleEndian(ReadBytes(readByte)),
            Endianness.Big => BinaryPrimitives.ReadSingleBigEndian(ReadBytes(readByte)),
            _ => base.ReadSingle(),
        };
    }

    /// <inheritdoc/>
    public override double ReadDouble()
    {
        const int readByte = 8;
        return Endianness switch
        {
            Endianness.Little => BinaryPrimitives.ReadDoubleLittleEndian(ReadBytes(readByte)),
            Endianness.Big => BinaryPrimitives.ReadDoubleBigEndian(ReadBytes(readByte)),
            _ => base.ReadDouble(),
        };
    }

    /// <inheritdoc/>
    public override decimal ReadDecimal()
    {
        if (Endianness == Endianness.Default)
            return base.ReadDecimal();

        ReadOnlySpan<byte> bytes = base.ReadBytes(16);
        Span<int> ints = Endianness == Endianness.Big ?
        [
            BinaryPrimitives.ReadInt32BigEndian(bytes), // low
            BinaryPrimitives.ReadInt32BigEndian(bytes[4..]), // mid
            BinaryPrimitives.ReadInt32BigEndian(bytes[8..]), // high
            BinaryPrimitives.ReadInt32BigEndian(bytes[12..]), // flags
        ]
        :
        [
            BinaryPrimitives.ReadInt32LittleEndian(bytes), // low
            BinaryPrimitives.ReadInt32LittleEndian(bytes[4..]), // mid
            BinaryPrimitives.ReadInt32LittleEndian(bytes[8..]), // high
            BinaryPrimitives.ReadInt32LittleEndian(bytes[12..]), // flags
        ];
        return new(ints);
    }

    /// <inheritdoc/>
    public override short ReadInt16()
    {
        const int readByte = 2;
        return Endianness switch
        {
            Endianness.Little => BinaryPrimitives.ReadInt16LittleEndian(ReadBytes(readByte)),
            Endianness.Big => BinaryPrimitives.ReadInt16BigEndian(ReadBytes(readByte)),
            _ => base.ReadInt16(),
        };
    }

    /// <summary>
    /// Reading a 3-byte signed integer from the current stream and advances the current position of the stream by three bytes.
    /// </summary>
    /// <returns>A 3-byte signed integer from the current stream.</returns>
    public Int24 ReadInt24()
    {
        Span<byte> data = base.ReadBytes(3);
        if (Endianness == Endianness.Big)
            data.Reverse();
        return new(data);
    }

    /// <inheritdoc/>
    public override int ReadInt32()
    {
        const int readByte = 4;
        return Endianness switch
        {
            Endianness.Little => BinaryPrimitives.ReadInt32LittleEndian(ReadBytes(readByte)),
            Endianness.Big => BinaryPrimitives.ReadInt32BigEndian(ReadBytes(readByte)),
            _ => base.ReadInt32(),
        };
    }

    /// <summary>
    /// Reading a 6-byte signed integer from the current stream and advances the current position of the stream by six bytes.
    /// </summary>
    /// <returns>A 6-byte signed integer from the current stream.</returns>
    public Int48 ReadInt48()
    {
        Span<byte> data = base.ReadBytes(6);
        if (Endianness == Endianness.Big)
            data.Reverse();
        return new(data);
    }

    /// <inheritdoc/>
    public override long ReadInt64()
    {
        const int readByte = 8;
        return Endianness switch
        {
            Endianness.Little => BinaryPrimitives.ReadInt64LittleEndian(ReadBytes(readByte)),
            Endianness.Big => BinaryPrimitives.ReadInt64BigEndian(ReadBytes(readByte)),
            _ => base.ReadInt64(),
        };
    }

    /// <inheritdoc/>
    public override ushort ReadUInt16()
    {
        const int readByte = 2;
        return Endianness switch
        {
            Endianness.Little => BinaryPrimitives.ReadUInt16LittleEndian(ReadBytes(readByte)),
            Endianness.Big => BinaryPrimitives.ReadUInt16BigEndian(ReadBytes(readByte)),
            _ => base.ReadUInt16(),
        };
    }

    /// <summary>
    /// Reading a 3-byte unsigned integer from the current stream and advances the current position of the stream by three bytes.
    /// </summary>
    /// <returns>A 3-byte unsigned integer from the current stream.</returns>
    public UInt24 ReadUInt24()
    {
        Span<byte> data = base.ReadBytes(3);
        if (Endianness == Endianness.Big)
            data.Reverse();
        return new(data);
    }

    /// <inheritdoc/>
    public override uint ReadUInt32()
    {
        const int readByte = 4;
        return Endianness switch
        {
            Endianness.Little => BinaryPrimitives.ReadUInt32LittleEndian(ReadBytes(readByte)),
            Endianness.Big => BinaryPrimitives.ReadUInt32BigEndian(ReadBytes(readByte)),
            _ => base.ReadUInt32(),
        };
    }

    /// <summary>
    /// Reading a 6-byte unsigned integer from the current stream and advances the current position of the stream by six bytes.
    /// </summary>
    /// <returns>A 6-byte unsigned integer from the current stream.</returns>
    public UInt48 ReadUInt48()
    {
        Span<byte> data = base.ReadBytes(6);
        if (Endianness == Endianness.Big)
            data.Reverse();
        return new(data);
    }

    /// <inheritdoc/>
    public override ulong ReadUInt64()
    {
        const int readByte = 8;
        return Endianness switch
        {
            Endianness.Little => BinaryPrimitives.ReadUInt64LittleEndian(ReadBytes(readByte)),
            Endianness.Big => BinaryPrimitives.ReadUInt64BigEndian(ReadBytes(readByte)),
            _ => base.ReadUInt64(),
        };
    }

    /// <summary>
    /// Reading a <typeparamref name="T"/> from the current stream and advances the current position of the stream.
    /// </summary>
    /// <returns>A <typeparamref name="T"/> from the current stream.</returns>
    public T ReadSerializable<T>() where T : ICustomSerializable, new()
    {
        return ReadSerializable<T>(new());
    }

    /// <summary>
    /// Reading a <typeparamref name="T"/> from the current stream and advances the current position of the stream.
    /// </summary>
    /// <returns>A <typeparamref name="T"/> from the current stream.</returns>
    public T ReadSerializable<T>(T serializable) where T : ICustomSerializable
    {
        serializable.Deserialize(this);
        return serializable;
    }

    /// <summary>
    /// Reading a <typeparamref name="T"/> from the current stream and advances the current position of the stream.
    /// </summary>
    /// <returns>A <typeparamref name="T"/> from the current stream.</returns>
    public T? ReadInstanceSerializable<T>() where T : ICustomInstanceSerializable<T>
    {
        return T.Parse(this);
    }
}
