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
            var expectedRequestJson = @"{""Messages"":[{""To"":""4511111111"",""Text"":""Hello world"",""From"":""1245"",""MessageId"":""someMessageId"",""RespectBlacklist"":true,""Flash"":false,""encoding"":""auto"",""ValidityPeriodInSeconds"":55}],""StatusCallback"":{""url"":null}}";

            var responseJson = @"{
""results"": [
{
    ""numberDetails"": {
        ""countryCode"": ""45"",
        ""phoneNumber"": ""12345678"",
        ""rawMsisdn"": ""45 12 34 56 78"",
        ""isValidMsisdn"": true,
        ""isAnonymized"": false
    },
    ""text"": ""This is a message text to be sent"",
    ""from"": ""PetShop"",
    ""smsCount"": 1,
    ""messageId"": ""PetShop"",
    ""encoding"": ""gsm7""
}]
}";
            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/sms/outgoing", jsonOrNull: expectedRequestJson);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
            using(var server = UnitTestHttpServer.StartOnAnyAvailablePort(expectedRequest: expectedRequest, response: responseToSendback))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
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
                Assert.NotNull(response);
            }
        }
    }
}

