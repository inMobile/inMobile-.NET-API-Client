using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.List.Recipients;

public class GetRecipientById_Integration_Test
{
    [Fact]
    public void GetRecipientById_Test()
    {
        var responseJson = @"{
                                    ""externalCreated"": ""2001-02-10T14:50:23Z"",
                                    ""numberInfo"": {
                                        ""countryCode"": ""45"",
                                        ""phoneNumber"": ""1111""
                                    },
                                    ""fields"": {
                                        ""firstname"": ""Mr"",
                                        ""lastname"": ""Anderson""
                                    },
                                    ""id"": ""recId1"",
                                    ""listId"": ""some_list_id"",
                                    ""created"": ""2002-02-25T14:50:23Z"",
                                    ""future_field_not_yet_known"": ""Hello""
                                }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/some_list_id/recipients/recId1", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var recipient = client.Lists.GetRecipientById(listId: new RecipientListId("some_list_id"), recipientId: new RecipientId("recId1"));
            Assert.Equal("recId1", recipient.Id.Value);
            Assert.True(recipient.ExternalCreated.HasValue);
            Assert.Equal(DateTimeKind.Utc, recipient.ExternalCreated.Value.Kind);
            Assert.Equal(new DateTime(2001, 02, 10, 14, 50, 23, DateTimeKind.Utc), recipient.ExternalCreated.Value);
            Assert.Equal(new DateTime(2002, 02, 25, 14, 50, 23, DateTimeKind.Utc), recipient.Created);
            Assert.Equal("45", recipient.NumberInfo.CountryCode);
            Assert.Equal("1111", recipient.NumberInfo.PhoneNumber);
            Assert.Equal("some_list_id", recipient.ListId.Value);
            Assert.Equal("Mr", recipient.Fields["firstname"]);
            Assert.Equal("Anderson", recipient.Fields["lastname"]);
        }
    }
    
    [Fact]
    public async Task GetRecipientByIdAsync_Test()
    {
        var responseJson = @"{
                                    ""externalCreated"": ""2001-02-10T14:50:23Z"",
                                    ""numberInfo"": {
                                        ""countryCode"": ""45"",
                                        ""phoneNumber"": ""1111""
                                    },
                                    ""fields"": {
                                        ""firstname"": ""Mr"",
                                        ""lastname"": ""Anderson""
                                    },
                                    ""id"": ""recId1"",
                                    ""listId"": ""some_list_id"",
                                    ""created"": ""2002-02-25T14:50:23Z"",
                                    ""future_field_not_yet_known"": ""Hello""
                                }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/some_list_id/recipients/recId1", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var recipient = await client.Lists.GetRecipientByIdAsync(listId: new RecipientListId("some_list_id"), recipientId: new RecipientId("recId1"));
            Assert.Equal("recId1", recipient.Id.Value);
            Assert.True(recipient.ExternalCreated.HasValue);
            Assert.Equal(DateTimeKind.Utc, recipient.ExternalCreated.Value.Kind);
            Assert.Equal(new DateTime(2001, 02, 10, 14, 50, 23, DateTimeKind.Utc), recipient.ExternalCreated.Value);
            Assert.Equal(new DateTime(2002, 02, 25, 14, 50, 23, DateTimeKind.Utc), recipient.Created);
            Assert.Equal("45", recipient.NumberInfo.CountryCode);
            Assert.Equal("1111", recipient.NumberInfo.PhoneNumber);
            Assert.Equal("some_list_id", recipient.ListId.Value);
            Assert.Equal("Mr", recipient.Fields["firstname"]);
            Assert.Equal("Anderson", recipient.Fields["lastname"]);
        }
    }

    [Fact]
    public void GetRecipientById_ApiError_NotFound_Test()
    {
        var responseJson = @"{
                ""errorMessage"": ""Could not find recipient: ..."",
                ""details"": []
            }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/some_list_id/recipients/rec_id", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "404 Not Found");
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");

            var ex = Assert.Throws<InMobileApiException>(() => client.Lists.GetRecipientById(listId: new RecipientListId("some_list_id"), recipientId: new RecipientId("rec_id")));
            Assert.Equal(HttpStatusCode.NotFound, ex.ErrorHttpStatusCode);
        }
    }
    
    [Fact]
    public async Task GetRecipientByIdAsync_ApiError_NotFound_Test()
    {
        var responseJson = @"{
                ""errorMessage"": ""Could not find recipient: ..."",
                ""details"": []
            }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/some_list_id/recipients/rec_id", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "404 Not Found");
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");

            var ex = await Assert.ThrowsAsync<InMobileApiException>(() => client.Lists.GetRecipientByIdAsync(listId: new RecipientListId("some_list_id"), recipientId: new RecipientId("rec_id")));
            Assert.Equal(HttpStatusCode.NotFound, ex.ErrorHttpStatusCode);
        }
    }

    [Fact]
    public void GetRecipientById_ApiError_InternalServerError_Test()
    {
        var responseJson = @"{
                ""errorMessage"": ""Forbidden thing"",
                ""details"": [
                    ""You shall not pass"",
                    ""Go away""
                ]
            }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/some_list_id/recipients/rec_id", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "500 ServerError");
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");

            var ex = Assert.Throws<InMobileApiException>(() => client.Lists.GetRecipientById(listId: new RecipientListId("some_list_id"), recipientId: new RecipientId("rec_id")));
            Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
        }
    }
    
    [Fact]
    public async Task GetRecipientByIdAsync_ApiError_InternalServerError_Test()
    {
        var responseJson = @"{
                ""errorMessage"": ""Forbidden thing"",
                ""details"": [
                    ""You shall not pass"",
                    ""Go away""
                ]
            }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/some_list_id/recipients/rec_id", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "500 ServerError");
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");

            var ex = await Assert.ThrowsAsync<InMobileApiException>(() => client.Lists.GetRecipientByIdAsync(listId: new RecipientListId("some_list_id"), recipientId: new RecipientId("rec_id")));
            Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
        }
    }
}