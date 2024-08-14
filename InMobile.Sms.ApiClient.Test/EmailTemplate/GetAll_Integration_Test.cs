using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.EmailTemplate;

public class GetAll_Integration_Test
{
    [Fact]
    public void GetAll_EmptyResult_Test()
    {
        var responseJson = @"{
                ""entries"": [],
                ""_links"": {
                    ""next"": null,
                    ""isLastPage"": true
                }
            }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/email/templates?pageLimit=250", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var allEntries = client.EmailTemplates.GetAll();
            Assert.Empty(allEntries);
        }
    }
    
    [Fact]
    public async Task GetAllAsync_EmptyResult_Test()
    {
        var responseJson = @"{
                ""entries"": [],
                ""_links"": {
                    ""next"": null,
                    ""isLastPage"": true
                }
            }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/email/templates?pageLimit=250", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var allEntries = await client.EmailTemplates.GetAllAsync();
            Assert.Empty(allEntries);
        }
    }

    [Fact]
    public void GetAll_SinglePage_Test()
    {
        var responseJson = @"{
                ""entries"": [
                    {
                        ""id"": ""some_template_id1"",
                        ""name"": ""Name1"",
                        ""html"": ""<!DOCTYPE html><html><head></head><body><p>Template 1. Hello {firstname}.This is the text version.</p></body></html>"",
                        ""text"": ""Template 1. Hello {firstname}.This is the text version."",
                        ""subject"": ""Subject1"",
                        ""preheader"": ""Preheader1"",
                        ""placeholders"": [
                            ""{firstname1}""
                        ],
                        ""created"": ""2021-01-01T08:08:48Z"",
                        ""lastUpdated"": ""2021-01-01T08:20:42Z""
                    },
                    {
                        ""id"": ""some_template_id2"",
                        ""name"": ""Name2"",
                        ""html"": ""<!DOCTYPE html><html><head></head><body><p>Template 2. Hello {firstname}.This is the text version.</p></body></html>"",
                        ""text"": ""Template 2. Hello {firstname}.This is the text version."",
                        ""subject"": ""Subject2"",
                        ""preheader"": ""Preheader2"",
                        ""placeholders"": [
                            ""{firstname2}""
                        ],
                        ""created"": ""2022-02-02T08:08:48Z"",
                        ""lastUpdated"": ""2022-02-02T08:20:42Z""
                    }
                ],
                ""_links"": {
                    ""next"": null,
                    ""isLastPage"": true
                }
            }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/email/templates?pageLimit=250", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var allEntries = client.EmailTemplates.GetAll();
            Assert.NotEmpty(allEntries);
            Assert.Equal(2, allEntries.Count);
            {
                var entry1 = allEntries[0];
                Assert.Equal("some_template_id1", entry1.Id.Value);
                Assert.Equal("Name1", entry1.Name);
                Assert.Equal("<!DOCTYPE html><html><head></head><body><p>Template 1. Hello {firstname}.This is the text version.</p></body></html>", entry1.Html);
                Assert.Equal("Template 1. Hello {firstname}.This is the text version.", entry1.Text);
                Assert.Equal("Subject1", entry1.Subject);
                Assert.Equal("Preheader1", entry1.Preheader);

                Assert.Single(entry1.Placeholders);
                Assert.Contains("{firstname1}", entry1.Placeholders);
                Assert.Equal(new DateTime(2021, 01, 01, 08, 08, 48, DateTimeKind.Utc), entry1.Created);
                Assert.Equal(new DateTime(2021, 01, 01, 08, 20, 42, DateTimeKind.Utc), entry1.LastUpdated);
            }
            {
                var entry2 = allEntries[1];
                Assert.Equal("some_template_id2", entry2.Id.Value);
                Assert.Equal("Name2", entry2.Name);
                Assert.Equal("<!DOCTYPE html><html><head></head><body><p>Template 2. Hello {firstname}.This is the text version.</p></body></html>", entry2.Html);
                Assert.Equal("Template 2. Hello {firstname}.This is the text version.", entry2.Text);
                Assert.Equal("Subject2", entry2.Subject);
                Assert.Equal("Preheader2", entry2.Preheader);

                Assert.Single(entry2.Placeholders);
                Assert.Contains("{firstname2}", entry2.Placeholders);
                Assert.Equal(new DateTime(2022, 02, 02, 08, 08, 48, DateTimeKind.Utc), entry2.Created);
                Assert.Equal(new DateTime(2022, 02, 02, 08, 20, 42, DateTimeKind.Utc), entry2.LastUpdated);
            }
        }
    }
    
    [Fact]
    public async Task GetAllAsync_SinglePage_Test()
    {
        var responseJson = @"{
                ""entries"": [
                    {
                        ""id"": ""some_template_id1"",
                        ""name"": ""Name1"",
                        ""html"": ""<!DOCTYPE html><html><head></head><body><p>Template 1. Hello {firstname}.This is the text version.</p></body></html>"",
                        ""text"": ""Template 1. Hello {firstname}.This is the text version."",
                        ""subject"": ""Subject1"",
                        ""preheader"": ""Preheader1"",
                        ""placeholders"": [
                            ""{firstname1}""
                        ],
                        ""created"": ""2021-01-01T08:08:48Z"",
                        ""lastUpdated"": ""2021-01-01T08:20:42Z""
                    },
                    {
                        ""id"": ""some_template_id2"",
                        ""name"": ""Name2"",
                        ""html"": ""<!DOCTYPE html><html><head></head><body><p>Template 2. Hello {firstname}.This is the text version.</p></body></html>"",
                        ""text"": ""Template 2. Hello {firstname}.This is the text version."",
                        ""subject"": ""Subject2"",
                        ""preheader"": ""Preheader2"",
                        ""placeholders"": [
                            ""{firstname2}""
                        ],
                        ""created"": ""2022-02-02T08:08:48Z"",
                        ""lastUpdated"": ""2022-02-02T08:20:42Z""
                    }
                ],
                ""_links"": {
                    ""next"": null,
                    ""isLastPage"": true
                }
            }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/email/templates?pageLimit=250", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var allEntries = await client.EmailTemplates.GetAllAsync();
            Assert.NotEmpty(allEntries);
            Assert.Equal(2, allEntries.Count);
            {
                var entry1 = allEntries[0];
                Assert.Equal("some_template_id1", entry1.Id.Value);
                Assert.Equal("Name1", entry1.Name);
                Assert.Equal("<!DOCTYPE html><html><head></head><body><p>Template 1. Hello {firstname}.This is the text version.</p></body></html>", entry1.Html);
                Assert.Equal("Template 1. Hello {firstname}.This is the text version.", entry1.Text);
                Assert.Equal("Subject1", entry1.Subject);
                Assert.Equal("Preheader1", entry1.Preheader);

                Assert.Single(entry1.Placeholders);
                Assert.Contains("{firstname1}", entry1.Placeholders);
                Assert.Equal(new DateTime(2021, 01, 01, 08, 08, 48, DateTimeKind.Utc), entry1.Created);
                Assert.Equal(new DateTime(2021, 01, 01, 08, 20, 42, DateTimeKind.Utc), entry1.LastUpdated);
            }
            {
                var entry2 = allEntries[1];
                Assert.Equal("some_template_id2", entry2.Id.Value);
                Assert.Equal("Name2", entry2.Name);
                Assert.Equal("<!DOCTYPE html><html><head></head><body><p>Template 2. Hello {firstname}.This is the text version.</p></body></html>", entry2.Html);
                Assert.Equal("Template 2. Hello {firstname}.This is the text version.", entry2.Text);
                Assert.Equal("Subject2", entry2.Subject);
                Assert.Equal("Preheader2", entry2.Preheader);

                Assert.Single(entry2.Placeholders);
                Assert.Contains("{firstname2}", entry2.Placeholders);
                Assert.Equal(new DateTime(2022, 02, 02, 08, 08, 48, DateTimeKind.Utc), entry2.Created);
                Assert.Equal(new DateTime(2022, 02, 02, 08, 20, 42, DateTimeKind.Utc), entry2.LastUpdated);
            }
        }
    }

    [Fact]
    public void GetAll_MultiPage_Test()
    {
        var apiKey = new InMobileApiKey("UnitTestKey123");

        var pair1 = new RequestResponsePair(
            request: new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/email/templates?pageLimit=250", jsonOrNull: null),
            response: new UnitTestResponseInfo(@"{
                    ""entries"": [
                        {
                            ""id"": ""some_template_id1"",
                            ""name"": ""Name1"",
                            ""html"": ""<!DOCTYPE html><html><head></head><body><p>Template 1. Hello {firstname}.This is the text version.</p></body></html>"",
                            ""text"": ""Template 1. Hello {firstname}.This is the text version."",
                            ""subject"": ""Subject1"",
                            ""preheader"": ""Preheader1"",
                            ""placeholders"": [
                                ""{firstname1}""
                            ],
                            ""created"": ""2021-01-01T08:08:48Z"",
                            ""lastUpdated"": ""2021-01-01T08:20:42Z""
                        },
                        {
                            ""id"": ""some_template_id2"",
                            ""name"": ""Name2"",
                            ""html"": ""<!DOCTYPE html><html><head></head><body><p>Template 2. Hello {firstname}.This is the text version.</p></body></html>"",
                            ""text"": ""Template 2. Hello {firstname}.This is the text version."",
                            ""subject"": ""Subject2"",
                            ""preheader"": ""Preheader2"",
                            ""placeholders"": [
                                ""{firstname2}""
                            ],
                            ""created"": ""2022-02-02T08:08:48Z"",
                            ""lastUpdated"": ""2022-02-02T08:20:42Z""
                        }
                    ],
                    ""_links"": {
                        ""next"": ""/v4/email/templates/page/token_page_2"",
                        ""isLastPage"": false
                    }
                }"));
        var pair2 = new RequestResponsePair(
            request: new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/email/templates/page/token_page_2", jsonOrNull: null),
            response: new UnitTestResponseInfo(@"{
                    ""entries"": [
                    ],
                    ""_links"": {
                        ""next"": ""/v4/email/templates/page/token_page_3"",
                        ""isLastPage"": false
                    }
                }"));
        var pair3 = new RequestResponsePair(
            request: new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/email/templates/page/token_page_3", jsonOrNull: null),
            response: new UnitTestResponseInfo(@"{
                    ""entries"": [
                        {
                            ""id"": ""some_template_id3"",
                            ""name"": ""Name3"",
                            ""html"": ""<!DOCTYPE html><html><head></head><body><p>Template 3. Hello {firstname}.This is the text version.</p></body></html>"",
                            ""text"": ""Template 3. Hello {firstname}.This is the text version."",
                            ""subject"": ""Subject3"",
                            ""preheader"": ""Preheader3"",
                            ""placeholders"": [
                                ""{firstname3}""
                            ],
                            ""created"": ""2023-03-03T08:08:48Z"",
                            ""lastUpdated"": ""2023-03-03T08:20:42Z""
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
            var allEntries = client.EmailTemplates.GetAll();
            Assert.NotEmpty(allEntries);
            Assert.Equal(3, allEntries.Count);
            {
                var entry1 = allEntries[0];
                Assert.Equal("some_template_id1", entry1.Id.Value);
                Assert.Equal("Name1", entry1.Name);
                Assert.Equal("<!DOCTYPE html><html><head></head><body><p>Template 1. Hello {firstname}.This is the text version.</p></body></html>", entry1.Html);
                Assert.Equal("Template 1. Hello {firstname}.This is the text version.", entry1.Text);
                Assert.Equal("Subject1", entry1.Subject);
                Assert.Equal("Preheader1", entry1.Preheader);

                Assert.Single(entry1.Placeholders);
                Assert.Contains("{firstname1}", entry1.Placeholders);
                Assert.Equal(new DateTime(2021, 01, 01, 08, 08, 48, DateTimeKind.Utc), entry1.Created);
                Assert.Equal(new DateTime(2021, 01, 01, 08, 20, 42, DateTimeKind.Utc), entry1.LastUpdated);
            }
            {
                var entry2 = allEntries[1];
                Assert.Equal("some_template_id2", entry2.Id.Value);
                Assert.Equal("Name2", entry2.Name);
                Assert.Equal("<!DOCTYPE html><html><head></head><body><p>Template 2. Hello {firstname}.This is the text version.</p></body></html>", entry2.Html);
                Assert.Equal("Template 2. Hello {firstname}.This is the text version.", entry2.Text);
                Assert.Equal("Subject2", entry2.Subject);
                Assert.Equal("Preheader2", entry2.Preheader);

                Assert.Single(entry2.Placeholders);
                Assert.Contains("{firstname2}", entry2.Placeholders);
                Assert.Equal(new DateTime(2022, 02, 02, 08, 08, 48, DateTimeKind.Utc), entry2.Created);
                Assert.Equal(new DateTime(2022, 02, 02, 08, 20, 42, DateTimeKind.Utc), entry2.LastUpdated);
            }
            {
                var entry3 = allEntries[2];
                Assert.Equal("some_template_id3", entry3.Id.Value);
                Assert.Equal("Name3", entry3.Name);
                Assert.Equal("<!DOCTYPE html><html><head></head><body><p>Template 3. Hello {firstname}.This is the text version.</p></body></html>", entry3.Html);
                Assert.Equal("Template 3. Hello {firstname}.This is the text version.", entry3.Text);
                Assert.Equal("Subject3", entry3.Subject);
                Assert.Equal("Preheader3", entry3.Preheader);

                Assert.Single(entry3.Placeholders);
                Assert.Contains("{firstname3}", entry3.Placeholders);
                Assert.Equal(new DateTime(2023, 03, 03, 08, 08, 48, DateTimeKind.Utc), entry3.Created);
                Assert.Equal(new DateTime(2023, 03, 03, 08, 20, 42, DateTimeKind.Utc), entry3.LastUpdated);
            }
        }
    }
    
    [Fact]
    public async Task GetAllAsync_MultiPage_Test()
    {
        var apiKey = new InMobileApiKey("UnitTestKey123");

        var pair1 = new RequestResponsePair(
            request: new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/email/templates?pageLimit=250", jsonOrNull: null),
            response: new UnitTestResponseInfo(@"{
                    ""entries"": [
                        {
                            ""id"": ""some_template_id1"",
                            ""name"": ""Name1"",
                            ""html"": ""<!DOCTYPE html><html><head></head><body><p>Template 1. Hello {firstname}.This is the text version.</p></body></html>"",
                            ""text"": ""Template 1. Hello {firstname}.This is the text version."",
                            ""subject"": ""Subject1"",
                            ""preheader"": ""Preheader1"",
                            ""placeholders"": [
                                ""{firstname1}""
                            ],
                            ""created"": ""2021-01-01T08:08:48Z"",
                            ""lastUpdated"": ""2021-01-01T08:20:42Z""
                        },
                        {
                            ""id"": ""some_template_id2"",
                            ""name"": ""Name2"",
                            ""html"": ""<!DOCTYPE html><html><head></head><body><p>Template 2. Hello {firstname}.This is the text version.</p></body></html>"",
                            ""text"": ""Template 2. Hello {firstname}.This is the text version."",
                            ""subject"": ""Subject2"",
                            ""preheader"": ""Preheader2"",
                            ""placeholders"": [
                                ""{firstname2}""
                            ],
                            ""created"": ""2022-02-02T08:08:48Z"",
                            ""lastUpdated"": ""2022-02-02T08:20:42Z""
                        }
                    ],
                    ""_links"": {
                        ""next"": ""/v4/email/templates/page/token_page_2"",
                        ""isLastPage"": false
                    }
                }"));
        var pair2 = new RequestResponsePair(
            request: new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/email/templates/page/token_page_2", jsonOrNull: null),
            response: new UnitTestResponseInfo(@"{
                    ""entries"": [
                    ],
                    ""_links"": {
                        ""next"": ""/v4/email/templates/page/token_page_3"",
                        ""isLastPage"": false
                    }
                }"));
        var pair3 = new RequestResponsePair(
            request: new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/email/templates/page/token_page_3", jsonOrNull: null),
            response: new UnitTestResponseInfo(@"{
                    ""entries"": [
                        {
                            ""id"": ""some_template_id3"",
                            ""name"": ""Name3"",
                            ""html"": ""<!DOCTYPE html><html><head></head><body><p>Template 3. Hello {firstname}.This is the text version.</p></body></html>"",
                            ""text"": ""Template 3. Hello {firstname}.This is the text version."",
                            ""subject"": ""Subject3"",
                            ""preheader"": ""Preheader3"",
                            ""placeholders"": [
                                ""{firstname3}""
                            ],
                            ""created"": ""2023-03-03T08:08:48Z"",
                            ""lastUpdated"": ""2023-03-03T08:20:42Z""
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
            var allEntries = await client.EmailTemplates.GetAllAsync();
            Assert.NotEmpty(allEntries);
            Assert.Equal(3, allEntries.Count);
            {
                var entry1 = allEntries[0];
                Assert.Equal("some_template_id1", entry1.Id.Value);
                Assert.Equal("Name1", entry1.Name);
                Assert.Equal("<!DOCTYPE html><html><head></head><body><p>Template 1. Hello {firstname}.This is the text version.</p></body></html>", entry1.Html);
                Assert.Equal("Template 1. Hello {firstname}.This is the text version.", entry1.Text);
                Assert.Equal("Subject1", entry1.Subject);
                Assert.Equal("Preheader1", entry1.Preheader);

                Assert.Single(entry1.Placeholders);
                Assert.Contains("{firstname1}", entry1.Placeholders);
                Assert.Equal(new DateTime(2021, 01, 01, 08, 08, 48, DateTimeKind.Utc), entry1.Created);
                Assert.Equal(new DateTime(2021, 01, 01, 08, 20, 42, DateTimeKind.Utc), entry1.LastUpdated);
            }
            {
                var entry2 = allEntries[1];
                Assert.Equal("some_template_id2", entry2.Id.Value);
                Assert.Equal("Name2", entry2.Name);
                Assert.Equal("<!DOCTYPE html><html><head></head><body><p>Template 2. Hello {firstname}.This is the text version.</p></body></html>", entry2.Html);
                Assert.Equal("Template 2. Hello {firstname}.This is the text version.", entry2.Text);
                Assert.Equal("Subject2", entry2.Subject);
                Assert.Equal("Preheader2", entry2.Preheader);

                Assert.Single(entry2.Placeholders);
                Assert.Contains("{firstname2}", entry2.Placeholders);
                Assert.Equal(new DateTime(2022, 02, 02, 08, 08, 48, DateTimeKind.Utc), entry2.Created);
                Assert.Equal(new DateTime(2022, 02, 02, 08, 20, 42, DateTimeKind.Utc), entry2.LastUpdated);
            }
            {
                var entry3 = allEntries[2];
                Assert.Equal("some_template_id3", entry3.Id.Value);
                Assert.Equal("Name3", entry3.Name);
                Assert.Equal("<!DOCTYPE html><html><head></head><body><p>Template 3. Hello {firstname}.This is the text version.</p></body></html>", entry3.Html);
                Assert.Equal("Template 3. Hello {firstname}.This is the text version.", entry3.Text);
                Assert.Equal("Subject3", entry3.Subject);
                Assert.Equal("Preheader3", entry3.Preheader);

                Assert.Single(entry3.Placeholders);
                Assert.Contains("{firstname3}", entry3.Placeholders);
                Assert.Equal(new DateTime(2023, 03, 03, 08, 08, 48, DateTimeKind.Utc), entry3.Created);
                Assert.Equal(new DateTime(2023, 03, 03, 08, 20, 42, DateTimeKind.Utc), entry3.LastUpdated);
            }
        }
    }

    [Fact]
    public void GetAll_ApiError_Test()
    {
        var responseJson = @"{
                ""errorMessage"": ""Forbidden thing"",
                ""details"": [
                    ""You shall not pass"",
                    ""Go away""
                ]
            }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/email/templates?pageLimit=250", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "500 ServerError");
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var ex = Assert.Throws<InMobileApiException>(() => client.EmailTemplates.GetAll());

            Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
        }
    }
    
    [Fact]
    public async Task GetAllAsync_ApiError_Test()
    {
        var responseJson = @"{
                ""errorMessage"": ""Forbidden thing"",
                ""details"": [
                    ""You shall not pass"",
                    ""Go away""
                ]
            }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/email/templates?pageLimit=250", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "500 ServerError");
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var ex = await Assert.ThrowsAsync<InMobileApiException>(() => client.EmailTemplates.GetAllAsync());

            Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
        }
    }
}