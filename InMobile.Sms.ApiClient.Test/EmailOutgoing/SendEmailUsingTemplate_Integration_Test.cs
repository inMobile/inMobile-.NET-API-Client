using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.EmailOutgoing;

public class SendEmailUsingTemplate_Integration_Test
{
    [Fact]
    public void SendEmailUsingTemplate_WithOnlyRequiredFields_Success_Test()
    {
        var expectedRequestJson = @"{""From"":{""EmailAddress"":""support@inmobile.com"",""DisplayName"":""X from inMobile""},""To"":[{""EmailAddress"":""test@inmobile.com"",""DisplayName"":""Test""}],""Tracking"":true,""TemplateId"":""d8311715-3566-46df-81a5-dfd4dc61fb5b""}";
        var responseJson = @"{
    ""usedPlaceholderKeys"": [],
    ""notUsedPlaceholderKeys"": [],
    ""messageId"": ""59efc500-d38c-482c-a079-3314ff8e5f63"",
    ""to"": [
        {
            ""emailAddress"": ""test@inmobile.com"",
            ""displayName"": ""Test""
        }
    ]
}";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/email/outgoing/sendusingtemplate", jsonOrNull: expectedRequestJson);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var response = client.EmailOutgoing.SendEmailUsingTemplate(new OutgoingEmailTemplateCreateInfo(
                templateId: new EmailTemplateId("d8311715-3566-46df-81a5-dfd4dc61fb5b"),
                from: new EmailSender(emailAddress: "support@inmobile.com", displayName: "X from inMobile"),
                to: new List<EmailRecipient> { new EmailRecipient(emailAddress: "test@inmobile.com", displayName: "Test") }));

            Assert.NotNull(response);
            Assert.Equal(new OutgoingEmailId("59efc500-d38c-482c-a079-3314ff8e5f63"), response.MessageId);
            Assert.Empty(response.UsedPlaceholderKeys);
            Assert.Empty(response.NotUsedPlaceholderKeys);
            Assert.Single(response.To);
            var singleRecipient = response.To.Single();
            Assert.Equal("Test", singleRecipient.DisplayName);
            Assert.Equal("test@inmobile.com", singleRecipient.EmailAddress);
        }
    }
    
    [Fact]
    public async Task SendEmailUsingTemplateAsync_WithOnlyRequiredFields_Success_Test()
    {
        var expectedRequestJson = @"{""From"":{""EmailAddress"":""support@inmobile.com"",""DisplayName"":""X from inMobile""},""To"":[{""EmailAddress"":""test@inmobile.com"",""DisplayName"":""Test""}],""Tracking"":true,""TemplateId"":""d8311715-3566-46df-81a5-dfd4dc61fb5b""}";
        var responseJson = @"{
    ""usedPlaceholderKeys"": [],
    ""notUsedPlaceholderKeys"": [],
    ""messageId"": ""59efc500-d38c-482c-a079-3314ff8e5f63"",
    ""to"": [
        {
            ""emailAddress"": ""test@inmobile.com"",
            ""displayName"": ""Test""
        }
    ]
}";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/email/outgoing/sendusingtemplate", jsonOrNull: expectedRequestJson);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var response = await client.EmailOutgoing.SendEmailUsingTemplateAsync(new OutgoingEmailTemplateCreateInfo(
                templateId: new EmailTemplateId("d8311715-3566-46df-81a5-dfd4dc61fb5b"),
                from: new EmailSender(emailAddress: "support@inmobile.com", displayName: "X from inMobile"),
                to: new List<EmailRecipient> { new EmailRecipient(emailAddress: "test@inmobile.com", displayName: "Test") }));

            Assert.NotNull(response);
            Assert.Equal(new OutgoingEmailId("59efc500-d38c-482c-a079-3314ff8e5f63"), response.MessageId);
            Assert.Empty(response.UsedPlaceholderKeys);
            Assert.Empty(response.NotUsedPlaceholderKeys);
            Assert.Single(response.To);
            var singleRecipient = response.To.Single();
            Assert.Equal("Test", singleRecipient.DisplayName);
            Assert.Equal("test@inmobile.com", singleRecipient.EmailAddress);
        }
    }

    [Fact]
    public void SendEmailUsingTemplate_WithAllFields_Success_Test()
    {
        var expectedRequestJson = @"{""From"":{""EmailAddress"":""noreply@inmobile.com"",""DisplayName"":""X from inMobile""},""To"":[{""EmailAddress"":""test@inmobile.com"",""DisplayName"":""Test""}],""ReplyTo"":[{""EmailAddress"":""support@inmobile.com""}],""MessageId"":""02a7ec4b-9056-4228-bd6e-6d9af385716d"",""SendTime"":""2020-10-10T16:13:00Z"",""Tracking"":false,""TemplateId"":""d8311715-3566-46df-81a5-dfd4dc61fb5b"",""Placeholders"":{""{firstname}"":""TestABC""}}";
        var responseJson = @"{
    ""usedPlaceholderKeys"": [
        ""{firstname}""
    ],
    ""messageId"": ""02a7ec4b-9056-4228-bd6e-6d9af385716d"",
    ""to"": [
        {
            ""emailAddress"": ""test@inmobile.com"",
            ""displayName"": ""Test""
        }
    ]
}";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/email/outgoing/sendusingtemplate", jsonOrNull: expectedRequestJson);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var response = client.EmailOutgoing.SendEmailUsingTemplate(new OutgoingEmailTemplateCreateInfo(
                templateId: new EmailTemplateId("d8311715-3566-46df-81a5-dfd4dc61fb5b"),
                from: new EmailSender(emailAddress: "noreply@inmobile.com", displayName: "X from inMobile"),
                to: new List<EmailRecipient> { new EmailRecipient(emailAddress: "test@inmobile.com", displayName: "Test") },
                replyTo: new List<EmailReplyToRecipient> { new EmailReplyToRecipient(emailAddress: "support@inmobile.com") },
                sendTime: new DateTime(2020, 10, 10, 16, 13, 0, kind: DateTimeKind.Utc),
                tracking: false,
                messageId: new OutgoingEmailId("02a7ec4b-9056-4228-bd6e-6d9af385716d"),
                placeholders: new Dictionary<string, string> { { "{firstname}", "TestABC" } }));

            Assert.NotNull(response);
            Assert.Equal(new OutgoingEmailId("02a7ec4b-9056-4228-bd6e-6d9af385716d"), response.MessageId);
            Assert.Single(response.UsedPlaceholderKeys);
            Assert.Empty(response.NotUsedPlaceholderKeys);
            Assert.Single(response.To);
            var singleRecipient = response.To.Single();
            Assert.Equal("Test", singleRecipient.DisplayName);
            Assert.Equal("test@inmobile.com", singleRecipient.EmailAddress);
        }
    }
    
    [Fact]
    public async Task SendEmailUsingTemplateAsync_WithAllFields_Success_Test()
    {
        var expectedRequestJson = @"{""From"":{""EmailAddress"":""noreply@inmobile.com"",""DisplayName"":""X from inMobile""},""To"":[{""EmailAddress"":""test@inmobile.com"",""DisplayName"":""Test""}],""ReplyTo"":[{""EmailAddress"":""support@inmobile.com""}],""MessageId"":""02a7ec4b-9056-4228-bd6e-6d9af385716d"",""SendTime"":""2020-10-10T16:13:00Z"",""Tracking"":false,""TemplateId"":""d8311715-3566-46df-81a5-dfd4dc61fb5b"",""Placeholders"":{""{firstname}"":""TestABC""}}";
        var responseJson = @"{
    ""usedPlaceholderKeys"": [
        ""{firstname}""
    ],
    ""messageId"": ""02a7ec4b-9056-4228-bd6e-6d9af385716d"",
    ""to"": [
        {
            ""emailAddress"": ""test@inmobile.com"",
            ""displayName"": ""Test""
        }
    ]
}";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/email/outgoing/sendusingtemplate", jsonOrNull: expectedRequestJson);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var response = await client.EmailOutgoing.SendEmailUsingTemplateAsync(new OutgoingEmailTemplateCreateInfo(
                templateId: new EmailTemplateId("d8311715-3566-46df-81a5-dfd4dc61fb5b"),
                from: new EmailSender(emailAddress: "noreply@inmobile.com", displayName: "X from inMobile"),
                to: new List<EmailRecipient> { new EmailRecipient(emailAddress: "test@inmobile.com", displayName: "Test") },
                replyTo: new List<EmailReplyToRecipient> { new EmailReplyToRecipient(emailAddress: "support@inmobile.com") },
                sendTime: new DateTime(2020, 10, 10, 16, 13, 0, kind: DateTimeKind.Utc),
                tracking: false,
                messageId: new OutgoingEmailId("02a7ec4b-9056-4228-bd6e-6d9af385716d"),
                placeholders: new Dictionary<string, string> { { "{firstname}", "TestABC" } }));

            Assert.NotNull(response);
            Assert.Equal(new OutgoingEmailId("02a7ec4b-9056-4228-bd6e-6d9af385716d"), response.MessageId);
            Assert.Single(response.UsedPlaceholderKeys);
            Assert.Empty(response.NotUsedPlaceholderKeys);
            Assert.Single(response.To);
            var singleRecipient = response.To.Single();
            Assert.Equal("Test", singleRecipient.DisplayName);
            Assert.Equal("test@inmobile.com", singleRecipient.EmailAddress);
        }
    }

    [Fact]
    public void SendEmailUsingTemplate_ApiError_Test()
    {
        var expectedRequestJson = @"{""From"":{""EmailAddress"":""support@inmobile.com"",""DisplayName"":""X from inMobile""},""To"":[{""EmailAddress"":""test@inmobile.com"",""DisplayName"":""Test""}],""Tracking"":true,""TemplateId"":""bf6e3dee-92b3-482a-b45a-991ae61bba1c""}";
        var responseJson = @"{
    ""errorMessage"": ""Domain not validated"",
    ""details"": []
}";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/email/outgoing/sendusingtemplate", jsonOrNull: expectedRequestJson);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "400 BadRequest");
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var ex = Assert.Throws<InMobileApiException>(() => client.EmailOutgoing.SendEmailUsingTemplate(new OutgoingEmailTemplateCreateInfo(
                templateId: new EmailTemplateId("bf6e3dee-92b3-482a-b45a-991ae61bba1c"),
                from: new EmailSender(emailAddress: "support@inmobile.com", displayName: "X from inMobile"),
                to: new List<EmailRecipient> { new EmailRecipient(emailAddress: "test@inmobile.com", displayName: "Test") })));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ErrorHttpStatusCode);
        }
    }
    
    [Fact]
    public async Task SendEmailUsingTemplateAsync_ApiError_Test()
    {
        var expectedRequestJson = @"{""From"":{""EmailAddress"":""support@inmobile.com"",""DisplayName"":""X from inMobile""},""To"":[{""EmailAddress"":""test@inmobile.com"",""DisplayName"":""Test""}],""Tracking"":true,""TemplateId"":""bf6e3dee-92b3-482a-b45a-991ae61bba1c""}";
        var responseJson = @"{
    ""errorMessage"": ""Domain not validated"",
    ""details"": []
}";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/email/outgoing/sendusingtemplate", jsonOrNull: expectedRequestJson);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "400 BadRequest");
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var ex = await Assert.ThrowsAsync<InMobileApiException>(() => client.EmailOutgoing.SendEmailUsingTemplateAsync(new OutgoingEmailTemplateCreateInfo(
                templateId: new EmailTemplateId("bf6e3dee-92b3-482a-b45a-991ae61bba1c"),
                from: new EmailSender(emailAddress: "support@inmobile.com", displayName: "X from inMobile"),
                to: new List<EmailRecipient> { new EmailRecipient(emailAddress: "test@inmobile.com", displayName: "Test") })));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ErrorHttpStatusCode);
        }
    }
}