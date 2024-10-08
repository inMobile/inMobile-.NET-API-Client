﻿using System.Net;
using System.Threading.Tasks;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.Blacklist;

public class DeleteAllRecipientsInList_Integration_Test
{
    [Fact]
    public void DeleteAllRecipientsInList_Test()
    {
        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "DELETE /v4/lists/some_list_id/recipients/all", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: null, statusCodeString: "204 NoContent");
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            client.Lists.DeleteAllRecipientsInList(listId: new RecipientListId("some_list_id"));

            server.AssertNoAwaitingRequestsLeft();
        }
    }
    
    [Fact]
    public async Task DeleteAllRecipientsInListAsync_Test()
    {
        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "DELETE /v4/lists/some_list_id/recipients/all", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: null, statusCodeString: "204 NoContent");
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            await client.Lists.DeleteAllRecipientsInListAsync(listId: new RecipientListId("some_list_id"));

            server.AssertNoAwaitingRequestsLeft();
        }
    }

    [Fact]
    public void DeleteAllRecipientsInList_ApiError_Test()
    {
        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "DELETE /v4/lists/some_list_id/recipients/all", jsonOrNull: null);
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
            var ex = Assert.Throws<InMobileApiException>(() => client.Lists.DeleteAllRecipientsInList(listId: new RecipientListId("some_list_id")));

            Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
        }
    }
    
    [Fact]
    public async Task DeleteAllRecipientsInListAsync_ApiError_Test()
    {
        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "DELETE /v4/lists/some_list_id/recipients/all", jsonOrNull: null);
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
            var ex = await Assert.ThrowsAsync<InMobileApiException>(() => client.Lists.DeleteAllRecipientsInListAsync(listId: new RecipientListId("some_list_id")));

            Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
        }
    }
}