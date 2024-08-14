using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.Lists;

public class GetAllLists_Integration_Test
{
    [Fact]
    public void GetAllLists_EmptyResult_Test()
    {
        var responseJson = @"{
                ""entries"": [],
                ""_links"": {
                    ""next"": null,
                    ""isLastPage"": true
                }
            }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists?pageLimit=250", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var allEntries = client.Lists.GetAllLists();
            Assert.Empty(allEntries);
        }
    }

    [Fact]
    public async Task GetAllListsAsync_EmptyResult_Test()
    {
        var responseJson = @"{
                ""entries"": [],
                ""_links"": {
                    ""next"": null,
                    ""isLastPage"": true
                }
            }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists?pageLimit=250", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var allEntries = await client.Lists.GetAllListsAsync();
            Assert.Empty(allEntries);
        }
    }

    [Fact]
    public void GetAllLists_SinglePage_Test()
    {
        var responseJson = @"{
                ""entries"": [
                    {
                        ""id"":""id1"",
                        ""name"": ""name1"",
                        ""created"": ""2001-02-24T14:50:23Z""
                    },
                    {
                        ""id"":""id2"",
                        ""name"": ""name2"",
                        ""created"": ""2002-03-25T16:50:23Z""
                    }
                ],
                ""_links"": {
                    ""next"": null,
                    ""isLastPage"": true
                }
            }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists?pageLimit=250", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var allEntries = client.Lists.GetAllLists();
            Assert.Equal(2, allEntries.Count);
            {
                var entry1 = allEntries[0];
                Assert.Equal("id1", entry1.Id.Value);
                Assert.Equal("name1", entry1.Name);
                Assert.Equal(new DateTime(2001, 02, 24, 14, 50, 23, DateTimeKind.Utc), entry1.Created);
            }

            {
                var entry2 = allEntries[1];
                Assert.Equal("id2", entry2.Id.Value);
                Assert.Equal("name2", entry2.Name);
                Assert.Equal(new DateTime(2002, 03, 25, 16, 50, 23, DateTimeKind.Utc), entry2.Created);
            }
        }
    }

    [Fact]
    public async Task GetAllListsAsync_SinglePage_Test()
    {
        var responseJson = @"{
                ""entries"": [
                    {
                        ""id"":""id1"",
                        ""name"": ""name1"",
                        ""created"": ""2001-02-24T14:50:23Z""
                    },
                    {
                        ""id"":""id2"",
                        ""name"": ""name2"",
                        ""created"": ""2002-03-25T16:50:23Z""
                    }
                ],
                ""_links"": {
                    ""next"": null,
                    ""isLastPage"": true
                }
            }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists?pageLimit=250", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var allEntries = await client.Lists.GetAllListsAsync();
            Assert.Equal(2, allEntries.Count);
            {
                var entry1 = allEntries[0];
                Assert.Equal("id1", entry1.Id.Value);
                Assert.Equal("name1", entry1.Name);
                Assert.Equal(new DateTime(2001, 02, 24, 14, 50, 23, DateTimeKind.Utc), entry1.Created);
            }

            {
                var entry2 = allEntries[1];
                Assert.Equal("id2", entry2.Id.Value);
                Assert.Equal("name2", entry2.Name);
                Assert.Equal(new DateTime(2002, 03, 25, 16, 50, 23, DateTimeKind.Utc), entry2.Created);
            }
        }
    }

    [Fact]
    public void GetAllLists_MultiPage_Test()
    {
        var apiKey = new InMobileApiKey("UnitTestKey123");
        var pair1 = new RequestResponsePair(new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists?pageLimit=250", jsonOrNull: null),
            new UnitTestResponseInfo(@"{
                            ""entries"": [
                                {
                                    ""id"":""id1"",
                                    ""name"": ""name1"",
                                    ""created"": ""2001-02-24T14:50:23Z""
                                },
                                {
                                    ""id"":""id2"",
                                    ""name"": ""name2"",
                                    ""created"": ""2002-03-25T16:50:23Z""
                                }
                            ],
                            ""_links"": {
                                ""next"": ""/v4/lists/page/token_page_2"",
                                ""isLastPage"": false
                            }
                        }"));

        // Testing an empty result in the middle of the flow
        var pair2 = new RequestResponsePair(new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/page/token_page_2", jsonOrNull: null),
            new UnitTestResponseInfo(@"{
                                ""entries"": [
                                ],
                                ""_links"": {
                                    ""next"": ""/v4/lists/page/token_page_3"",
                                    ""isLastPage"": false
                                }
                            }"));

        var pair3 = new RequestResponsePair(new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/page/token_page_3", jsonOrNull: null),
            new UnitTestResponseInfo(@"{
                ""entries"": [
                    {
                        ""id"":""id3"",
                        ""name"": ""name3"",
                        ""created"": ""2003-04-26T16:50:23Z""
                    },
                    {
                        ""id"":""id4"",
                        ""name"": ""name4"",
                        ""created"": ""2004-05-27T16:50:23Z""
                    }
                ],
                ""_links"": {
                    ""next"": null,
                    ""isLastPage"": true
                }
            }"));

        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(pair1, pair2, pair3))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var allEntries = client.Lists.GetAllLists();
            Assert.Equal(4, allEntries.Count);
            {
                var entry1 = allEntries[0];
                Assert.Equal("id1", entry1.Id.Value);
                Assert.Equal("name1", entry1.Name);
                Assert.Equal(new DateTime(2001, 02, 24, 14, 50, 23, DateTimeKind.Utc), entry1.Created);
            }

            {
                var entry2 = allEntries[1];
                Assert.Equal("id2", entry2.Id.Value);
                Assert.Equal("name2", entry2.Name);
                Assert.Equal(new DateTime(2002, 03, 25, 16, 50, 23, DateTimeKind.Utc), entry2.Created);
            }

            {
                var entry3 = allEntries[2];
                Assert.Equal("id3", entry3.Id.Value);
                Assert.Equal("name3", entry3.Name);
                Assert.Equal(new DateTime(2003, 04, 26, 16, 50, 23, DateTimeKind.Utc), entry3.Created);
            }

            {
                var entry4 = allEntries[3];
                Assert.Equal("id4", entry4.Id.Value);
                Assert.Equal("name4", entry4.Name);
                Assert.Equal(new DateTime(2004, 05, 27, 16, 50, 23, DateTimeKind.Utc), entry4.Created);
            }
        }
    }

    [Fact]
    public async Task GetAllListsAsync_MultiPage_Test()
    {
        var apiKey = new InMobileApiKey("UnitTestKey123");
        var pair1 = new RequestResponsePair(new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists?pageLimit=250", jsonOrNull: null),
            new UnitTestResponseInfo(@"{
                            ""entries"": [
                                {
                                    ""id"":""id1"",
                                    ""name"": ""name1"",
                                    ""created"": ""2001-02-24T14:50:23Z""
                                },
                                {
                                    ""id"":""id2"",
                                    ""name"": ""name2"",
                                    ""created"": ""2002-03-25T16:50:23Z""
                                }
                            ],
                            ""_links"": {
                                ""next"": ""/v4/lists/page/token_page_2"",
                                ""isLastPage"": false
                            }
                        }"));

        // Testing an empty result in the middle of the flow
        var pair2 = new RequestResponsePair(new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/page/token_page_2", jsonOrNull: null),
            new UnitTestResponseInfo(@"{
                                ""entries"": [
                                ],
                                ""_links"": {
                                    ""next"": ""/v4/lists/page/token_page_3"",
                                    ""isLastPage"": false
                                }
                            }"));

        var pair3 = new RequestResponsePair(new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/page/token_page_3", jsonOrNull: null),
            new UnitTestResponseInfo(@"{
                ""entries"": [
                    {
                        ""id"":""id3"",
                        ""name"": ""name3"",
                        ""created"": ""2003-04-26T16:50:23Z""
                    },
                    {
                        ""id"":""id4"",
                        ""name"": ""name4"",
                        ""created"": ""2004-05-27T16:50:23Z""
                    }
                ],
                ""_links"": {
                    ""next"": null,
                    ""isLastPage"": true
                }
            }"));

        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(pair1, pair2, pair3))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var allEntries = await client.Lists.GetAllListsAsync();
            Assert.Equal(4, allEntries.Count);
            {
                var entry1 = allEntries[0];
                Assert.Equal("id1", entry1.Id.Value);
                Assert.Equal("name1", entry1.Name);
                Assert.Equal(new DateTime(2001, 02, 24, 14, 50, 23, DateTimeKind.Utc), entry1.Created);
            }

            {
                var entry2 = allEntries[1];
                Assert.Equal("id2", entry2.Id.Value);
                Assert.Equal("name2", entry2.Name);
                Assert.Equal(new DateTime(2002, 03, 25, 16, 50, 23, DateTimeKind.Utc), entry2.Created);
            }

            {
                var entry3 = allEntries[2];
                Assert.Equal("id3", entry3.Id.Value);
                Assert.Equal("name3", entry3.Name);
                Assert.Equal(new DateTime(2003, 04, 26, 16, 50, 23, DateTimeKind.Utc), entry3.Created);
            }

            {
                var entry4 = allEntries[3];
                Assert.Equal("id4", entry4.Id.Value);
                Assert.Equal("name4", entry4.Name);
                Assert.Equal(new DateTime(2004, 05, 27, 16, 50, 23, DateTimeKind.Utc), entry4.Created);
            }
        }
    }

    [Fact]
    public void GetAllLists_ApiError_FirstPage_Test()
    {
        var responseJson = @"{
""errorMessage"": ""Forbidden thing"",
""details"": [
""You shall not pass"",
""Go away""
]
}";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists?pageLimit=250", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "500 ServerError");
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var ex = Assert.Throws<InMobileApiException>(() => client.Lists.GetAllLists());

            Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
        }
    }

    [Fact]
    public async Task GetAllListsAsync_ApiError_FirstPage_Test()
    {
        var responseJson = @"{
""errorMessage"": ""Forbidden thing"",
""details"": [
""You shall not pass"",
""Go away""
]
}";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists?pageLimit=250", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "500 ServerError");
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var ex = await Assert.ThrowsAsync<InMobileApiException>(() => client.Lists.GetAllListsAsync());

            Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
        }
    }

    [Fact]
    public void GetAllLists_ApiError_MultiPage_Test()
    {
        var apiKey = new InMobileApiKey("UnitTestKey123");
        var pair1 = new RequestResponsePair(new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists?pageLimit=250", jsonOrNull: null),
            new UnitTestResponseInfo(@"{
                            ""entries"": [
                                {
                                ""id"":""id1"",
                                ""name"": ""name1""
                            },
                            {
                                ""id"":""id2"",
                                ""name"": ""name2""
                            }
                            ],
                            ""_links"": {
                                ""next"": ""/v4/lists/page/token_page_2"",
                                ""isLastPage"": false
                            }
                        }"));

        // Testing an empty result in the middle of the flow
        var pair2 = new RequestResponsePair(new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/page/token_page_2", jsonOrNull: null),
            new UnitTestResponseInfo(@"{
                                ""entries"": [
                                ],
                                ""_links"": {
                                    ""next"": ""/v4/lists/page/token_page_3"",
                                    ""isLastPage"": false
                                }
                            }"));

        var pair3 = new RequestResponsePair(new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/page/token_page_3", jsonOrNull: null),
            new UnitTestResponseInfo(@"{
""errorMessage"": ""Forbidden thing"",
""details"": [
""You shall not pass"",
""Go away""
]
}", statusCodeString: "500 ServerError"));

        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(pair1, pair2, pair3))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var ex = Assert.Throws<InMobileApiException>(() => client.Lists.GetAllLists());

            Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
        }
    }

    [Fact]
    public async Task GetAllListsAsync_ApiError_MultiPage_Test()
    {
        var apiKey = new InMobileApiKey("UnitTestKey123");
        var pair1 = new RequestResponsePair(new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists?pageLimit=250", jsonOrNull: null),
            new UnitTestResponseInfo(@"{
                            ""entries"": [
                                {
                                ""id"":""id1"",
                                ""name"": ""name1""
                            },
                            {
                                ""id"":""id2"",
                                ""name"": ""name2""
                            }
                            ],
                            ""_links"": {
                                ""next"": ""/v4/lists/page/token_page_2"",
                                ""isLastPage"": false
                            }
                        }"));

        // Testing an empty result in the middle of the flow
        var pair2 = new RequestResponsePair(new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/page/token_page_2", jsonOrNull: null),
            new UnitTestResponseInfo(@"{
                                ""entries"": [
                                ],
                                ""_links"": {
                                    ""next"": ""/v4/lists/page/token_page_3"",
                                    ""isLastPage"": false
                                }
                            }"));

        var pair3 = new RequestResponsePair(new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/page/token_page_3", jsonOrNull: null),
            new UnitTestResponseInfo(@"{
""errorMessage"": ""Forbidden thing"",
""details"": [
""You shall not pass"",
""Go away""
]
}", statusCodeString: "500 ServerError"));

        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(pair1, pair2, pair3))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var ex = await Assert.ThrowsAsync<InMobileApiException>(() => client.Lists.GetAllListsAsync());

            Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
        }
    }
}