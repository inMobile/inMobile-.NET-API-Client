using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace InMobile.Sms.ApiClient.Test.Common;

public class CustomDataTypeBase_Test
{
    [Fact]
    public void OrderingAndComparable_Test()
    {
        var list = new List<UnitTestId1>() { new UnitTestId1(1), new UnitTestId1(3), new UnitTestId1(2), new UnitTestId1(1) };
        var orderedList = list.OrderBy(e => e).ToList();
        Assert.Equal(1, orderedList[0].Value);
        Assert.Equal(1, orderedList[1].Value);
        Assert.Equal(2, orderedList[2].Value);
        Assert.Equal(3, orderedList[3].Value);
    }

    [Fact]
    public void IdTypeMismatchException_Test()
    {
        Assert.Throws<CustomDataTypeBaseTypeMismatchException>(() => new UnitTestId_WithWrongOwnType(id: 123));
    }

    [Fact]
    public void Constructor_Test()
    {
        var q = new UnitTestId1(id: 123);
        Assert.Equal(123, q.Value);
    }

    [Fact]
    public void Equals_Nulls_Test()
    {
        Assert.False(new UnitTestId1(123) == null);
        Assert.False(null == new UnitTestId1(123));

        UnitTestId1 null1 = null;
        UnitTestId1 null2 = null;
        Assert.True(null1 == null2);
        Assert.True(new UnitTestId1(123) != null);
        Assert.True(null != new UnitTestId1(123));
        Assert.True((UnitTestId1)null == (UnitTestId1)null);
        Assert.False((UnitTestId1)null != (UnitTestId1)null);
    }

    [Fact]
    public void Equals_Test()
    {
        Assert.True(new UnitTestId1(123) == new UnitTestId1(123));
        Assert.False(new UnitTestId1(123) != new UnitTestId1(123));
        Assert.True(new UnitTestId1(123).Equals(new UnitTestId1(123)));

        Assert.False(new UnitTestId1(123) == new UnitTestId1(124));


        Assert.True(new UnitTestId1(123) != new UnitTestId1(124));
        Assert.False(new UnitTestId1(123).Equals(124));

        {
            object obj1 = new UnitTestId1(123);
            object obj2 = new UnitTestId2(124);
            Assert.False(obj1 == obj2);
        }

        {
            object obj1 = new UnitTestId1(123);
            object obj2 = new UnitTestId2(124);
            Assert.True(obj1 != obj2);
        }

        Assert.False(new UnitTestId1(123).Equals(new UnitTestId2(123)));
    }

    [Fact]
    public void UsingDouble_ExpectNotAllowed_Test()
    {
        Assert.Throws<UnsupportedValueTypeException>(() => new UnitTestCustomDouble(1));
    }

    [Fact]
    public void UsingFloat_ExpectNotAllowed_Test()
    {
        Assert.Throws<UnsupportedValueTypeException>(() => new UnitTestCustomFloat(1));
    }

    public class UnitTestCustomDouble : CustomDataTypeBase<double, UnitTestCustomDouble>
    {
        public UnitTestCustomDouble(double value) : base(value)
        {
        }
    }


    public class UnitTestCustomFloat : CustomDataTypeBase<float, UnitTestCustomFloat>
    {
        public UnitTestCustomFloat(float value) : base(value)
        {
        }
    }


    public class UnitTestId1 : CustomDataTypeBase<long, UnitTestId1>
    {
        public UnitTestId1(long id) : base(id)
        {
        }
    }

    public class UnitTestId2 : CustomDataTypeBase<long, UnitTestId2>
    {
        public UnitTestId2(long id) : base(id)
        {
        }
    }

    public class UnitTestId_WithWrongOwnType : CustomDataTypeBase<long, UnitTestId1>
    {
        public UnitTestId_WithWrongOwnType(long id) : base(id)
        {
        }
    }
}