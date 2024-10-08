﻿using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.List.Recipients;

public class GetAllRecipients_Integration_Test
{
    [Fact]
    public void GetAllRecipients_EmptyResult_Test()
    {
        var responseJson = @"{
                ""entries"": [],
                ""_links"": {
                    ""next"": null,
                    ""isLastPage"": true
                }
            }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/some_list_id/recipients?pageLimit=250", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var allEntries = client.Lists.GetAllRecipientsInList(listId: new RecipientListId("some_list_id"));
            Assert.Empty(allEntries);
        }
    }
    
    [Fact]
    public async Task GetAllRecipientsAsync_EmptyResult_Test()
    {
        var responseJson = @"{
                ""entries"": [],
                ""_links"": {
                    ""next"": null,
                    ""isLastPage"": true
                }
            }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/some_list_id/recipients?pageLimit=250", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var allEntries = await client.Lists.GetAllRecipientsInListAsync(listId: new RecipientListId("some_list_id"));
            Assert.Empty(allEntries);
        }
    }

    [Fact]
    public void GetAllRecipients_SinglePage_Test()
    {
        var responseJson = @"{
                ""entries"": [
                    {
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
                        ""listId"": ""some_list_id""
                    },
                    {
                        ""numberInfo"": {
                            ""countryCode"": ""33"",
                            ""phoneNumber"": ""2222""
                        },
                        ""fields"": {
                            ""firstname"": ""Mrs"",
                            ""lastname"": ""Doubtfire""
                        },
                        ""id"": ""recId2"",
                        ""listId"": ""some_list_id"",
                        ""created"": ""2002-02-25T14:50:23Z""
                    }
                ],
                ""_links"": {
                    ""next"": null,
                    ""isLastPage"": true
                }
            }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/some_LIST_id/recipients?pageLimit=250", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var allEntries = client.Lists.GetAllRecipientsInList(listId: new RecipientListId("some_LIST_id"));
            Assert.Equal(2, allEntries.Count);

            {
                var entry1 = allEntries[0];
                Assert.Equal("recId1", entry1.Id.Value);
                Assert.True(entry1.ExternalCreated.HasValue);
                Assert.Equal(DateTimeKind.Utc, entry1.ExternalCreated.Value.Kind);
                Assert.Equal(new DateTime(2001, 02, 10, 14, 50, 23, DateTimeKind.Utc), entry1.ExternalCreated.Value);
                Assert.Equal("45", entry1.NumberInfo.CountryCode);
                Assert.Equal("1111", entry1.NumberInfo.PhoneNumber);
                Assert.Equal("some_list_id", entry1.ListId.Value);
                Assert.Equal("Mr", entry1.Fields["firstname"]);
                Assert.Equal("Anderson", entry1.Fields["lastname"]);
            }

            Assert.Equal("recId2", allEntries[1].Id.Value);
        }
    }
    
    [Fact]
    public async Task GetAllRecipientsAsync_SinglePage_Test()
    {
        var responseJson = @"{
                ""entries"": [
                    {
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
                        ""listId"": ""some_list_id""
                    },
                    {
                        ""numberInfo"": {
                            ""countryCode"": ""33"",
                            ""phoneNumber"": ""2222""
                        },
                        ""fields"": {
                            ""firstname"": ""Mrs"",
                            ""lastname"": ""Doubtfire""
                        },
                        ""id"": ""recId2"",
                        ""listId"": ""some_list_id"",
                        ""created"": ""2002-02-25T14:50:23Z""
                    }
                ],
                ""_links"": {
                    ""next"": null,
                    ""isLastPage"": true
                }
            }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/some_LIST_id/recipients?pageLimit=250", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var allEntries = await client.Lists.GetAllRecipientsInListAsync(listId: new RecipientListId("some_LIST_id"));
            Assert.Equal(2, allEntries.Count);

            {
                var entry1 = allEntries[0];
                Assert.Equal("recId1", entry1.Id.Value);
                Assert.True(entry1.ExternalCreated.HasValue);
                Assert.Equal(DateTimeKind.Utc, entry1.ExternalCreated.Value.Kind);
                Assert.Equal(new DateTime(2001, 02, 10, 14, 50, 23, DateTimeKind.Utc), entry1.ExternalCreated.Value);
                Assert.Equal("45", entry1.NumberInfo.CountryCode);
                Assert.Equal("1111", entry1.NumberInfo.PhoneNumber);
                Assert.Equal("some_list_id", entry1.ListId.Value);
                Assert.Equal("Mr", entry1.Fields["firstname"]);
                Assert.Equal("Anderson", entry1.Fields["lastname"]);
            }

            Assert.Equal("recId2", allEntries[1].Id.Value);
        }
    }

    [Fact]
    public void GetAllRecipients_MultiPage_Test()
    {
        var apiKey = new InMobileApiKey("UnitTestKey123");
        var pair1 = new RequestResponsePair(
            new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/some_LIST_id/recipients?pageLimit=250", jsonOrNull: null),
            new UnitTestResponseInfo(@"{
                    ""entries"": [
                        {
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
                            ""created"": ""2003-02-25T14:50:23Z""
                        },
                        {
                            ""externalCreated"": null,
                            ""numberInfo"": {
                                ""countryCode"": ""33"",
                                ""phoneNumber"": ""2222""
                            },
                            ""fields"": {
                                ""firstname"": ""Mrs"",
                                ""lastname"": ""Doubtfire""
                            },
                            ""id"": ""recId2"",
                            ""listId"": ""some_list_id"",
                            ""created"": ""2002-02-25T14:50:23Z""
                        }
                    ],
                    ""_links"": {
                        ""next"": ""/v4/lists/page/token_page_2"",
                        ""isLastPage"": false
                    }
                }"));

        // Testing an empty result in the middle of the flow
        var pair2 = new RequestResponsePair(
            new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/page/token_page_2", jsonOrNull: null),
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
                        ""externalCreated"": null,
                        ""numberInfo"": {
                            ""countryCode"": ""45"",
                            ""phoneNumber"": ""4444""
                        },
                        ""fields"": {
                            ""firstname"": ""Mr"",
                            ""lastname"": ""Anderson""
                        },
                        ""id"": ""recId3"",
                        ""listId"": ""some_list_id"",
                        ""created"": ""2002-02-25T14:50:23Z""
                    },
                    {
                        ""externalCreated"": null,
                        ""numberInfo"": {
                            ""countryCode"": ""33"",
                            ""phoneNumber"": ""5555""
                        },
                        ""fields"": {
                            ""firstname"": ""Mrs"",
                            ""lastname"": ""Doubtfire""
                        },
                        ""id"": ""recId4"",
                        ""listId"": ""some_list_id"",
                        ""created"": ""2003-02-25T14:50:23Z""
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
            var allEntries = client.Lists.GetAllRecipientsInList(listId: new RecipientListId("some_LIST_id"));
            Assert.Equal(4, allEntries.Count);

            {
                var entry1 = allEntries[0];
                Assert.Equal("recId1", entry1.Id.Value);
                Assert.True(entry1.ExternalCreated.HasValue);
                Assert.Equal(DateTimeKind.Utc, entry1.ExternalCreated.Value.Kind);
                Assert.Equal(new DateTime(2001, 02, 10, 14, 50, 23, DateTimeKind.Utc), entry1.ExternalCreated.Value);
                Assert.Equal(new DateTime(2003, 02, 25, 14, 50, 23, DateTimeKind.Utc), entry1.Created);
                Assert.Equal("45", entry1.NumberInfo.CountryCode);
                Assert.Equal("1111", entry1.NumberInfo.PhoneNumber);
                Assert.Equal("some_list_id", entry1.ListId.Value);
                Assert.Equal("Mr", entry1.Fields["firstname"]);
                Assert.Equal("Anderson", entry1.Fields["lastname"]);
            }

            Assert.Equal("recId2", allEntries[1].Id.Value);
            Assert.Equal("recId3", allEntries[2].Id.Value);
            Assert.Equal("recId4", allEntries[3].Id.Value);
        }
    }
    
    [Fact]
    public async Task GetAllRecipientsAsync_MultiPage_Test()
    {
        var apiKey = new InMobileApiKey("UnitTestKey123");
        var pair1 = new RequestResponsePair(
            new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/some_LIST_id/recipients?pageLimit=250", jsonOrNull: null),
            new UnitTestResponseInfo(@"{
                    ""entries"": [
                        {
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
                            ""created"": ""2003-02-25T14:50:23Z""
                        },
                        {
                            ""externalCreated"": null,
                            ""numberInfo"": {
                                ""countryCode"": ""33"",
                                ""phoneNumber"": ""2222""
                            },
                            ""fields"": {
                                ""firstname"": ""Mrs"",
                                ""lastname"": ""Doubtfire""
                            },
                            ""id"": ""recId2"",
                            ""listId"": ""some_list_id"",
                            ""created"": ""2002-02-25T14:50:23Z""
                        }
                    ],
                    ""_links"": {
                        ""next"": ""/v4/lists/page/token_page_2"",
                        ""isLastPage"": false
                    }
                }"));

        // Testing an empty result in the middle of the flow
        var pair2 = new RequestResponsePair(
            new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/page/token_page_2", jsonOrNull: null),
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
                        ""externalCreated"": null,
                        ""numberInfo"": {
                            ""countryCode"": ""45"",
                            ""phoneNumber"": ""4444""
                        },
                        ""fields"": {
                            ""firstname"": ""Mr"",
                            ""lastname"": ""Anderson""
                        },
                        ""id"": ""recId3"",
                        ""listId"": ""some_list_id"",
                        ""created"": ""2002-02-25T14:50:23Z""
                    },
                    {
                        ""externalCreated"": null,
                        ""numberInfo"": {
                            ""countryCode"": ""33"",
                            ""phoneNumber"": ""5555""
                        },
                        ""fields"": {
                            ""firstname"": ""Mrs"",
                            ""lastname"": ""Doubtfire""
                        },
                        ""id"": ""recId4"",
                        ""listId"": ""some_list_id"",
                        ""created"": ""2003-02-25T14:50:23Z""
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
            var allEntries = await client.Lists.GetAllRecipientsInListAsync(listId: new RecipientListId("some_LIST_id"));
            Assert.Equal(4, allEntries.Count);

            {
                var entry1 = allEntries[0];
                Assert.Equal("recId1", entry1.Id.Value);
                Assert.True(entry1.ExternalCreated.HasValue);
                Assert.Equal(DateTimeKind.Utc, entry1.ExternalCreated.Value.Kind);
                Assert.Equal(new DateTime(2001, 02, 10, 14, 50, 23, DateTimeKind.Utc), entry1.ExternalCreated.Value);
                Assert.Equal(new DateTime(2003, 02, 25, 14, 50, 23, DateTimeKind.Utc), entry1.Created);
                Assert.Equal("45", entry1.NumberInfo.CountryCode);
                Assert.Equal("1111", entry1.NumberInfo.PhoneNumber);
                Assert.Equal("some_list_id", entry1.ListId.Value);
                Assert.Equal("Mr", entry1.Fields["firstname"]);
                Assert.Equal("Anderson", entry1.Fields["lastname"]);
            }

            Assert.Equal("recId2", allEntries[1].Id.Value);
            Assert.Equal("recId3", allEntries[2].Id.Value);
            Assert.Equal("recId4", allEntries[3].Id.Value);
        }
    }

    [Fact]
    public void GetAllRecipients_ApiError_FirstPage_Test()
    {
        var responseJson = @"{
""errorMessage"": ""Forbidden thing"",
""details"": [
""You shall not pass"",
""Go away""
]
}";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/some_LIST_id/recipients?pageLimit=250", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "500 ServerError");
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var ex = Assert.Throws<InMobileApiException>(() => client.Lists.GetAllRecipientsInList(listId: new RecipientListId("some_LIST_id")));

            Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
        }
    }
    
    [Fact]
    public async Task GetAllRecipientsAsync_ApiError_FirstPage_Test()
    {
        var responseJson = @"{
""errorMessage"": ""Forbidden thing"",
""details"": [
""You shall not pass"",
""Go away""
]
}";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/some_LIST_id/recipients?pageLimit=250", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "500 ServerError");
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var ex = await Assert.ThrowsAsync<InMobileApiException>(() => client.Lists.GetAllRecipientsInListAsync(listId: new RecipientListId("some_LIST_id")));

            Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
        }
    }

    [Fact]
    public void GetAllRecipients_ApiError_MultiPage_Test()
    {
        var apiKey = new InMobileApiKey("UnitTestKey123");
        var pair1 = new RequestResponsePair(
            new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/some_LIST_id/recipients?pageLimit=250", jsonOrNull: null),
            new UnitTestResponseInfo(@"{
                    ""entries"": [
                        {
                            ""externalCreated"": null,
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
                            ""created"": ""2002-02-25T14:50:23Z""
                        },
                        {
                            ""externalCreated"": null,
                            ""numberInfo"": {
                                ""countryCode"": ""33"",
                                ""phoneNumber"": ""2222""
                            },
                            ""fields"": {
                                ""firstname"": ""Mrs"",
                                ""lastname"": ""Doubtfire""
                            },
                            ""id"": ""recId2"",
                            ""listId"": ""some_list_id"",
                            ""created"": ""2003-02-25T14:50:23Z""
                        }
                    ],
                    ""_links"": {
                        ""next"": ""/v4/lists/page/token_page_2"",
                        ""isLastPage"": false
                    }
                }"));

        // Testing an empty result in the middle of the flow
        var pair2 = new RequestResponsePair(
            new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/page/token_page_2", jsonOrNull: null),
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
            var ex = Assert.Throws<InMobileApiException>(() => client.Lists.GetAllRecipientsInList(listId: new RecipientListId("some_LIST_id")));

            Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
        }
    }
    
    [Fact]
    public async Task GetAllRecipientsAsync_ApiError_MultiPage_Test()
    {
        var apiKey = new InMobileApiKey("UnitTestKey123");
        var pair1 = new RequestResponsePair(
            new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/some_LIST_id/recipients?pageLimit=250", jsonOrNull: null),
            new UnitTestResponseInfo(@"{
                    ""entries"": [
                        {
                            ""externalCreated"": null,
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
                            ""created"": ""2002-02-25T14:50:23Z""
                        },
                        {
                            ""externalCreated"": null,
                            ""numberInfo"": {
                                ""countryCode"": ""33"",
                                ""phoneNumber"": ""2222""
                            },
                            ""fields"": {
                                ""firstname"": ""Mrs"",
                                ""lastname"": ""Doubtfire""
                            },
                            ""id"": ""recId2"",
                            ""listId"": ""some_list_id"",
                            ""created"": ""2003-02-25T14:50:23Z""
                        }
                    ],
                    ""_links"": {
                        ""next"": ""/v4/lists/page/token_page_2"",
                        ""isLastPage"": false
                    }
                }"));

        // Testing an empty result in the middle of the flow
        var pair2 = new RequestResponsePair(
            new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/page/token_page_2", jsonOrNull: null),
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
            var ex = await Assert.ThrowsAsync<InMobileApiException>(() => client.Lists.GetAllRecipientsInListAsync(listId: new RecipientListId("some_LIST_id")));

            Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
        }
    }
}