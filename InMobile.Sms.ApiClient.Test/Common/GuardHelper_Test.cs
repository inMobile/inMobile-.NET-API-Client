using System;
using Xunit;

namespace InMobile.Sms.ApiClient.Test.Common;

public class GuardHelper_Test
{
    [Fact]
    public void EnsureNotNullOrThrow_WhenNotNull_NotThrows_Test()
    {
        var parameter = "my_value";

        try
        {
            GuardHelper.EnsureNotNullOrThrow(nameof(parameter), parameter);
        }
        catch 
        {
            Assert.Fail("Should not throw an exception");
        }
    }

    [Fact]
    public void EnsureNotNullOrThrow_WhenNull_Throws_Test() 
    {
        var parameter = null as string;

        Assert.Throws<ArgumentException>(() => { GuardHelper.EnsureNotNullOrThrow(nameof(parameter), parameter); });
    }
}