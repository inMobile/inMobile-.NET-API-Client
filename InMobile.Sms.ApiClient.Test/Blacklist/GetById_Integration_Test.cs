using System;
using System.Net;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.Blacklist
{
    public class GetById_Integration_Test
    {
        [Fact]
        public void GetById_Test()
        {
            var responseJson = @"{                
                ""numberInfo"": {
                    ""countryCode"": ""45"",
                    ""phoneNumber"": ""12345678""
                },
                ""comment"": ""Some text provided when created"",
                ""id"": ""some_blacklist_id"",
                ""created"": ""2001-02-24T14:50:23Z"",
                ""future_field_not_yet_known"": ""Hello""
            }";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/blacklist/some_blacklist_id", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var entry = client.Blacklist.GetById(blacklistEntryId: new BlacklistEntryId("some_blacklist_id"));
                Assert.Equal("45", entry.NumberInfo.CountryCode);
                Assert.Equal("12345678", entry.NumberInfo.PhoneNumber);
                Assert.Equal("Some text provided when created", entry.Comment);
                Assert.Equal("some_blacklist_id", entry.Id.Value);
                Assert.Equal(new DateTime(2001, 02, 24, 14, 50, 23, DateTimeKind.Utc), entry.Created);
            }
        }

        [Fact]
        public void GetById_ApiError_NotFound_Test()
        {
            var responseJson = @"{
                ""errorMessage"": ""Could not find: ..."",
                ""details"": []
            }";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/blacklist/some_blacklist_id", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "404 Not Found");
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");

                var ex = Assert.Throws<InMobileApiException>(() => client.Blacklist.GetById(new BlacklistEntryId("some_blacklist_id")));
                Assert.Equal(HttpStatusCode.NotFound, ex.ErrorHttpStatusCode);
            }
        }

        [Fact]
        public void GetById_ApiError_InternalServerError_Test()
        {
            var responseJson = @"{
                ""errorMessage"": ""Forbidden thing"",
                ""details"": [
                    ""You shall not pass"",
                    ""Go away""
                ]
            }";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/blacklist/some_blacklist_id", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "500 ServerError");
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");

                var ex = Assert.Throws<InMobileApiException>(() => client.Blacklist.GetById(new BlacklistEntryId("some_blacklist_id")));
                Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
            }
        }
    }
}
