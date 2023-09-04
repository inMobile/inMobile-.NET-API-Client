using System;
using System.Net;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.List
{
    public class GetListById_Integration_Test
    {
        [Fact]
        public void GetListById_Test()
        {
            var responseJson = @"{                
                ""name"": ""Some list name"",
                ""Id"": ""some_list_id"",
                ""created"": ""2001-02-24T14:50:23Z"",
                ""future_field_not_yet_known"": ""Hello""
            }";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/some_list_id", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var entry = client.Lists.GetListById(listId: new RecipientListId("some_list_id"));
                Assert.Equal("Some list name", entry.Name);
                Assert.Equal("some_list_id", entry.Id.Value);
                Assert.Equal(new DateTime(2001, 02, 24, 14, 50, 23, DateTimeKind.Utc), entry.Created);
            }
        }

        [Fact]
        public void GetListById_ApiError_NotFound_Test()
        {
            var responseJson = @"{
                 ""errorMessage"": ""Could not find list: some_list_id"",
                 ""details"": []
             }";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/some_list_id", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "404 Not Found");
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");

                var ex = Assert.Throws<InMobileApiException>(() => client.Lists.GetListById(listId: new RecipientListId("some_list_id")));
                Assert.Equal(HttpStatusCode.NotFound, ex.ErrorHttpStatusCode);
            }
        }

        [Fact]
        public void GetListById_ApiError_InternalServerError_Test()
        {
            var responseJson = @"{
                ""errorMessage"": ""Forbidden thing"",
                ""details"": [
                    ""You shall not pass"",
                    ""Go away""
                ]
            }";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/some_list_id", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "500 ServerError");
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");

                var ex = Assert.Throws<InMobileApiException>(() => client.Lists.GetListById(listId: new RecipientListId("some_list_id")));
                Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
            }
        }
    }
}
