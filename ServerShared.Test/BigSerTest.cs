using ServerShared.IO;

namespace ServerShared.Test;

internal class BigSer : ICustomSerializable
{
    public string StrData = string.Empty;
    public int Other;
    public long LongTest;
    public DateTimeOffset Time;
    public void Deserialize(EndiannessReader reader)
    {
        StrData = reader.ReadString();
        Other = reader.ReadInt32();
        LongTest = reader.ReadInt64();
        Time = DateTimeOffset.FromFileTime(reader.ReadInt64());
    }

    public void Serialize(EndiannessWriter writer)
    {
        writer.Write(StrData);
        writer.Write(Other);
        writer.Write(LongTest);
        writer.Write(Time.ToFileTime());
    }
}

internal class BigSer2 : ICustomInstanceSerializable<BigSer2>
{
    public string StrData = string.Empty;
    public int Other;
    public long LongTest;
    public DateTimeOffset Time;
    public void Deserialize(EndiannessReader reader)
    {
        StrData = reader.ReadString();
        Other = reader.ReadInt32();
        LongTest = reader.ReadInt64();
        Time = DateTimeOffset.FromFileTime(reader.ReadInt64());
    }

    public void Serialize(EndiannessWriter writer)
    {
        writer.Write(StrData);
        writer.Write(Other);
        writer.Write(LongTest);
        writer.Write(Time.ToFileTime());
    }

    public static BigSer2? Parse(EndiannessReader reader)
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
        using EndiannessWriter writerBig = new(writeStream, Endianness.Big);
        writerBig.WriteSerializable(ser);
        using MemoryStream readerStream = new(writeStream.ToArray());
        using EndiannessReader readerBig = new(readerStream, Endianness.Big);
        BigSer deser = readerBig.ReadSerializable<BigSer>();
        Assert.Equal(ser.StrData, deser.StrData);
        Assert.Equal(ser.LongTest, deser.LongTest);
        Assert.Equal(ser.Other, deser.Other);
        Assert.Equal(ser.Time, deser.Time);
    }

    [Fact]
    public void Test_LittleSer()
    {
        BigSer ser = new()
        {
            StrData = "AAAAAAAAAAAA",
            LongTest = 555,
            Other = 6456,
            Time = DateTimeOffset.UtcNow,
        };

        using MemoryStream writeStream = new();
        using EndiannessWriter writerBig = new(writeStream, Endianness.Little);
        writerBig.WriteSerializable(ser);
        using MemoryStream readerStream = new(writeStream.ToArray());
        using EndiannessReader readerBig = new(readerStream, Endianness.Little);
        BigSer deser = readerBig.ReadSerializable<BigSer>();
        Assert.Equal(ser.StrData, deser.StrData);
        Assert.Equal(ser.LongTest, deser.LongTest);
        Assert.Equal(ser.Other, deser.Other);
        Assert.Equal(ser.Time, deser.Time);
    }


    [Fact]
    public void Test_CommonSer()
    {
        BigSer ser = new()
        {
            StrData = "AAAAAAAAAAAA",
            LongTest = 555,
            Other = 6456,
            Time = DateTimeOffset.UtcNow,
        };

        using MemoryStream writeStream = new();
        using EndiannessWriter writerBig = new(writeStream, Endianness.Default);
        writerBig.WriteSerializable(ser);
        using MemoryStream readerStream = new(writeStream.ToArray());
        using EndiannessReader readerBig = new(readerStream, Endianness.Default);
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
