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
            var apiKey = "UnitTestKey123";
            using(var server = UnitTestHttpServer.StartOnAnyAvailablePort(expectedRequest: "Req", responseToSendBack: ""))
            {
                var client = new InMobileApiClient(new InMobileApiKey(apiKey), baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var response = client.SmsOutgoing.SendSmsMessages(new List<OutgoingSmsMessageCreateRequest>() {
                    new OutgoingSmsMessageCreateRequest(
                        to: "4511111111",
                        text: "Hello world",
                        from: "1245",
                        messageId: "someMessageId",
                        respectBlacklist: true,
                        flash: false,
                        encoding: MessageEncoding.AUTO,
                        validityPeriod: TimeSpan.FromSeconds(55))
                });
            }
        }
    }
}
