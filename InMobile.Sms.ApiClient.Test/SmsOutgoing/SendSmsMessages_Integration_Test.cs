using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace InMobile.Sms.ApiClient.Test.SmsOutgoing
{
    public class SendSmsMessages_Integration_Test
    {
        [Fact]
        public void SendSmsMessages_Success_Test()
        {
            using(var server = UnitTestHttpServer.StartOnAnyAvailablePort(expectedRequest: "Req", responseToSendBack: ""))
            {
                
            }
        }
    }
}
