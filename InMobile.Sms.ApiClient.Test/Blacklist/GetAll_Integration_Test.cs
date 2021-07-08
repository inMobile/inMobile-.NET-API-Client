using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.Blacklist
{
    public class GetAll_Integration_Test
    {
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
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new UnitTestRequestAndResponse(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var allEntries = client.Blacklist.GetAll();
                Assert.Equal(2, allEntries.Count);
                {
                    var entry1 = allEntries[0];
                    Assert.Equal("111", entry1.Id);
                    Assert.Equal("Some text provided when created", entry1.Comment);
                    Assert.Equal("45", entry1.NumberInfo.CountryCode);
                    Assert.Equal("1111", entry1.NumberInfo.PhoneNumber);
                }

                {
                    var entry2 = allEntries[1];
                    Assert.Equal("222", entry2.Id);
                    Assert.Equal(null, entry2.Comment);
                    Assert.Equal("45", entry2.NumberInfo.CountryCode);
                    Assert.Equal("2222", entry2.NumberInfo.PhoneNumber);
                }
            }
        }

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
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new UnitTestRequestAndResponse(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var allEntries = client.Blacklist.GetAll();
                Assert.Empty(allEntries);
            }
        }
    }
}
