using ServerShared.Types;
using System.Buffers.Binary;
using System.Text;

namespace ServerShared.IO;

/// <summary>
/// <see cref="BinaryReader"/> but reading as Big Endian.
/// </summary>
/// <inheritdoc/>
public class BinaryReaderBig(Stream input, Encoding encoding, bool leaveOpen) : BinaryReader(input, encoding, leaveOpen)
{
    /// <inheritdoc/>
    public BinaryReaderBig(Stream input) : this(input, Encoding.UTF8, false) { }

    /// <inheritdoc/>
    public BinaryReaderBig(Stream input, Encoding encoding) : this(input, encoding, false) { }

    /// <inheritdoc/>
    public override Half ReadHalf()
    {
        return BinaryPrimitives.ReadHalfBigEndian(base.ReadBytes(2));
    }

    /// <inheritdoc/>
    public override float ReadSingle()
    {
        return BinaryPrimitives.ReadSingleBigEndian(base.ReadBytes(4));
    }

    /// <inheritdoc/>
    public override double ReadDouble()
    {
        return BinaryPrimitives.ReadDoubleBigEndian(base.ReadBytes(8));
    }

    /// <inheritdoc/>
    public override decimal ReadDecimal()
    {
        ReadOnlySpan<byte> bytes = base.ReadBytes(16);
        Span<int> ints =
        [
            BinaryPrimitives.ReadInt32BigEndian(bytes), // low
            BinaryPrimitives.ReadInt32BigEndian(bytes[4..]), // mid
            BinaryPrimitives.ReadInt32BigEndian(bytes[8..]), // high
            BinaryPrimitives.ReadInt32BigEndian(bytes[12..]), // flags
        ];
        return new(ints);
    }

    /// <inheritdoc/>
    public override short ReadInt16()
    {
        return BinaryPrimitives.ReadInt16BigEndian(base.ReadBytes(2));
    }

    /// <summary>
    /// Reading a 3-byte signed integer from the current stream and advances the current position of the stream by three bytes.
    /// </summary>
    /// <returns>A 3-byte signed integer from the current stream.</returns>
    public Int24 ReadInt24()
    {
        return new([.. base.ReadBytes(3).Reverse()]);
    }

    /// <inheritdoc/>
    public override int ReadInt32()
    {
        return BinaryPrimitives.ReadInt32BigEndian(base.ReadBytes(4));
    }

    /// <summary>
    /// Reading a 6-byte signed integer from the current stream and advances the current position of the stream by six bytes.
    /// </summary>
    /// <returns>A 6-byte signed integer from the current stream.</returns>
    public Int48 ReadInt48()
    {
        return new([.. base.ReadBytes(6).Reverse()]);
    }

    /// <inheritdoc/>
    public override long ReadInt64()
    {
        return BinaryPrimitives.ReadInt64BigEndian(base.ReadBytes(8));
    }

    /// <inheritdoc/>
    public override ushort ReadUInt16()
    {
        return BinaryPrimitives.ReadUInt16BigEndian(base.ReadBytes(2));
    }

    /// <summary>
    /// Reading a 3-byte unsigned integer from the current stream and advances the current position of the stream by three bytes.
    /// </summary>
    /// <returns>A 3-byte unsigned integer from the current stream.</returns>
    public UInt24 ReadUInt24()
    {
        return new([.. base.ReadBytes(3).Reverse()]);
    }

    /// <inheritdoc/>
    public override uint ReadUInt32()
    {
        return BinaryPrimitives.ReadUInt32BigEndian(base.ReadBytes(4));
    }

    /// <summary>
    /// Reading a 6-byte unsigned integer from the current stream and advances the current position of the stream by six bytes.
    /// </summary>
    /// <returns>A 6-byte unsigned integer from the current stream.</returns>
    public UInt48 ReadUInt48()
    {
        return new([.. base.ReadBytes(6).Reverse()]);
    }

    /// <inheritdoc/>
    public override ulong ReadUInt64()
    {
        return BinaryPrimitives.ReadUInt64BigEndian(base.ReadBytes(8));
    }

    /// <summary>
    /// Reading a <typeparamref name="T"/> from the current stream and advances the current position of the stream.
    /// </summary>
    /// <returns>A <typeparamref name="T"/> from the current stream.</returns>
    public T ReadSerializable<T>() where T : IBigSerializable, new()
    {
        return ReadSerializable<T>(new());
    }

    /// <summary>
    /// Reading a <typeparamref name="T"/> from the current stream and advances the current position of the stream.
    /// </summary>
    /// <returns>A <typeparamref name="T"/> from the current stream.</returns>
    public T ReadSerializable<T>(T serializable) where T : IBigSerializable
    {
        serializable.Deserialize(this);
        return serializable;
    }

    /// <summary>
    /// Reading a <typeparamref name="T"/> from the current stream and advances the current position of the stream.
    /// </summary>
    /// <returns>A <typeparamref name="T"/> from the current stream.</returns>
    public T? ReadInstanceSerializable<T>() where T : IBigInstanceSerializable<T>
    {
        return T.Parse(this);
    }
}