#pragma warning disable xUnit2003 // Do not use equality check to test for null value
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.Blacklist
{
    public class GetAll_Integration_Test
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
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/blacklist?pageLimit=250", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var allEntries = client.Blacklist.GetAll();
                Assert.Empty(allEntries);
            }
        }

        [Fact]
        public void SinglePage_Test()
        {
            var responseJson = @"{
                ""entries"": [
                    {
                        ""numberInfo"": {
                            ""countryCode"": ""45"",
                            ""phoneNumber"": ""1111""
                        },
                        ""comment"": ""Some text provided when created"",
                        ""id"": ""111""
                    },
                    {
                        ""numberInfo"": {
                            ""countryCode"": ""45"",
                            ""phoneNumber"": ""2222""
                        },
                        ""comment"": null,
                        ""id"": ""222""
                    }
                ],
                ""_links"": {
                    ""next"": null,
                    ""isLastPage"": true
                }
            }";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/blacklist?pageLimit=250", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var allEntries = client.Blacklist.GetAll();
                Assert.Equal(2, allEntries.Count);
                {
                    var entry1 = allEntries[0];
                    Assert.Equal("111", entry1.Id.Value);
                    Assert.Equal("Some text provided when created", entry1.Comment);
                    Assert.Equal("45", entry1.NumberInfo.CountryCode);
                    Assert.Equal("1111", entry1.NumberInfo.PhoneNumber);
                }

                {
                    var entry2 = allEntries[1];
                    Assert.Equal("222", entry2.Id.Value);
                    Assert.Equal(null, entry2.Comment);
                    Assert.Equal("45", entry2.NumberInfo.CountryCode);
                    Assert.Equal("2222", entry2.NumberInfo.PhoneNumber);
                }
            }
        }

        [Fact]
        public void MultiPage_Test()
        {
            var apiKey = new InMobileApiKey("UnitTestKey123");
            var pair1 = new RequestResponsePair(new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/blacklist?pageLimit=250", jsonOrNull: null),
                        new UnitTestResponseInfo(@"{
                            ""entries"": [
                                {
                                    ""numberInfo"": {
                                        ""countryCode"": ""45"",
                                        ""phoneNumber"": ""1111""
                                    },
                                    ""comment"": ""Some text provided when created"",
                                    ""id"": ""111""
                                },
                                {
                                    ""numberInfo"": {
                                        ""countryCode"": ""45"",
                                        ""phoneNumber"": ""2222""
                                    },
                                    ""comment"": null,
                                    ""id"": ""222""
                                }
                            ],
                            ""_links"": {
                                ""next"": ""/v4/blacklist/page/token_page_2"",
                                ""isLastPage"": false
                            }
                        }"));

            // Testing an empty result in the middle of the flow
            var pair2 = new RequestResponsePair(new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/blacklist/page/token_page_2", jsonOrNull: null),
                            new UnitTestResponseInfo(@"{
                                ""entries"": [
                                ],
                                ""_links"": {
                                    ""next"": ""/v4/blacklist/page/token_page_3"",
                                    ""isLastPage"": false
                                }
                            }"));

            var pair3 = new RequestResponsePair(new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/blacklist/page/token_page_3", jsonOrNull: null),
             new UnitTestResponseInfo(@"{
                ""entries"": [
                    {
                        ""numberInfo"": {
                            ""countryCode"": ""45"",
                            ""phoneNumber"": ""3333""
                        },
                        ""comment"": null,
                        ""id"": ""333""
                    },
                    {
                        ""numberInfo"": {
                            ""countryCode"": ""45"",
                            ""phoneNumber"": ""4444""
                        },
                        ""comment"": null,
                        ""id"": ""444""
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
                var allEntries = client.Blacklist.GetAll();
                Assert.Equal(4, allEntries.Count);
                {
                    var entry1 = allEntries[0];
                    Assert.Equal("111", entry1.Id.Value);
                    Assert.Equal("Some text provided when created", entry1.Comment);
                    Assert.Equal("45", entry1.NumberInfo.CountryCode);
                    Assert.Equal("1111", entry1.NumberInfo.PhoneNumber);
                }

                {
                    var entry2 = allEntries[1];
                    Assert.Equal("222", entry2.Id.Value);

                    Assert.Equal(null, entry2.Comment);
                    Assert.Equal("45", entry2.NumberInfo.CountryCode);
                    Assert.Equal("2222", entry2.NumberInfo.PhoneNumber);
                }

                {
                    var entry3 = allEntries[2];
                    Assert.Equal("333", entry3.Id.Value);
                    Assert.Equal(null, entry3.Comment);
                    Assert.Equal("45", entry3.NumberInfo.CountryCode);
                    Assert.Equal("3333", entry3.NumberInfo.PhoneNumber);
                }

                {
                    var entry4 = allEntries[3];
                    Assert.Equal("444", entry4.Id.Value);
                    Assert.Equal(null, entry4.Comment);
                    Assert.Equal("45", entry4.NumberInfo.CountryCode);
                    Assert.Equal("4444", entry4.NumberInfo.PhoneNumber);
                }
            }
        }

        [Fact]
        public void GetFirstPage_ApiError_Test()
        {
            var responseJson = @"{
""errorMessage"": ""Forbidden thing"",
""details"": [
""You shall not pass"",
""Go away""
]
}";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/blacklist?pageLimit=250", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "500 ServerError");
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var ex = Assert.Throws<InMobileApiException>(() => client.Blacklist.GetAll());

                Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
            }
        }

        [Fact]
        public void MultiPage_ApiError_Test()
        {
            var apiKey = new InMobileApiKey("UnitTestKey123");
            var pair1 = new RequestResponsePair(new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/blacklist?pageLimit=250", jsonOrNull: null),
                        new UnitTestResponseInfo(@"{
                            ""entries"": [
                                {
                                    ""numberInfo"": {
                                        ""countryCode"": ""45"",
                                        ""phoneNumber"": ""1111""
                                    },
                                    ""comment"": ""Some text provided when created"",
                                    ""id"": ""111""
                                },
                                {
                                    ""numberInfo"": {
                                        ""countryCode"": ""45"",
                                        ""phoneNumber"": ""2222""
                                    },
                                    ""comment"": null,
                                    ""id"": ""222""
                                }
                            ],
                            ""_links"": {
                                ""next"": ""/v4/blacklist/page/token_page_2"",
                                ""isLastPage"": false
                            }
                        }"));

            // Testing an empty result in the middle of the flow
            var pair2 = new RequestResponsePair(new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/blacklist/page/token_page_2", jsonOrNull: null),
                            new UnitTestResponseInfo(@"{
                                ""entries"": [
                                ],
                                ""_links"": {
                                    ""next"": ""/v4/blacklist/page/token_page_3"",
                                    ""isLastPage"": false
                                }
                            }"));

            var pair3 = new RequestResponsePair(new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/blacklist/page/token_page_3", jsonOrNull: null),
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
                var ex = Assert.Throws<InMobileApiException>(() => client.Blacklist.GetAll());

                Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
            }
        }
    }
}
