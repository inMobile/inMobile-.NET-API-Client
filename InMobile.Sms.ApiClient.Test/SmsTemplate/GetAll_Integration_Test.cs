using System;
using System.Net;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.SmsTemplate
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
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/sms/templates?pageLimit=250", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var allEntries = client.SmsTemplates.GetAll();
                Assert.Empty(allEntries);
            }
        }

        [Fact]
        public void SinglePage_Test()
        {
            var responseJson = @"{
                ""entries"": [
                    {
                        ""id"": ""some_template_id1"",
                        ""name"": ""My template1"",
                        ""text"": ""My template text {name} {lastname} 1"",
                        ""senderName"": ""My sendername 1"",
                        ""encoding"": ""gsm7"",
                        ""placeholders"": [
                            ""{name}"",
                            ""{lastname}""
                        ],
                        ""created"": ""2001-02-22T14:50:23Z"",
                        ""lastUpdated"": ""2001-02-24T16:30:10Z""
                    },
                    {
                        ""id"": ""some_template_id2"",
                        ""name"": ""My template2"",
                        ""text"": ""My template text {name} {lastname} 2"",
                        ""senderName"": ""My sendername 2"",
                        ""encoding"": ""ucs2"",
                        ""placeholders"": [
                            ""{name}""
                        ],
                        ""created"": ""2001-01-11T14:50:23Z"",
                        ""lastUpdated"": ""2001-01-14T16:30:10Z""
                    }
                ],
                ""_links"": {
                    ""next"": null,
                    ""isLastPage"": true
                }
            }";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/sms/templates?pageLimit=250", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var allEntries = client.SmsTemplates.GetAll();
                {
                    var entry1 = allEntries[0];
                    Assert.Equal("some_template_id1", entry1.Id.Value);
                    Assert.Equal("My template1", entry1.Name);
                    Assert.Equal("My template text {name} {lastname} 1", entry1.Text);
                    Assert.Equal("My sendername 1", entry1.SenderName);
                    Assert.Equal(MessageEncoding.Gsm7, entry1.Encoding);

                    Assert.Equal(2, entry1.Placeholders.Count);
                    Assert.Contains("{name}", entry1.Placeholders);
                    Assert.Contains("{lastname}", entry1.Placeholders);

                    Assert.Equal(new DateTime(2001, 02, 22, 14, 50, 23, DateTimeKind.Utc), entry1.Created);
                    Assert.Equal(new DateTime(2001, 02, 24, 16, 30, 10, DateTimeKind.Utc), entry1.LastUpdated);
                }

                {
                    var entry2 = allEntries[1];
                    Assert.Equal("some_template_id2", entry2.Id.Value);
                    Assert.Equal("My template2", entry2.Name);
                    Assert.Equal("My template text {name} {lastname} 2", entry2.Text);
                    Assert.Equal("My sendername 2", entry2.SenderName);
                    Assert.Equal(MessageEncoding.Ucs2, entry2.Encoding);

                    Assert.Single(entry2.Placeholders);
                    Assert.Contains("{name}", entry2.Placeholders);

                    Assert.Equal(new DateTime(2001, 01, 11, 14, 50, 23, DateTimeKind.Utc), entry2.Created);
                    Assert.Equal(new DateTime(2001, 01, 14, 16, 30, 10, DateTimeKind.Utc), entry2.LastUpdated);
                }
            }
        }

        [Fact]
        public void MultiPage_Test()
        {
            var apiKey = new InMobileApiKey("UnitTestKey123");
            var pair1 = new RequestResponsePair(
                new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/sms/templates?pageLimit=250", jsonOrNull: null),
                new UnitTestResponseInfo(@"{
                    ""entries"": [
                        {
                            ""id"": ""some_template_id1"",
                            ""name"": ""My template1"",
                            ""text"": ""My template text {name} {lastname} 1"",
                            ""senderName"": ""My sendername 1"",
                            ""encoding"": ""gsm7"",
                            ""placeholders"": [
                                ""{name}"",
                                ""{lastname}""
                            ],
                            ""created"": ""2001-02-22T14:50:23Z"",
                            ""lastUpdated"": ""2001-02-24T16:30:10Z""
                        },
                        {
                            ""id"": ""some_template_id2"",
                            ""name"": ""My template2"",
                            ""text"": ""My template text {name} {lastname} 2"",
                            ""senderName"": ""My sendername 2"",
                            ""encoding"": ""ucs2"",
                            ""placeholders"": [
                                ""{name}""
                            ],
                            ""created"": ""2001-01-11T14:50:23Z"",
                            ""lastUpdated"": ""2001-01-14T16:30:10Z""
                        }
                    ],
                    ""_links"": {
                        ""next"": ""/v4/sms/templates/page/token_page_2"",
                        ""isLastPage"": false
                    }
                }"));

            // Testing an empty result in the middle of the flow
            var pair2 = new RequestResponsePair(
                new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/sms/templates/page/token_page_2", jsonOrNull: null),
                new UnitTestResponseInfo(@"{
                    ""entries"": [
                    ],
                    ""_links"": {
                        ""next"": ""/v4/sms/templates/page/token_page_3"",
                        ""isLastPage"": false
                    }
                }"));

            var pair3 = new RequestResponsePair(
                new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/sms/templates/page/token_page_3", jsonOrNull: null),
                new UnitTestResponseInfo(@"{
                    ""entries"": [
                        {
                            ""id"": ""some_template_id3"",
                            ""name"": ""My template3"",
                            ""text"": ""My template text {name} {lastname} 3"",
                            ""senderName"": ""My sendername 3"",
                            ""encoding"": ""ucs2"",
                            ""placeholders"": [],
                            ""created"": ""2003-03-27T14:50:23Z"",
                            ""lastUpdated"": ""2003-04-10T16:30:10Z""
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
                var allEntries = client.SmsTemplates.GetAll();
                Assert.Equal(3, allEntries.Count);
                {
                    var entry1 = allEntries[0];
                    Assert.Equal("some_template_id1", entry1.Id.Value);
                    Assert.Equal("My template1", entry1.Name);
                    Assert.Equal("My template text {name} {lastname} 1", entry1.Text);
                    Assert.Equal("My sendername 1", entry1.SenderName);
                    Assert.Equal(MessageEncoding.Gsm7, entry1.Encoding);

                    Assert.Equal(2, entry1.Placeholders.Count);
                    Assert.Contains("{name}", entry1.Placeholders);
                    Assert.Contains("{lastname}", entry1.Placeholders);

                    Assert.Equal(new DateTime(2001, 02, 22, 14, 50, 23, DateTimeKind.Utc), entry1.Created);
                    Assert.Equal(new DateTime(2001, 02, 24, 16, 30, 10, DateTimeKind.Utc), entry1.LastUpdated);
                }

                {
                    var entry2 = allEntries[1];
                    Assert.Equal("some_template_id2", entry2.Id.Value);
                    Assert.Equal("My template2", entry2.Name);
                    Assert.Equal("My template text {name} {lastname} 2", entry2.Text);
                    Assert.Equal("My sendername 2", entry2.SenderName);
                    Assert.Equal(MessageEncoding.Ucs2, entry2.Encoding);

                    Assert.Single(entry2.Placeholders);
                    Assert.Contains("{name}", entry2.Placeholders);

                    Assert.Equal(new DateTime(2001, 01, 11, 14, 50, 23, DateTimeKind.Utc), entry2.Created);
                    Assert.Equal(new DateTime(2001, 01, 14, 16, 30, 10, DateTimeKind.Utc), entry2.LastUpdated);
                }

                {
                    var entry3 = allEntries[2];
                    Assert.Equal("some_template_id3", entry3.Id.Value);
                    Assert.Equal("My template3", entry3.Name);
                    Assert.Equal("My template text {name} {lastname} 3", entry3.Text);
                    Assert.Equal("My sendername 3", entry3.SenderName);
                    Assert.Equal(MessageEncoding.Ucs2, entry3.Encoding);

                    Assert.Empty(entry3.Placeholders);

                    Assert.Equal(new DateTime(2003, 03, 27, 14, 50, 23, DateTimeKind.Utc), entry3.Created);
                    Assert.Equal(new DateTime(2003, 04, 10, 16, 30, 10, DateTimeKind.Utc), entry3.LastUpdated);
                }
            }
        }

        [Fact]
        public void ApiError_Test()
        {
            var responseJson = @"{
                ""errorMessage"": ""Forbidden thing"",
                ""details"": [
                    ""You shall not pass"",
                    ""Go away""
                ]
            }";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/sms/templates?pageLimit=250", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "500 ServerError");
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var ex = Assert.Throws<InMobileApiException>(() => client.SmsTemplates.GetAll());

                Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
            }
        }
    }
}
