using System.Linq;
using System.Net;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

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
                            ""future_field_not_yet_known"": ""Hello""
                            },
                        ""deliveryInfo"": {
                            ""stateCode"": -1,
                            ""stateDescription"": ""Failed"",
                            ""errorCode"": -1,
                            ""errorDescription"": ""Undeliverable message"",
                            ""future_field_not_yet_known"": ""Hello""
                            },
                        ""chargeInfo"": {
                            ""isCharged"": true,
                            ""smsCount"": 2,
                            ""encoding"": ""gsm7"",
                            ""future_field_not_yet_known"": ""Hello""
                            },
                        ""future_field_not_yet_known"": ""Hello""
                    }
                    ]}";
            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/sms/outgoing/reports", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var response = client.SmsOutgoing.GetStatusReports();
                Assert.NotNull(response);
                Assert.Single(response.Reports);
                var report = response.Reports.Single();
                Assert.NotNull(report);
                Assert.Equal(new OutgoingMessageId("id1"), report.MessageId);

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
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
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
        public void GetReports_ApiError_Test()
        {
            var responseJson = @"{
""errorMessage"": ""Forbidden thing"",
""details"": [
""You shall not pass"",
""Go away""
]
}";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/sms/outgoing/reports", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "500 ServerError");
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var ex = Assert.Throws<InMobileApiException>(() => client.SmsOutgoing.GetStatusReports());

                Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
            }
        }

    }
}

