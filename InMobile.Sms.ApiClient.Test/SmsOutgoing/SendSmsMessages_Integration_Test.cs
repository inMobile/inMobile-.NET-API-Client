using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.SmsOutgoing
{
    public class SendSmsMessages_Integration_Test
    {
        [Fact]
        public void SendSmsMessages_Success_Test()
        {
            var expectedRequestJson = @"{""Messages"":[{""To"":""4511111111"",""Text"":""Hello world"",""From"":""1245"",""MessageId"":""someMessageId"",""RespectBlacklist"":true,""Flash"":false,""Encoding"":""auto"",""ValidityPeriodInSeconds"":55,""SendTime"":""2001-02-03T14:05:06Z""}]}";

            var responseJson = @"{
""results"": [
{
    ""numberDetails"": {
        ""countryCode"": ""45"",
        ""phoneNumber"": ""11111111"",
        ""rawMsisdn"": ""+45 11111111"",
        ""msisdn"": ""4511111111"",
        ""isValidMsisdn"": true,
        ""countryHint"": """",
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
                        messageId: new OutgoingMessageId("someMessageId"),
                        respectBlacklist: true,
                        flash: false,
                        encoding: MessageEncoding.Auto,
                        validityPeriod: TimeSpan.FromSeconds(55),
                        sendTime: new DateTime(2001,02,03,14,05,06, DateTimeKind.Utc))
                });
                Assert.NotNull(response);
                Assert.Single(response.Results);
                var singleResult = response.Results.Single();
                Assert.NotNull(singleResult);
                Assert.Equal("someMessageId", singleResult.MessageId.Value);
                Assert.Equal(MessageEncoding.Ucs2, singleResult.Encoding);
                Assert.Equal("PetShop", singleResult.From);
                Assert.Equal("45", singleResult.NumberDetails.CountryCode);
                Assert.Equal("11111111", singleResult.NumberDetails.PhoneNumber);
                Assert.Equal(true, singleResult.NumberDetails.IsValidMsisdn);
                Assert.Equal("+45 11111111", singleResult.NumberDetails.RawMsisdn);
                Assert.Equal("4511111111", singleResult.NumberDetails.Msisdn);
                Assert.Equal("", singleResult.NumberDetails.CountryHint);
            }
        }

        [Fact]
        public void SendSmsMessages_WithCountryHint_Success_Test()
        {
            var expectedRequestJson = @"{""Messages"":[{""To"":""4511111111"",""CountryHint"":""DK"",""Text"":""Hello world"",""From"":""1245"",""MessageId"":""someMessageId"",""RespectBlacklist"":true,""Flash"":false,""Encoding"":""auto"",""ValidityPeriodInSeconds"":55,""SendTime"":""2001-02-03T14:05:06Z""}]}";

            var responseJson = @"{
""results"": [
{
    ""numberDetails"": {
        ""countryCode"": ""45"",
        ""phoneNumber"": ""11111111"",
        ""rawMsisdn"": ""+45 11111111"",
        ""msisdn"": ""4511111111"",
        ""isValidMsisdn"": true,
        ""isAnonymized"": false,
        ""countryHint"": ""DK"",
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
                        messageId: new OutgoingMessageId("someMessageId"),
                        respectBlacklist: true,
                        flash: false,
                        encoding: MessageEncoding.Auto,
                        validityPeriod: TimeSpan.FromSeconds(55),
                        sendTime: new DateTime(2001,02,03,14,05,06, DateTimeKind.Utc),
                        countryHint: "DK")
                });
                Assert.NotNull(response);
                Assert.Single(response.Results);
                var singleResult = response.Results.Single();
                Assert.NotNull(singleResult);
                Assert.Equal("someMessageId", singleResult.MessageId.Value);
                Assert.Equal(MessageEncoding.Ucs2, singleResult.Encoding);
                Assert.Equal("PetShop", singleResult.From);
                Assert.Equal("45", singleResult.NumberDetails.CountryCode);
                Assert.Equal("11111111", singleResult.NumberDetails.PhoneNumber);
                Assert.True(singleResult.NumberDetails.IsValidMsisdn);
                Assert.False(singleResult.NumberDetails.IsAnonymized);
                Assert.Equal("+45 11111111", singleResult.NumberDetails.RawMsisdn);
                Assert.Equal("4511111111", singleResult.NumberDetails.Msisdn);
                Assert.Equal("DK", singleResult.NumberDetails.CountryHint);
            }
        }

        [Fact]
        public void SendSmsMessages_EnsureNotBreakingOfFutureEncodingsAreReceived_Test()
        {
            var expectedRequestJson = @"{""Messages"":[{""To"":""+45 11111111"",""Text"":""Hello world"",""From"":""1245"",""MessageId"":""someMessageId"",""RespectBlacklist"":true,""Flash"":false,""Encoding"":""auto"",""ValidityPeriodInSeconds"":55}]}";

            var responseJson = @"{
""results"": [
{
    ""numberDetails"": {
        ""countryCode"": ""45"",
        ""phoneNumber"": ""11111111"",
        ""rawMsisdn"": ""+45 11111111"",
        ""msisdn"": ""4511111111"",
        ""isValidMsisdn"": true,
        ""isAnonymized"": false,
        ""countryHint"": """",
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
                        messageId: new OutgoingMessageId("someMessageId"),
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

        [Fact]
        public void SendSmsMessages_ApiError_Test()
        {
            var expectedRequestJson = @"{""Messages"":[{""To"":""4511111111"",""Text"":""Hello world"",""From"":""1245"",""MessageId"":""someMessageId"",""RespectBlacklist"":true,""Flash"":false,""Encoding"":""auto"",""ValidityPeriodInSeconds"":55,""SendTime"":""2001-02-03T14:05:06Z""}]}";

            var responseJson = @"{
""errorMessage"": ""Forbidden thing"",
""details"": [
""You shall not pass"",
""Go away""
]
}";
            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/sms/outgoing", jsonOrNull: expectedRequestJson);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "500 ServerError");
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var ex = Assert.Throws<InMobileApiException>(() => client.SmsOutgoing.SendSmsMessages(new List<OutgoingSmsMessageCreateInfo>() {
                    new OutgoingSmsMessageCreateInfo(
                        to: "4511111111",
                        text: "Hello world",
                        from: "1245",
                        messageId: new OutgoingMessageId("someMessageId"),
                        respectBlacklist: true,
                        flash: false,
                        encoding: MessageEncoding.Auto,
                        validityPeriod: TimeSpan.FromSeconds(55),
                        sendTime: new DateTime(2001,02,03,14,05,06, DateTimeKind.Utc))
                }));
                Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
            }
        }
    }
}

