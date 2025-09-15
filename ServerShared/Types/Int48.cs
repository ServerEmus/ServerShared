namespace ServerShared.Types;

/// <summary>
/// Represent a 48-bit signed integer.
/// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
[Serializable]
public readonly struct Int48
{
    private readonly byte m_b0, m_b1, m_b2, m_b3, m_b4, m_b5, m_sign;

    private Int48(long value)
    {
        m_b0 = (byte)(value & 0xFF);
        m_b1 = (byte)((value >> 8) & 0xFF);
        m_b2 = (byte)((value >> 16) & 0xFF);
        m_b3 = (byte)((value >> 24) & 0xFF);
        m_b4 = (byte)((value >> 32) & 0xFF);
        m_b5 = (byte)((value >> 40) & 0x7F);
        m_sign = (byte)((value >> 47) & 1);
    }

    public Int48(byte b0, byte b1, byte b2, byte b3, byte b4, byte b5)
    {
        m_b0 = b0;
        m_b1 = b1;
        m_b2 = b2;
        m_b3 = b3;
        m_b4 = b4;
        m_b5 = (byte)(b5 & 0x7F);
        m_sign = (byte)(b5 >> 7 & 1);
    }

    public Int48(byte[] bytes)
    {
        if (bytes.Length != 5)
            throw new Exception("Bytes are not 5!");
        m_b0 = bytes[0];
        m_b1 = bytes[1];
        m_b2 = bytes[2];
        m_b3 = bytes[3];
        m_b4 = bytes[4];
        m_b5 = (byte)(bytes[5] & 0x7F);
        m_sign = (byte)(bytes[5] >> 7 & 1);
    }

    public static implicit operator Int48(long value)
    {
        return new Int48(value);
    }

    public static implicit operator long(Int48 i)
    {
        long value = i.m_b0 + (i.m_b1 << 8) + (i.m_b2 << 16) + ((long)i.m_b3 << 24) + ((long)i.m_b4 << 32) +
                     ((long)i.m_b5 << 40);
        return -((long)i.m_sign << 47) + value;
    }

    public byte[] ToBytes()
    {
        return [m_b0, m_b1, m_b2, m_b3, m_b4, m_b5];
    }

    public bool IsSigned => m_sign == 1;
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member