using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.Blacklist
{
    public class GetByNumber_Integration_Test
    {
        [Fact]
        public void GetByNumber_Test()
        {
            var responseJson = @"{                
                ""numberInfo"": {
                    ""countryCode"": ""45"",
                    ""phoneNumber"": ""12345678""
                },
                ""comment"": ""Some text provided when created"",
                ""id"": ""some_blacklist_id""
            }";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/blacklist/bynumber?countryCode=47&phoneNumber=11223344", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var entry = client.Blacklist.GetByNumber(new NumberInfo(countryCode: "47", phoneNumber: "11223344"));
                Assert.Equal("45", entry.NumberInfo.CountryCode);
                Assert.Equal("12345678", entry.NumberInfo.PhoneNumber);
                Assert.Equal("Some text provided when created", entry.Comment);
                Assert.Equal("some_blacklist_id", entry.Id.Value);
            }
        }

        [Fact]
        public void GetByNumber_ApiError_Test()
        {
            var responseJson = @"{
""errorMessage"": ""Forbidden thing"",
""details"": [
""You shall not pass"",
""Go away""
]
}";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/blacklist/bynumber?countryCode=47&phoneNumber=11223344", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "500 ServerError");
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var ex = Assert.Throws<InMobileApiException>(() => client.Blacklist.GetByNumber(new NumberInfo(countryCode: "47", phoneNumber: "11223344")));

                Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
            }
        }
    }
}
