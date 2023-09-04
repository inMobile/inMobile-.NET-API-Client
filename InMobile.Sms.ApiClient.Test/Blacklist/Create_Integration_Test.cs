using System;
using System.Net;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.Blacklist
{
    public class Create_Integration_Test
    {
        [Fact]
        public void Create_Test()
        {
            var requestJson = @"{""NumberInfo"":{""CountryCode"":""45"",""PhoneNumber"":""12345678""},""Comment"":""Sometextprovidedwhencreated""}";
            var responseJson = @"{                
                ""numberInfo"": {
                    ""countryCode"": ""45"",
                    ""phoneNumber"": ""12345678""
                },
                ""comment"": ""Sometextprovidedwhencreated"",
                ""id"": ""some_blacklist_id"",
                ""created"": ""2001-02-24T14:50:23Z""
            }";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/blacklist", jsonOrNull: requestJson);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var entry = client.Blacklist.Create(new BlacklistEntryCreateInfo(new NumberInfo(countryCode: "45", phoneNumber: "12345678"), comment: "Sometextprovidedwhencreated"));
                Assert.Equal("45", entry.NumberInfo.CountryCode);
                Assert.Equal("12345678", entry.NumberInfo.PhoneNumber);
                Assert.Equal("Sometextprovidedwhencreated", entry.Comment);
                Assert.Equal("some_blacklist_id", entry.Id.Value);
                Assert.Equal(new DateTime(2001, 02, 24, 14, 50, 23, DateTimeKind.Utc), entry.Created);
            }
        }

        [Fact]
        public void Create_ApiError_Test()
        {
            var requestJson = @"{""NumberInfo"":{""CountryCode"":""45"",""PhoneNumber"":""12345678""},""Comment"":""Sometextprovidedwhencreated""}";
            var responseJson = @"{
""errorMessage"": ""Forbidden thing"",
""details"": [
""You shall not pass"",
""Go away""
]
}";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/blacklist", jsonOrNull: requestJson);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "500 ServerError");
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var ex = Assert.Throws<InMobileApiException>(() => client.Blacklist.Create(new BlacklistEntryCreateInfo(new NumberInfo(countryCode: "45", phoneNumber: "12345678"), comment: "Sometextprovidedwhencreated")));
                Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
            }
        }
    }
}
