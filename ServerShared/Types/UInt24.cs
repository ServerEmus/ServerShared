namespace ServerShared.Types;

/// <summary>
/// Represent a 24-bit unsigned integer.
/// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
[Serializable]
public readonly struct UInt24 
{
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
        if (bytes.Length != 3)
            throw new Exception("Bytes are not 3!");
        m_b0 = bytes[0];
        m_b1 = bytes[1];
        m_b2 = (byte)(bytes[2] & 0x7F);
    }

    public static implicit operator UInt24(uint value)
        => new(value);

    public static implicit operator uint(UInt24 i)
    {
        return (uint)(i.m_b0 | (i.m_b1 << 8) | (i.m_b2 << 16));
    }
    public byte[] ToBytes()
    {
        return [m_b0, m_b1, m_b2];
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member