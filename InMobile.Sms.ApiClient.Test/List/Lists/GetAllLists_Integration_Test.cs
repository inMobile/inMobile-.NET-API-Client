using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.Lists
{
    public class GetAllLists_Integration_Test
    {
        [Fact]
        public void EmptyResult_Test()
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
        public void SinglePage_Test()
        {
            var responseJson = @"{
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
                    Assert.Equal("id1", entry1.ListId);
                    Assert.Equal("name1", entry1.Name);
                }

                {
                    var entry2 = allEntries[1];
                    Assert.Equal("id2", entry2.ListId);
                    Assert.Equal("name2", entry2.Name);
                }
            }
        }

        [Fact]
        public void MultiPage_Test()
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
                ""entries"": [
                    {
                        ""id"":""id3"",
                        ""name"": ""name3""
                    },
                    {
                        ""id"":""id4"",
                        ""name"": ""name4""
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
                    Assert.Equal("id1", entry1.ListId);
                    Assert.Equal("name1", entry1.Name);
                }

                {
                    var entry2 = allEntries[1];
                    Assert.Equal("id2", entry2.ListId);
                    Assert.Equal("name2", entry2.Name);
                }

                {
                    var entry3 = allEntries[2];
                    Assert.Equal("id3", entry3.ListId);
                    Assert.Equal("name3", entry3.Name);
                }

                {
                    var entry4 = allEntries[3];
                    Assert.Equal("id4", entry4.ListId);
                    Assert.Equal("name4", entry4.Name);
                }
            }
        }
    }
}
