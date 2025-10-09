using ServerShared.IO;
using ServerShared.Types;

namespace ServerShared.Test;

public class IOTest
{
    [Fact]
    public void BigIO_Test()
    {
        using MemoryStream writeStream = new();
        using BinaryWriterBig writerBig = new(writeStream);
        writerBig.Write(Half.MaxValue);
        writerBig.Write(float.MaxValue);
        writerBig.Write(double.MaxValue);
        writerBig.Write(decimal.MaxValue);
        writerBig.Write(short.MaxValue);
        writerBig.WriteInt24(Int24.MaxValue);
        writerBig.Write(int.MaxValue);
        writerBig.WriteInt48(Int48.MaxValue);
        writerBig.Write(long.MaxValue);
        writerBig.Write(ushort.MaxValue);
        writerBig.WriteUInt24(UInt24.MaxValue);
        writerBig.Write(uint.MaxValue);
        writerBig.WriteUInt48(UInt48.MaxValue);
        writerBig.Write(ulong.MaxValue);
        using MemoryStream readerStream = new(writeStream.ToArray());
        using BinaryReaderBig readerBig = new(readerStream);
        Assert.Equal(Half.MaxValue, readerBig.ReadHalf());
        Assert.Equal(float.MaxValue, readerBig.ReadSingle());
        Assert.Equal(double.MaxValue, readerBig.ReadDouble());
        Assert.Equal(decimal.MaxValue, readerBig.ReadDecimal());
        Assert.Equal(short.MaxValue, readerBig.ReadInt16());
        Assert.Equal((int)Int24.MaxValue, (int)readerBig.ReadInt24());
        Assert.Equal(int.MaxValue, readerBig.ReadInt32());
        Assert.Equal((long)Int48.MaxValue, (long)readerBig.ReadInt48());
        Assert.Equal(long.MaxValue, readerBig.ReadInt64());
        Assert.Equal(ushort.MaxValue, readerBig.ReadUInt16());
        Assert.Equal((uint)UInt24.MaxValue, (uint)readerBig.ReadUInt24());
        Assert.Equal(uint.MaxValue, readerBig.ReadUInt32());
        Assert.Equal((ulong)UInt48.MaxValue, (ulong)readerBig.ReadUInt48());
        Assert.Equal(ulong.MaxValue, readerBig.ReadUInt64());
    }
}
