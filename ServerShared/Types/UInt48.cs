namespace ServerShared.Types;

/// <summary>
/// Represent a 48-bit signed integer.
/// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
[Serializable]
public readonly struct UInt48 : IEquatable<UInt48>
{
    public static readonly UInt48 Zero = new(0);
    public static readonly UInt48 MaxValue = new(140737488355327);
    public static readonly UInt48 MinValue = new(0);
    private readonly byte m_b0, m_b1, m_b2, m_b3, m_b4, m_b5;

    private UInt48(ulong value)
    {
        m_b0 = (byte)(value & 0xFF);
        m_b1 = (byte)((value >> 8) & 0xFF);
        m_b2 = (byte)((value >> 16) & 0xFF);
        m_b3 = (byte)((value >> 24) & 0xFF);
        m_b4 = (byte)((value >> 32) & 0xFF);
        m_b5 = (byte)((value >> 40) & 0x7F);
    }

    public UInt48(byte b0, byte b1, byte b2, byte b3, byte b4, byte b5)
    {
        m_b0 = b0;
        m_b1 = b1;
        m_b2 = b2;
        m_b3 = b3;
        m_b4 = b4;
        m_b5 = (byte)(b5 & 0x7F);
    }

    public UInt48(Span<byte> bytes)
    {
        if (bytes.Length < 6)
            throw new Exception($"Bytes are not 6! {bytes.Length}");

        m_b0 = bytes[0];
        m_b1 = bytes[1];
        m_b2 = bytes[2];
        m_b3 = bytes[3];
        m_b4 = bytes[4];
        m_b5 = (byte)(bytes[5] & 0x7F);
    }

    public static implicit operator UInt48(ulong value) => ToUInt48(value);

    public static implicit operator ulong(UInt48 i) => ToUInt64(i);

    public byte[] ToBytes()
    {
        return [m_b0, m_b1, m_b2, m_b3, m_b4, m_b5];
    }

    public static UInt48 ToUInt48(ulong value) => new(value);

    public static ulong ToUInt64(UInt48 i)
    {
        return (ulong)(i.m_b0 + (i.m_b1 << 8) + (i.m_b2 << 16) + ((long)i.m_b3 << 24) + ((long)i.m_b4 << 32) +
                     ((long)i.m_b5 << 40));
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Int48 int48)
            return false;
        return Equals(int48);
    }

    public override int GetHashCode()
    {
        return
            m_b0.GetHashCode() +
            m_b1.GetHashCode() +
            m_b2.GetHashCode() +
            m_b3.GetHashCode() +
            m_b4.GetHashCode() +
            m_b5.GetHashCode();
    }

    public static bool operator ==(UInt48 left, UInt48 right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(UInt48 left, UInt48 right)
    {
        return !(left == right);
    }

    public bool Equals(UInt48 other)
    {
        return
            other.m_b0 == m_b0 &&
            other.m_b1 == m_b1 &&
            other.m_b2 == m_b2 &&
            other.m_b3 == m_b3 &&
            other.m_b4 == m_b4 &&
            other.m_b5 == m_b5;
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member