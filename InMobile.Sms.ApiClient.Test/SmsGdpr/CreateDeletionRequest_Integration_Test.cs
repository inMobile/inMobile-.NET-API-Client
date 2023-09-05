using System.Net;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;
using Xunit;

namespace InMobile.Sms.ApiClient.Test.SmsGdpr
{
    public class CreateDeletionRequest_Integration_Test
    {
        [Fact]
        public void CreateDeletionRequest_Test()
        {
            var requestJson = @"{""NumberInfo"":{""CountryCode"":""45"",""PhoneNumber"":""12345678""}}";
            var responseJson = @"{                
                ""id"": ""dd5f73f7-0a83-4301-8d92-53d22b2c59f8""
            }";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/sms/gdpr/deletionrequests", jsonOrNull: requestJson);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var entry = client.SmsGdpr.CreateDeletionRequest(new NumberInfo(countryCode: "45", phoneNumber: "12345678"));

                Assert.Equal("dd5f73f7-0a83-4301-8d92-53d22b2c59f8", entry.Id.Value);
            }
        }

        [Fact]
        public void CreateDeletionRequest_ApiError_Test()
        {
            var requestJson = @"{""NumberInfo"":{""CountryCode"":""45"",""PhoneNumber"":""12345678""}}";
            var responseJson = @"{
                ""errorMessage"": ""Forbidden thing"",
                ""details"": [
                    ""You shall not pass"",
                    ""Go away""
                ]
            }";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/sms/gdpr/deletionrequests", jsonOrNull: requestJson);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "500 ServerError");
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var ex = Assert.Throws<InMobileApiException>(() => client.SmsGdpr.CreateDeletionRequest(new NumberInfo(countryCode: "45", phoneNumber: "12345678")));

                Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
            }
        }
    }
}
