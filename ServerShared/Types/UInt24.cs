namespace ServerShared.Types;

/// <summary>
/// Represent a 24-bit unsigned integer.
/// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
[Serializable]
public readonly struct UInt24 : IEquatable<UInt24>
{
    public static readonly UInt24 Zero = new(0);
    public static readonly UInt24 MaxValue = new(0x00ffffff);
    public static readonly UInt24 MinValue = new(0);

    private readonly byte m_b0, m_b1, m_b2;

    private UInt24(uint value)
    {
        m_b0 = (byte)(value & 0xFF);
        m_b1 = (byte)((value >> 8) & 0xFF);
        m_b2 = (byte)((value >> 16) & 0x7F);
    }

    public UInt24(byte b0, byte b1, byte b2)
    {
        m_b0 = b0;
        m_b1 = b1;
        m_b2 = (byte)(b2 & 0x7F);
    }

    public UInt24(byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes);

        if (bytes.Length != 3)
            throw new Exception($"Bytes are not 3! {bytes.Length}");
        m_b0 = bytes[0];
        m_b1 = bytes[1];
        m_b2 = (byte)(bytes[2] & 0x7F);
    }

    public static implicit operator UInt24(uint value) => ToUInt24(value);

    public static implicit operator uint(UInt24 i) => ToUInt32(i);

    public byte[] ToBytes()
    {
        return [m_b0, m_b1, m_b2];
    }

    public static UInt24 ToUInt24(uint value) => new(value);

    public static uint ToUInt32(UInt24 i)
    {
        return (uint)(i.m_b0 | (i.m_b1 << 8) | (i.m_b2 << 16));
    }

    public override bool Equals(object? obj)
    {
        if (obj is not UInt24 uint24)
            return false;
        return Equals(uint24);
    }

    public override int GetHashCode()
    {
        return
            m_b0.GetHashCode() +
            m_b1.GetHashCode() +
            m_b2.GetHashCode();
    }

    public static bool operator ==(UInt24 left, UInt24 right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(UInt24 left, UInt24 right)
    {
        return !(left == right);
    }

    public bool Equals(UInt24 other)
    {
        return
            other.m_b0 == m_b0 &&
            other.m_b1 == m_b1 &&
            other.m_b2 == m_b2;
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member