using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.EmailTemplate;

public class GetById_Integration_Test
{
    [Fact]
    public void GetById_Success_Test()
    {
        var responseJson = @"{
                ""id"": ""some_template_id"",
                ""name"": ""inMobile API Client"",
                ""html"": ""<!DOCTYPE html><html><head></head><body><p>Hello {firstname}.This is the text version.</p></body></html>"",
                ""text"": ""Hello {firstname}.This is the text version."",
                ""subject"": ""inMobile API Client Test"",
                ""preheader"": ""Preheader text"",
                ""placeholders"": [
                    ""{firstname}""
                ],
                ""created"": ""2023-05-09T08:08:48Z"",
                ""lastUpdated"": ""2023-05-09T08:20:42Z""
            }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/email/templates/some_template_id", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var entry = client.EmailTemplates.GetById(templateId: new EmailTemplateId("some_template_id"));

            Assert.Equal("some_template_id", entry.Id.Value);
            Assert.Equal("inMobile API Client", entry.Name);
            Assert.Equal("<!DOCTYPE html><html><head></head><body><p>Hello {firstname}.This is the text version.</p></body></html>", entry.Html);
            Assert.Equal("Hello {firstname}.This is the text version.", entry.Text);
            Assert.Equal("inMobile API Client Test", entry.Subject);
            Assert.Equal("Preheader text", entry.Preheader);

            Assert.Single(entry.Placeholders);
            Assert.Contains("{firstname}", entry.Placeholders);
            Assert.Equal(new DateTime(2023, 05, 09, 08, 08, 48, DateTimeKind.Utc), entry.Created);
            Assert.Equal(new DateTime(2023, 05, 09, 08, 20, 42, DateTimeKind.Utc), entry.LastUpdated);
        }
    }
    
    [Fact]
    public async Task GetByIdAsync_Success_Test()
    {
        var responseJson = @"{
                ""id"": ""some_template_id"",
                ""name"": ""inMobile API Client"",
                ""html"": ""<!DOCTYPE html><html><head></head><body><p>Hello {firstname}.This is the text version.</p></body></html>"",
                ""text"": ""Hello {firstname}.This is the text version."",
                ""subject"": ""inMobile API Client Test"",
                ""preheader"": ""Preheader text"",
                ""placeholders"": [
                    ""{firstname}""
                ],
                ""created"": ""2023-05-09T08:08:48Z"",
                ""lastUpdated"": ""2023-05-09T08:20:42Z""
            }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/email/templates/some_template_id", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var entry = await client.EmailTemplates.GetByIdAsync(templateId: new EmailTemplateId("some_template_id"));

            Assert.Equal("some_template_id", entry.Id.Value);
            Assert.Equal("inMobile API Client", entry.Name);
            Assert.Equal("<!DOCTYPE html><html><head></head><body><p>Hello {firstname}.This is the text version.</p></body></html>", entry.Html);
            Assert.Equal("Hello {firstname}.This is the text version.", entry.Text);
            Assert.Equal("inMobile API Client Test", entry.Subject);
            Assert.Equal("Preheader text", entry.Preheader);

            Assert.Single(entry.Placeholders);
            Assert.Contains("{firstname}", entry.Placeholders);
            Assert.Equal(new DateTime(2023, 05, 09, 08, 08, 48, DateTimeKind.Utc), entry.Created);
            Assert.Equal(new DateTime(2023, 05, 09, 08, 20, 42, DateTimeKind.Utc), entry.LastUpdated);
        }
    }

    [Fact]
    public void GetById_ApiError_NotFound_Test()
    {
        var responseJson = @"{
                ""errorMessage"": ""Could not find template: some_template_id"",
                ""details"": []
            }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/email/templates/some_template_id", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "404 Not Found");
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");

            var ex = Assert.Throws<InMobileApiException>(() => client.EmailTemplates.GetById(new EmailTemplateId("some_template_id")));
            Assert.Equal(HttpStatusCode.NotFound, ex.ErrorHttpStatusCode);
        }
    }
    
    [Fact]
    public async Task GetByIdAsync_ApiError_NotFound_Test()
    {
        var responseJson = @"{
                ""errorMessage"": ""Could not find template: some_template_id"",
                ""details"": []
            }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/email/templates/some_template_id", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "404 Not Found");
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");

            var ex = await Assert.ThrowsAsync<InMobileApiException>(() => client.EmailTemplates.GetByIdAsync(new EmailTemplateId("some_template_id")));
            Assert.Equal(HttpStatusCode.NotFound, ex.ErrorHttpStatusCode);
        }
    }

    [Fact]
    public void GetById_ApiError_InternalServerError_Test()
    {
        var responseJson = @"{
                ""errorMessage"": ""Forbidden thing"",
                ""details"": [
                    ""You shall not pass"",
                    ""Go away""
                ]
            }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/email/templates/some_template_id", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "500 ServerError");
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");

            var ex = Assert.Throws<InMobileApiException>(() => client.EmailTemplates.GetById(new EmailTemplateId("some_template_id")));
            Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
        }
    }
    
    [Fact]
    public async Task GetByIdAsync_ApiError_InternalServerError_Test()
    {
        var responseJson = @"{
                ""errorMessage"": ""Forbidden thing"",
                ""details"": [
                    ""You shall not pass"",
                    ""Go away""
                ]
            }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/email/templates/some_template_id", jsonOrNull: null);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "500 ServerError");
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");

            var ex = await Assert.ThrowsAsync<InMobileApiException>(() => client.EmailTemplates.GetByIdAsync(new EmailTemplateId("some_template_id")));
            Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
        }
    }
}