using System;
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
                            ""msisdn"": ""4512345678"",
                            ""isValidMsisdn"": true,
                            ""isAnonymized"": false,
                            ""countryHint"": ""DK"",
                            ""future_field_not_yet_known"": ""Hello""
                            },
                        ""deliveryInfo"": {
                            ""stateCode"": -1,
                            ""stateDescription"": ""Failed"",
                            ""sendTime"": ""2001-02-24T14:50:23Z"",
                            ""doneTime"": ""2001-02-24T16:50:23Z"",
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
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/sms/outgoing/reports?limit=8", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var response = client.SmsOutgoing.GetStatusReports(limit: 8);
                Assert.NotNull(response);
                Assert.Single(response.Reports);
                var report = response.Reports.Single();
                Assert.NotNull(report);
                Assert.Equal(new OutgoingMessageId("id1"), report.MessageId);

                Assert.Equal("45", report.NumberDetails.CountryCode);
                Assert.Equal("12345678", report.NumberDetails.PhoneNumber);
                Assert.Equal("45 12 34 56 78", report.NumberDetails.RawMsisdn);
                Assert.Equal("4512345678", report.NumberDetails.Msisdn);
                Assert.Equal("DK", report.NumberDetails.CountryHint);
                Assert.Equal(true, report.NumberDetails.IsValidMsisdn);
                Assert.Equal(false, report.NumberDetails.IsAnonymized);

                Assert.Equal(MessageStateCode.Failed, report.DeliveryInfo.StateCode);
                Assert.Equal("Failed", report.DeliveryInfo.StateDescription);
                Assert.Equal(new DateTime(2001, 02, 24, 14, 50, 23, DateTimeKind.Utc), report.DeliveryInfo.SendTime);
                Assert.Equal(new DateTime(2001, 02, 24, 16, 50, 23, DateTimeKind.Utc), report.DeliveryInfo.DoneTime);
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
                            ""msisdn"": ""4512345678"",
                            ""isValidMsisdn"": true,
                            ""isAnonymized"": false,
                            ""countryHint"": ""DK"",
                            ""futureFieldToBeIgnored"": false
                            },
                        ""deliveryInfo"": {
                            ""stateCode"": 8,
                            ""stateDescription"": ""FutureState"",
                            ""sendTime"": ""2001-02-24T14:50:23Z"",
                            ""doneTime"": null,
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
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/sms/outgoing/reports?limit=250", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var response = client.SmsOutgoing.GetStatusReports(limit: 250);
                Assert.NotNull(response);
                Assert.Single(response.Reports);
                var report = response.Reports.Single();
                Assert.NotNull(report);
                Assert.Equal(8, (int)report.DeliveryInfo.StateCode);
                Assert.Equal(new DateTime(2001, 02, 24, 14, 50, 23, DateTimeKind.Utc), report.DeliveryInfo.SendTime);
                Assert.Null(report.DeliveryInfo.DoneTime);
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
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/sms/outgoing/reports?limit=1", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "500 ServerError");
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var ex = Assert.Throws<InMobileApiException>(() => client.SmsOutgoing.GetStatusReports(limit: 1));

                Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
            }
        }


        [Theory]
        [InlineData(-1000, false)]
        [InlineData(-1, false)]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(10, true)]
        [InlineData(249, true)]
        [InlineData(250, true)]
        [InlineData(251, false)]
        [InlineData(1000, false)]
        public void GetReports_Limit_Test(int limit, bool expectApiCalled)
        {
            var emptyResponseJson = @"{
                    ""reports"": [
                    ]}";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: $"GET /v4/sms/outgoing/reports?limit={limit}", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: emptyResponseJson);
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                if (expectApiCalled)
                {
                    var reports = client.SmsOutgoing.GetStatusReports(limit: limit);
                    Assert.Empty(reports.Reports);
                }
                else
                {
                    var ex = Assert.Throws<ArgumentException>(() => client.SmsOutgoing.GetStatusReports(limit: limit));
                }
            }
        }

    }
}

