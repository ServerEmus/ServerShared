using System.Numerics;

namespace ServerShared.Types;

/// <summary>
/// Represent a 24-bit signed integer.
/// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
[Serializable]
public readonly struct Int24
{
    public static readonly Int24 Zero = new(0);
    public static readonly Int24 MaxValue = new(8388607);
    public static readonly Int24 MinValue = new(-8388608);

    private readonly byte m_b0, m_b1, m_b2, m_sign;

    private Int24(int value)
    {
        m_b0 = (byte)(value & 0xFF);
        m_b1 = (byte)((value >> 8) & 0xFF);
        m_b2 = (byte)((value >> 16) & 0x7F);
        m_sign = (byte)((value >> 23) & 1);
    }

    public Int24(byte b0, byte b1, byte b2)
    {
        m_b0 = b0;
        m_b1 = b1;
        m_b2 = (byte)(b2 & 0x7F);
        m_sign = (byte)(b2 >> 7 & 1);
    }

    public Int24(byte[] bytes)
    {
        if (bytes.Length != 3)
            throw new Exception($"Bytes are not 3! {bytes.Length}");
        m_b0 = bytes[0];
        m_b1 = bytes[1];
        m_b2 = (byte)(bytes[2] & 0x7F);
        m_sign = (byte)(bytes[2] >> 7 & 1);
    }

    public static implicit operator Int24(int value)
        => new(value);

    public static implicit operator int(Int24 i)
    {
        int value = i.m_b0 | (i.m_b1 << 8) | (i.m_b2 << 16);
        return -(i.m_sign << 23) + value;
    }
    public byte[] ToBytes()
    {
        return [m_b0, m_b1, m_b2];
    }

    public bool IsSigned => m_sign == 1;
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member