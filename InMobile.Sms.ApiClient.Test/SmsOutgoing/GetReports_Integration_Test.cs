using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace InMobile.Sms.ApiClient.Test.SmsOutgoing
{
    public class GetReports_Integration_Test
    {
        [Fact]
        public void GetReports_Success_Test()
        {
            var responseJson = @"{
                    ""reports"": [
                    {
                        ""messageId"": ""id1"",
                        ""numberDetails"": {
                            ""countryCode"": ""45"",
                            ""phoneNumber"": ""12345678"",
                            ""rawMsisdn"": ""45 12 34 56 78"",
                            ""isValidMsisdn"": true,
                            ""isAnonymized"": false,
                            ""futureFieldToBeIgnored"": false
                            },
                        ""deliveryInfo"": {
                            ""stateCode"": -1,
                            ""stateDescription"": ""Failed"",
                            ""errorCode"": -1,
                            ""errorDescription"": ""Undeliverable message"",
                            ""futureFieldToBeIgnored"": false
                            },
                        ""chargeInfo"": {
                            ""isCharged"": true,
                            ""smsCount"": 2,
                            ""encoding"": ""gsm7"",
                            ""futureFieldToBeIgnored"": false
                            }
                        , ""futureFieldToBeIgnored"": false
                    }
                    ]}";
            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/sms/outgoing/reports", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(expectedRequest: expectedRequest, response: responseToSendback))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var response = client.SmsOutgoing.GetStatusReports();
                Assert.NotNull(response);
                Assert.Single(response.Reports);
                var report = response.Reports.Single();
                Assert.NotNull(report);
                Assert.Equal("id1", report.MessageId);

                Assert.Equal("45", report.NumberDetails.CountryCode);
                Assert.Equal("12345678", report.NumberDetails.PhoneNumber);
                Assert.Equal("45 12 34 56 78", report.NumberDetails.RawMsisdn);
                Assert.Equal(true, report.NumberDetails.IsValidMsisdn);
                Assert.Equal(false, report.NumberDetails.IsAnonymized);

                Assert.Equal(MessageStateCode.Failed, report.DeliveryInfo.StateCode);
                Assert.Equal("Failed", report.DeliveryInfo.StateDescription);
                Assert.Equal(-1, report.DeliveryInfo.ErrorCode);
                Assert.Equal("Undeliverable message", report.DeliveryInfo.ErrorDescription);

                Assert.Equal(MessageEncoding.Gsm7, report.ChargeInfo.Encoding);
                Assert.Equal(true, report.ChargeInfo.IsCharged);
                Assert.Equal(null, report.ChargeInfo.Network);
                Assert.Equal(2, report.ChargeInfo.SmsCount);
            }
        }

        [Fact]
        public void GetReports_WithDifferentData_Test()
        {
            var responseJson = @"{
                    ""reports"": [
                    {
                        ""messageId"": ""id1"",
                        ""numberDetails"": {
                            ""countryCode"": ""45"",
                            ""phoneNumber"": ""12345678"",
                            ""rawMsisdn"": ""45 12 34 56 78"",
                            ""isValidMsisdn"": true,
                            ""isAnonymized"": false,
                            ""futureFieldToBeIgnored"": false
                            },
                        ""deliveryInfo"": {
                            ""stateCode"": 8,
                            ""stateDescription"": ""FutureState"",
                            ""futureFieldToBeIgnored"": false
                            },
                        ""chargeInfo"": {
                            ""isCharged"": true,
                            ""smsCount"": 2,
                            ""encoding"": ""futureValue"",
                            ""futureFieldToBeIgnored"": false,
                            ""network"": ""45-TDC""
                            }
                        , ""futureFieldToBeIgnored"": false
                    }
                    ]}";
            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/sms/outgoing/reports", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(expectedRequest: expectedRequest, response: responseToSendback))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var response = client.SmsOutgoing.GetStatusReports();
                Assert.NotNull(response);
                Assert.Single(response.Reports);
                var report = response.Reports.Single();
                Assert.NotNull(report);
                Assert.Equal(8, (int)report.DeliveryInfo.StateCode);
                Assert.Equal(null, report.DeliveryInfo.ErrorCode);
                Assert.Equal(null, report.DeliveryInfo.ErrorDescription);
                Assert.Equal(MessageEncoding.Unknown, report.ChargeInfo.Encoding);
                Assert.Equal("45-TDC", report.ChargeInfo.Network);
            }
        }

        [Fact]
        public void SendSmsMessages_EnsureNotBreakingOfFutureEncodingsAreReceived_Test()
        {
            var expectedRequestJson = @"{""Messages"":[{""To"":""+45 11111111"",""Text"":""Hello world"",""From"":""PetShop"",""MessageId"":""someMessageId"",""RespectBlacklist"":true,""Flash"":false,""encoding"":""auto"",""ValidityPeriodInSeconds"":55}],""StatusCallback"":{""url"":null}}";

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
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(expectedRequest: expectedRequest, response: responseToSendback))
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

