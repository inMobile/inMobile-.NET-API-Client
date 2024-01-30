using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.SmsOutgoing
{
    public class SendSmsMessagesUsingTemplate_Integration_Test
    {
        [Fact]
        public void SendSmsMessagesUsingTemplate_Success_Test()
        {
            var expectedRequestJson = @"{""TemplateId"":""d33a51b8-13a8-4714-8a96-11347326a4a9"",""Messages"":[{""To"":""+45 11111111"",""CountryHint"":""DK"",""MessageId"":""someMessageId"",""RespectBlacklist"":true,""ValidityPeriodInSeconds"":55,""MsisdnCooldownInMinutes"":120,""SendTime"":""2001-02-03T14:05:06Z"",""Placeholders"":{""{name}"":""Clark Kent""}}]}";

            var responseJson = @"{
  ""usedPlaceholderKeys"": [
    ""{name}""
  ],
  ""notUsedPlaceholderKeys"": [
    ""{lastname}""
  ],
  ""results"": [
    {
      ""numberDetails"": {
        ""countryCode"": ""45"",
        ""phoneNumber"": ""11111111"",
        ""rawMsisdn"": ""+45 11111111"",
        ""msisdn"": ""4511111111"",
        ""isValidMsisdn"": true,
        ""countryHint"": ""DK""
      },
      ""text"": ""This is a message text to be sent"",
      ""from"": ""PetShop"",
      ""smsCount"": 1,
      ""messageId"": ""someMessageId"",
      ""encoding"": ""ucs2""
    }
  ]
}";
            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/sms/outgoing/sendusingtemplate", jsonOrNull: expectedRequestJson);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var response = client.SmsOutgoing.SendSmsMessagesUsingTemplate(new OutgoingSmsTemplateCreateInfo(
                    new SmsTemplateId("d33a51b8-13a8-4714-8a96-11347326a4a9"),
                    new List<OutgoingSmsTemplateMessageCreateInfo>
                    {
                        new OutgoingSmsTemplateMessageCreateInfo(
                            placeholders: new Dictionary<string, string>
                            {
                                { "{name}", "Clark Kent" }
                            },
                            to: "+45 11111111",
                            countryHint: "DK",
                            messageId: new OutgoingMessageId("someMessageId"),
                            respectBlacklist: true,
                            validityPeriod: TimeSpan.FromSeconds(55),
                            msisdnCooldown: TimeSpan.FromMinutes(120),
                            statusCallbackUrl: null,
                            sendTime: new DateTime(2001,02,03,14,05,06, DateTimeKind.Utc))
                    }));

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
                Assert.Equal("+45 11111111", singleResult.NumberDetails.RawMsisdn);
                Assert.Equal("4511111111", singleResult.NumberDetails.Msisdn);
                Assert.Equal("DK", singleResult.NumberDetails.CountryHint);
            }
        }

        [Fact]
        public void SendSmsMessagesUsingTemplate_ApiError_Test()
        {
            var expectedRequestJson = @"{""TemplateId"":""d33a51b8-13a8-4714-8a96-11347326a4a9"",""Messages"":[{""To"":""4511111111"",""CountryHint"":""DK"",""MessageId"":""someMessageId"",""RespectBlacklist"":true,""ValidityPeriodInSeconds"":55,""SendTime"":""2001-02-03T14:05:06Z"",""Placeholders"":{""{name}"":""Clark Kent""}}]}";

            var responseJson = @"{
""errorMessage"": ""Forbidden thing"",
""details"": [
""You shall not pass"",
""Go away""
]
}";
            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/sms/outgoing/sendusingtemplate", jsonOrNull: expectedRequestJson);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "500 ServerError");
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var ex = Assert.Throws<InMobileApiException>(() => client.SmsOutgoing.SendSmsMessagesUsingTemplate(new OutgoingSmsTemplateCreateInfo(
                    new SmsTemplateId("d33a51b8-13a8-4714-8a96-11347326a4a9"),
                    new List<OutgoingSmsTemplateMessageCreateInfo>
                    {
                        new OutgoingSmsTemplateMessageCreateInfo(
                            placeholders: new Dictionary<string, string>
                            {
                                { "{name}", "Clark Kent" }
                            },
                            to: "4511111111",
                            countryHint: "DK",
                            messageId: new OutgoingMessageId("someMessageId"),
                            respectBlacklist: true,
                            validityPeriod: TimeSpan.FromSeconds(55),
                            statusCallbackUrl: null,
                            sendTime: new DateTime(2001,02,03,14,05,06, DateTimeKind.Utc))
                    })));

                Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
            }
        }
    }
}
