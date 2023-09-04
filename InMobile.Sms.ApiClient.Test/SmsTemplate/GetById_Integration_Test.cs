using System;
using System.Net;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.SmsTemplate
{
    public class GetById_Integration_Test
    {
        [Fact]
        public void GetById_Success_Test() 
        {
            var responseJson = @"{
                ""id"": ""some_template_id"",
                ""name"": ""My template"",
                ""text"": ""My template text {name} {lastname}"",
                ""senderName"": ""My sendername"",
                ""encoding"": ""gsm7"",
                ""placeholders"": [
                    ""{name}"",
                    ""{lastname}""
                ],
                ""created"": ""2001-02-22T14:50:23Z"",
                ""lastUpdated"": ""2001-02-24T16:30:10Z"",
                ""future_field_not_yet_known"": ""Hello""
            }";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/sms/templates/some_template_id", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var entry = client.SmsTemplates.GetById(templateId: new SmsTemplateId("some_template_id"));

                Assert.Equal("some_template_id", entry.Id.Value);
                Assert.Equal("My template", entry.Name);
                Assert.Equal("My template text {name} {lastname}", entry.Text);
                Assert.Equal("My sendername", entry.SenderName);
                Assert.Equal(MessageEncoding.Gsm7, entry.Encoding);

                Assert.Equal(2, entry.Placeholders.Count);
                Assert.Contains("{name}", entry.Placeholders);
                Assert.Contains("{lastname}", entry.Placeholders);

                Assert.Equal(new DateTime(2001, 02, 22, 14, 50, 23, DateTimeKind.Utc), entry.Created);
                Assert.Equal(new DateTime(2001, 02, 24, 16, 30, 10, DateTimeKind.Utc), entry.LastUpdated);
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
             var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/sms/templates/some_template_id", jsonOrNull: null);
             var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "404 Not Found");
             using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
             {
                 var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
             
                 var ex = Assert.Throws<InMobileApiException>(() => client.SmsTemplates.GetById(new SmsTemplateId("some_template_id")));
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
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/sms/templates/some_template_id", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "500 ServerError");
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");

                var ex = Assert.Throws<InMobileApiException>(() => client.SmsTemplates.GetById(new SmsTemplateId("some_template_id")));
                Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
            }
        }
    }
}
