using ServerShared.IO;

namespace ServerShared.Test;

public class BigSer : IBigSerializable
{
    public string StrData = string.Empty;
    public int Other;
    public long LongTest;
    public DateTimeOffset Time;
    public void Deserialize(BinaryReaderBig reader)
    {
        StrData = reader.ReadString();
        Other = reader.ReadInt32();
        LongTest = reader.ReadInt64();
        Time = DateTimeOffset.FromFileTime(reader.ReadInt64());
    }

    public void Serialize(BinaryWriterBig writer)
    {
        writer.Write(StrData);
        writer.Write(Other);
        writer.Write(LongTest);
        writer.Write(Time.ToFileTime());
    }
}

public class BigSer2 : IBigInstanceSerializable<BigSer2>
{
    public string StrData = string.Empty;
    public int Other;
    public long LongTest;
    public DateTimeOffset Time;
    public void Deserialize(BinaryReaderBig reader)
    {
        StrData = reader.ReadString();
        Other = reader.ReadInt32();
        LongTest = reader.ReadInt64();
        Time = DateTimeOffset.FromFileTime(reader.ReadInt64());
    }

    public void Serialize(BinaryWriterBig writer)
    {
        writer.Write(StrData);
        writer.Write(Other);
        writer.Write(LongTest);
        writer.Write(Time.ToFileTime());
    }
    public static BigSer2? Parse(BinaryReaderBig reader)
    {
        return new()
        { 
            StrData = reader.ReadString(),
            Other = reader.ReadInt32(),
            LongTest = reader.ReadInt64(),
            Time = DateTimeOffset.FromFileTime(reader.ReadInt64())
        };
    }
}

public class BigSerTest
{
    [Fact]
    public void Test_BigSer()
    {
        BigSer ser = new()
        {
            StrData = "AAAAAAAAAAAA",
            LongTest = 555,
            Other = 6456,
            Time = DateTimeOffset.UtcNow,
        };

        using MemoryStream writeStream = new();
        using BinaryWriterBig writerBig = new(writeStream);
        writerBig.WriteSerializable(ser);
        using MemoryStream readerStream = new(writeStream.ToArray());
        using BinaryReaderBig readerBig = new(readerStream);
        BigSer deser = readerBig.ReadSerializable<BigSer>();
        Assert.Equal(ser.StrData, deser.StrData);
        Assert.Equal(ser.LongTest, deser.LongTest);
        Assert.Equal(ser.Other, deser.Other);
        Assert.Equal(ser.Time, deser.Time);
    }

    [Fact]
    public void Test_BigSerInstance()
    {
        BigSer2 ser = new()
        {
            StrData = "AAAAAAAAAAAA",
            LongTest = 555,
            Other = 6456,
            Time = DateTimeOffset.UtcNow,
        };

        using MemoryStream writeStream = new();
        using BinaryWriterBig writerBig = new(writeStream);
        writerBig.WriteSerializable(ser);
        using MemoryStream readerStream = new(writeStream.ToArray());
        using BinaryReaderBig readerBig = new(readerStream);
        BigSer2? deser = readerBig.ReadInstanceSerializable<BigSer2>();
        Assert.NotNull(deser);
        Assert.Equal(ser.StrData, deser.StrData);
        Assert.Equal(ser.LongTest, deser.LongTest);
        Assert.Equal(ser.Other, deser.Other);
        Assert.Equal(ser.Time, deser.Time);
    }
}
