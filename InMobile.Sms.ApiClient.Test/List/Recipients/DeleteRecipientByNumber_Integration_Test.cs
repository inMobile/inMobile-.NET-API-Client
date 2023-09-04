using System.Net;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.Blacklist
{
    public class DeleteRecipientByNumber_Integration_Test
    {
        [Fact]
        public void DeleteRecipientByNumber_Test()
        {
            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "DELETE /v4/lists/some_list_id/recipients/bynumber?countryCode=47&phoneNumber=11223344", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: null, statusCodeString: "204 NoContent");
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                client.Lists.DeleteRecipientByNumber(listId: new RecipientListId("some_list_id"), numberInfo: new NumberInfo(countryCode: "47", phoneNumber: "11223344"));

                server.AssertNoAwaitingRequestsLeft();
            }
        }

        [Fact]
        public void DeleteRecipientByNumber_ApiError_NotFound_Test()
        {
            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "DELETE /v4/lists/some_list_id/recipients/bynumber?countryCode=47&phoneNumber=11223344", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: @"{
                ""errorMessage"": ""Could not find: ..."",
                ""details"": []
            }", statusCodeString: "404 Not Found");

            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var ex = Assert.Throws<InMobileApiException>(() => client.Lists.DeleteRecipientByNumber(listId: new RecipientListId("some_list_id"), numberInfo: new NumberInfo(countryCode: "47", phoneNumber: "11223344")));

                Assert.Equal(HttpStatusCode.NotFound, ex.ErrorHttpStatusCode);
            }
        }

        [Fact]
        public void DeleteRecipientByNumber_ApiError_InternalServerError_Test()
        {
            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "DELETE /v4/lists/some_list_id/recipients/bynumber?countryCode=47&phoneNumber=11223344", jsonOrNull: null);
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
                var ex = Assert.Throws<InMobileApiException>(() => client.Lists.DeleteRecipientByNumber(listId: new RecipientListId("some_list_id"), numberInfo: new NumberInfo(countryCode: "47", phoneNumber: "11223344")));

                Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
            }
        }
    }
}
