using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.Blacklist;

public class CreateList_Integration_Test
{
    [Fact]
    public void CreateList_Test()
    {
        var requestJson = @"{""Name"":""New list name""}";
        var responseJson = @"{                
                ""id"": ""SomeId123"",
                ""name"": ""New list name"",
                ""created"": ""2001-02-24T14:50:23Z""
            }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/lists", jsonOrNull: requestJson);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var entry = client.Lists.CreateList(new RecipientListCreateInfo(name: "New list name"));
            Assert.Equal("New list name", entry.Name);
            Assert.Equal("SomeId123", entry.Id.Value);
            Assert.Equal(new DateTime(2001, 02, 24, 14, 50, 23, DateTimeKind.Utc), entry.Created);
        }
    }

    [Fact]
    public async Task CreateListAsync_Test()
    {
        var requestJson = @"{""Name"":""New list name""}";
        var responseJson = @"{                
                ""id"": ""SomeId123"",
                ""name"": ""New list name"",
                ""created"": ""2001-02-24T14:50:23Z""
            }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/lists", jsonOrNull: requestJson);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var entry = await client.Lists.CreateListAsync(new RecipientListCreateInfo(name: "New list name"));
            Assert.Equal("New list name", entry.Name);
            Assert.Equal("SomeId123", entry.Id.Value);
            Assert.Equal(new DateTime(2001, 02, 24, 14, 50, 23, DateTimeKind.Utc), entry.Created);
        }
    }

    [Fact]
    public void CreateList_ApiError_Test()
    {
        var requestJson = @"{""Name"":""New list name""}";
        var responseJson = @"{
""errorMessage"": ""Forbidden thing"",
""details"": [
""You shall not pass"",
""Go away""
]
}";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/lists", jsonOrNull: requestJson);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "500 ServerError");
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var ex = Assert.Throws<InMobileApiException>(() => client.Lists.CreateList(new RecipientListCreateInfo(name: "New list name")));

            Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
        }
    }

    [Fact]
    public async Task CreateListAsync_ApiError_Test()
    {
        var requestJson = @"{""Name"":""New list name""}";
        var responseJson = @"{
""errorMessage"": ""Forbidden thing"",
""details"": [
""You shall not pass"",
""Go away""
]
}";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/lists", jsonOrNull: requestJson);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "500 ServerError");
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var ex = await Assert.ThrowsAsync<InMobileApiException>(() => client.Lists.CreateListAsync(new RecipientListCreateInfo(name: "New list name")));

            Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
        }
    }
}