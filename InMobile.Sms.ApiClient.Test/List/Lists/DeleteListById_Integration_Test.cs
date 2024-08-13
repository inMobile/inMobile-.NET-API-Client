﻿using System.Net;
using System.Threading.Tasks;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.Blacklist;

public class DeleteListById_Integration_Test
{
    [Fact]
    public void DeleteListById_Test()
    {
        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "DELETE /v4/lists/some_list_id", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: null, statusCodeString: "204 NoContent");
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            client.Lists.DeleteListById(listId: new RecipientListId("some_list_id"));

            server.AssertNoAwaitingRequestsLeft();
        }
    }
    
    [Fact]
    public async Task DeleteListByIdAsync_Test()
    {
        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "DELETE /v4/lists/some_list_id", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: null, statusCodeString: "204 NoContent");
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            await client.Lists.DeleteListByIdAsync(listId: new RecipientListId("some_list_id"));

            server.AssertNoAwaitingRequestsLeft();
        }
    }

    [Fact]
    public void DeleteListById_ApiError_NotFound_Test()
    {
        var responseJson = @"{
                ""errorMessage"": ""Could not find: ..."",
                ""details"": []
            }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "DELETE /v4/lists/some_list_id", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "404 Not Found");
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var ex = Assert.Throws<InMobileApiException>(() => client.Lists.DeleteListById(listId: new RecipientListId("some_list_id")));

            Assert.Equal(HttpStatusCode.NotFound, ex.ErrorHttpStatusCode);
        }
    }
    
    [Fact]
    public async Task DeleteListByIdAsync_ApiError_NotFound_Test()
    {
        var responseJson = @"{
                ""errorMessage"": ""Could not find: ..."",
                ""details"": []
            }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "DELETE /v4/lists/some_list_id", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "404 Not Found");
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var ex = await Assert.ThrowsAsync<InMobileApiException>(() => client.Lists.DeleteListByIdAsync(listId: new RecipientListId("some_list_id")));

            Assert.Equal(HttpStatusCode.NotFound, ex.ErrorHttpStatusCode);
        }
    }

    [Fact]
    public void DeleteListById_ApiError_InternalServerError_Test()
    {
        var responseJson = @"{
                ""errorMessage"": ""Forbidden thing"",
                ""details"": [
                    ""You shall not pass"",
                    ""Go away""
                ]
            }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "DELETE /v4/lists/some_list_id", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "500 ServerError");
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var ex = Assert.Throws<InMobileApiException>(() => client.Lists.DeleteListById(listId: new RecipientListId("some_list_id")));

            Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
        }
    }
    
    [Fact]
    public async Task DeleteListByIdAsync_ApiError_InternalServerError_Test()
    {
        var responseJson = @"{
                ""errorMessage"": ""Forbidden thing"",
                ""details"": [
                    ""You shall not pass"",
                    ""Go away""
                ]
            }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "DELETE /v4/lists/some_list_id", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "500 ServerError");
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var ex = await Assert.ThrowsAsync<InMobileApiException>(() => client.Lists.DeleteListByIdAsync(listId: new RecipientListId("some_list_id")));

            Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
        }
    }
}