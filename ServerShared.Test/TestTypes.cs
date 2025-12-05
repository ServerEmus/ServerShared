using ServerShared.Types;

namespace ServerShared.Test;

public class TestTypes
{
    [Fact]
    public void TestInt24()
    {
        Int24 int24 = new();
        Assert.Equal(0, (int)int24);
        int24 = short.MaxValue;
        Assert.Equal(short.MaxValue, (int)int24);
        int max = Int24.MaxValue;
        int24 = max;
        Assert.Equal(max, (int)int24);
        int min = Int24.MinValue;
        int24 = min;
        Assert.Equal(min, (int)int24);
    }

    [Fact]
    public void TestUInt24()
    {
        UInt24 uint24 = new();
        Assert.Equal(0u, (uint)uint24);
        uint24 = ushort.MaxValue;
        Assert.Equal(ushort.MaxValue, (uint)uint24);
        uint max = UInt24.MaxValue;
        uint24 = max;
        Assert.Equal(max, (uint)uint24);
        uint min = UInt24.MinValue;
        uint24 = min;
        Assert.Equal(min, (uint)uint24);
    }

    [Fact]
    public void TestInt48()
    {
        Int48 int48 = new();
        Assert.Equal(0, (long)int48);
        int48 = short.MaxValue;
        Assert.Equal(short.MaxValue, (long)int48);
        long max = int.MaxValue;
        int48 = max;
        Assert.Equal(max, (long)int48);
        max = Int48.MaxValue;
        int48 = max;
        Assert.Equal(max, (long)int48);
        long min = Int48.MinValue;
        int48 = min;
        Assert.Equal(min, (long)int48);
    }

    [Fact]
    public void TestUInt48()
    {
        UInt48 int48 = new();
        Assert.Equal(0ul, (ulong)int48);
        int48 = ushort.MaxValue;
        Assert.Equal(ushort.MaxValue, (ulong)int48);
        ulong max = uint.MaxValue;
        int48 = max;
        Assert.Equal(max, (ulong)int48);
        max = UInt48.MaxValue;
        int48 = max;
        Assert.Equal(max, (ulong)int48);
        ulong min = UInt48.MinValue;
        int48 = min;
        Assert.Equal(min, (ulong)int48);
    }
}
