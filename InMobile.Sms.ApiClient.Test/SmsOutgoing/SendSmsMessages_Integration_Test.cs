using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.SmsOutgoing
{
    public class SendSmsMessages_Integration_Test
    {
        [Fact]
        public void SendSmsMessages_Success_Test()
        {
            var expectedRequestJson = @"{""Messages"":[{""To"":""4511111111"",""Text"":""Hello world"",""From"":""1245"",""MessageId"":""someMessageId"",""RespectBlacklist"":true,""Flash"":false,""Encoding"":""auto"",""ValidityPeriodInSeconds"":55,""StatusCallbackUrl"":null,""SendTime"":""2001-02-03T13:05:06Z""}]}";

            var responseJson = @"{
""results"": [
{
    ""numberDetails"": {
        ""countryCode"": ""45"",
        ""phoneNumber"": ""11111111"",
        ""rawMsisdn"": ""+45 11111111"",
        ""isValidMsisdn"": true,
        ""isAnonymized"": false,
        ""future_field_not_yet_known"": ""Hello""
    },
    ""text"": ""This is a message text to be sent"",
    ""from"": ""PetShop"",
    ""smsCount"": 1,
    ""messageId"": ""someMessageId"",
    ""encoding"": ""ucs2"",
    ""future_field_not_yet_known"": ""Hello""
}]
}";
            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/sms/outgoing", jsonOrNull: expectedRequestJson);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var response = client.SmsOutgoing.SendSmsMessages(new List<OutgoingSmsMessageCreateInfo>() {
                    new OutgoingSmsMessageCreateInfo(
                        to: "4511111111",
                        text: "Hello world",
                        from: "1245",
                        messageId: "someMessageId",
                        respectBlacklist: true,
                        flash: false,
                        encoding: MessageEncoding.Auto,
                        validityPeriod: TimeSpan.FromSeconds(55),
                        sendTime: new DateTime(2001,02,03,14,05,06))
                });
                Assert.NotNull(response);
                Assert.Single(response.Results);
                var singleResult = response.Results.Single();
                Assert.NotNull(singleResult);
                Assert.Equal("someMessageId", singleResult.MessageId);
                Assert.Equal(MessageEncoding.Ucs2, singleResult.Encoding);
                Assert.Equal("PetShop", singleResult.From);
                Assert.Equal("45", singleResult.NumberDetails.CountryCode);
                Assert.Equal("11111111", singleResult.NumberDetails.PhoneNumber);
                Assert.Equal(true, singleResult.NumberDetails.IsValidMsisdn);
                Assert.Equal(false, singleResult.NumberDetails.IsAnonymized);
                Assert.Equal("+45 11111111", singleResult.NumberDetails.RawMsisdn);
            }
        }

        [Fact]
        public void SendSmsMessages_EnsureNotBreakingOfFutureEncodingsAreReceived_Test()
        {
            var expectedRequestJson = @"{""Messages"":[{""To"":""+45 11111111"",""Text"":""Hello world"",""From"":""1245"",""MessageId"":""someMessageId"",""RespectBlacklist"":true,""Flash"":false,""encoding"":""auto"",""ValidityPeriodInSeconds"":55,""StatusCallbackUrl"":null,""SendTime"":null}]}";

            var responseJson = @"{
""results"": [
{
    ""numberDetails"": {
        ""countryCode"": ""45"",
        ""phoneNumber"": ""11111111"",
        ""rawMsisdn"": ""+45 11111111"",
        ""isValidMsisdn"": true,
        ""isAnonymized"": false
    },
    ""text"": ""This is a message text to be sent"",
    ""from"": ""PetShop"",
    ""smsCount"": 1,
    ""messageId"": ""someMessageId"",
    ""encoding"": ""FutureValue""
}]
}";
            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/sms/outgoing", jsonOrNull: expectedRequestJson);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var response = client.SmsOutgoing.SendSmsMessages(new List<OutgoingSmsMessageCreateInfo>() {
                    new OutgoingSmsMessageCreateInfo(
                        to: "+45 11111111",
                        text: "Hello world",
                        from: "1245",
                        messageId: "someMessageId",
                        respectBlacklist: true,
                        flash: false,
                        encoding: MessageEncoding.Auto,
                        validityPeriod: TimeSpan.FromSeconds(55))
                });
                Assert.NotNull(response);
                Assert.Single(response.Results);
                var singleResult = response.Results.Single();
                Assert.NotNull(singleResult);
                
                Assert.Equal(MessageEncoding.Unknown, singleResult.Encoding);
            }
        }
    }
}

