using System.Net;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.Blacklist
{
    public class DeleteRecipientById_Integration_Test
    {
        [Fact]
        public void DeleteRecipientById_Test()
        {
            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "DELETE /v4/lists/some_list_id/recipients/recId1", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: null, statusCodeString: "204 NoContent");
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                client.Lists.DeleteRecipientById(listId: new RecipientListId("some_list_id"), recipientId: new RecipientId("recId1"));

                server.AssertNoAwaitingRequestsLeft();
            }
        }

        [Fact]
        public void DeleteRecipientById_ApiError_NotFound_Test()
        {
            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "DELETE /v4/lists/some_list_id/recipients/recId1", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: @"{
                ""errorMessage"": ""Could not find: ..."",
                ""details"": []
            }", statusCodeString: "404 Not Found");

            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var ex = Assert.Throws<InMobileApiException>(() => client.Lists.DeleteRecipientById(listId: new RecipientListId("some_list_id"), recipientId: new RecipientId("recId1")));

                Assert.Equal(HttpStatusCode.NotFound, ex.ErrorHttpStatusCode);
            }
        }

        [Fact]
        public void DeleteRecipientById_ApiError_InternalServerError_Test()
        {
            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "DELETE /v4/lists/some_list_id/recipients/recId1", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: @"{
                ""errorMessage"": ""Forbidden thing"",
                ""details"": [
                    ""You shall not pass"",
                    ""Go away""
                ]
            }", statusCodeString: "500 ServerError");

            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var ex = Assert.Throws<InMobileApiException>(() => client.Lists.DeleteRecipientById(listId: new RecipientListId("some_list_id"), recipientId: new RecipientId("recId1")));

                Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
            }
        }
    }
}
