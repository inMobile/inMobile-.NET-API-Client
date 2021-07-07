using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace InMobile.Sms.ApiClient.Test
{
    public class ClassNamingTest
    {
        [Fact]
        public void EnsureAllClassesSpeelInMobileWithCorrectCasing_Test()
        {
            var allTypes = typeof(InMobileApiClient).Assembly.GetTypes();
            foreach(var type in allTypes)
            {
                // If it contains "InMobile" in any case, ensure it is the correct casing
                if (type.Name.ToLower().Contains("inmobile")
                        && !type.Name.Contains("InMobile"))
                {
                    Assert.False(true, "The type " + type.FullName + " does not have InMobile spelled with correct casing");
                }
            }
        }
    }
}
