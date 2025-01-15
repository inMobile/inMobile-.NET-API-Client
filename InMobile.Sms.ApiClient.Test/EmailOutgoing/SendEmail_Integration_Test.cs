using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.EmailOutgoing;

public class SendEmail_Integration_Test
{
    [Fact]
    public void SendEmail_WithOnlyRequiredFields_Success_Test()
    {
        var expectedRequestJson = @"{""From"":{""EmailAddress"":""support@inmobile.com"",""DisplayName"":""X from inMobile""},""To"":[{""EmailAddress"":""test@inmobile.com"",""DisplayName"":""Test""}],""Tracking"":true,""Subject"":""This is a subject!"",""Html"":""<!DOCTYPE html><html><head></head><body><p>This is my HTML</p></body></html>""}";
        var responseJson = @"{
    ""messageId"": ""0448b2aa-490b-4bbc-92de-2eb5c4f93d68"",
    ""to"": [
        {
            ""emailAddress"": ""test@inmobile.com"",
            ""displayName"": ""Test""
        }
    ]
}";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/email/outgoing", jsonOrNull: expectedRequestJson);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var response = client.EmailOutgoing.SendEmail(new OutgoingEmailCreateInfo(
                subject: "This is a subject!",
                html: "<!DOCTYPE html><html><head></head><body><p>This is my HTML</p></body></html>",
                from: new EmailSender(emailAddress: "support@inmobile.com", displayName: "X from inMobile"),
                to: new List<EmailRecipient> { new EmailRecipient(emailAddress: "test@inmobile.com", displayName: "Test") }));
            Assert.NotNull(response);
            Assert.Equal(new OutgoingEmailId("0448b2aa-490b-4bbc-92de-2eb5c4f93d68"), response.MessageId);
            Assert.Single(response.To);
            var singleRecipient = response.To.Single();
            Assert.Equal("Test", singleRecipient.DisplayName);
            Assert.Equal("test@inmobile.com", singleRecipient.EmailAddress);
        }
    }
    
    [Fact]
    public async Task SendEmailAsync_WithOnlyRequiredFields_Success_Test()
    {
        var expectedRequestJson = @"{""From"":{""EmailAddress"":""support@inmobile.com"",""DisplayName"":""X from inMobile""},""To"":[{""EmailAddress"":""test@inmobile.com"",""DisplayName"":""Test""}],""Tracking"":true,""Subject"":""This is a subject!"",""Html"":""<!DOCTYPE html><html><head></head><body><p>This is my HTML</p></body></html>""}";
        var responseJson = @"{
    ""messageId"": ""0448b2aa-490b-4bbc-92de-2eb5c4f93d68"",
    ""to"": [
        {
            ""emailAddress"": ""test@inmobile.com"",
            ""displayName"": ""Test""
        }
    ]
}";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/email/outgoing", jsonOrNull: expectedRequestJson);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var response = await client.EmailOutgoing.SendEmailAsync(new OutgoingEmailCreateInfo(
                subject: "This is a subject!",
                html: "<!DOCTYPE html><html><head></head><body><p>This is my HTML</p></body></html>",
                from: new EmailSender(emailAddress: "support@inmobile.com", displayName: "X from inMobile"),
                to: new List<EmailRecipient> { new EmailRecipient(emailAddress: "test@inmobile.com", displayName: "Test") }));
            Assert.NotNull(response);
            Assert.Equal(new OutgoingEmailId("0448b2aa-490b-4bbc-92de-2eb5c4f93d68"), response.MessageId);
            Assert.Single(response.To);
            var singleRecipient = response.To.Single();
            Assert.Equal("Test", singleRecipient.DisplayName);
            Assert.Equal("test@inmobile.com", singleRecipient.EmailAddress);
        }
    }

    [Fact]
    public void SendEmail_WithAllFields_Success_Test()
    {
        var expectedRequestJson = @"{""From"":{""EmailAddress"":""noreply@inmobile.com"",""DisplayName"":""X from inMobile""},""To"":[{""EmailAddress"":""test@inmobile.com"",""DisplayName"":""Test""}],""ReplyTo"":[{""EmailAddress"":""support@inmobile.com""}],""MessageId"":""558c8a34-531d-4ad4-8580-22b554ed00c1"",""SendTime"":""2020-10-10T16:13:00Z"",""Tracking"":false,""Subject"":""This is a subject!"",""Html"":""<!DOCTYPE html><html><head></head><body><p>This is my HTML</p></body></html>"",""Text"":""Text edition""}";
        var responseJson = @"{
    ""messageId"": ""558c8a34-531d-4ad4-8580-22b554ed00c1"",
    ""to"": [
        {
            ""emailAddress"": ""test@inmobile.com"",
            ""displayName"": ""Test""
        }
    ]
}";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/email/outgoing", jsonOrNull: expectedRequestJson);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var response = client.EmailOutgoing.SendEmail(new OutgoingEmailCreateInfo(
                subject: "This is a subject!",
                html: "<!DOCTYPE html><html><head></head><body><p>This is my HTML</p></body></html>",
                from: new EmailSender(emailAddress: "noreply@inmobile.com", displayName: "X from inMobile"),
                to: new List<EmailRecipient> { new EmailRecipient(emailAddress: "test@inmobile.com", displayName: "Test") },
                replyTo: new List<EmailReplyToRecipient> { new EmailReplyToRecipient(emailAddress: "support@inmobile.com") },
                text: "Text edition",
                sendTime: new DateTime(2020, 10, 10, 16, 13, 0, kind: DateTimeKind.Utc),
                tracking: false,
                messageId: new OutgoingEmailId("558c8a34-531d-4ad4-8580-22b554ed00c1")));
            Assert.NotNull(response);
            Assert.Equal(new OutgoingEmailId("558c8a34-531d-4ad4-8580-22b554ed00c1"), response.MessageId);
            Assert.Single(response.To);
            var singleRecipient = response.To.Single();
            Assert.Equal("Test", singleRecipient.DisplayName);
            Assert.Equal("test@inmobile.com", singleRecipient.EmailAddress);
        }
    }
    
    [Fact]
    public async Task SendEmailAsync_WithAllFields_Success_Test()
    {
        var expectedRequestJson = @"{""From"":{""EmailAddress"":""noreply@inmobile.com"",""DisplayName"":""X from inMobile""},""To"":[{""EmailAddress"":""test@inmobile.com"",""DisplayName"":""Test""}],""ReplyTo"":[{""EmailAddress"":""support@inmobile.com""}],""MessageId"":""558c8a34-531d-4ad4-8580-22b554ed00c1"",""SendTime"":""2020-10-10T16:13:00Z"",""Tracking"":false,""Subject"":""This is a subject!"",""Html"":""<!DOCTYPE html><html><head></head><body><p>This is my HTML</p></body></html>"",""Text"":""Text edition""}";
        var responseJson = @"{
    ""messageId"": ""558c8a34-531d-4ad4-8580-22b554ed00c1"",
    ""to"": [
        {
            ""emailAddress"": ""test@inmobile.com"",
            ""displayName"": ""Test""
        }
    ]
}";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/email/outgoing", jsonOrNull: expectedRequestJson);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var response = await client.EmailOutgoing.SendEmailAsync(new OutgoingEmailCreateInfo(
                subject: "This is a subject!",
                html: "<!DOCTYPE html><html><head></head><body><p>This is my HTML</p></body></html>",
                from: new EmailSender(emailAddress: "noreply@inmobile.com", displayName: "X from inMobile"),
                to: new List<EmailRecipient> { new EmailRecipient(emailAddress: "test@inmobile.com", displayName: "Test") },
                replyTo: new List<EmailReplyToRecipient> { new EmailReplyToRecipient(emailAddress: "support@inmobile.com") },
                text: "Text edition",
                sendTime: new DateTime(2020, 10, 10, 16, 13, 0, kind: DateTimeKind.Utc),
                tracking: false,
                messageId: new OutgoingEmailId("558c8a34-531d-4ad4-8580-22b554ed00c1")));
            Assert.NotNull(response);
            Assert.Equal(new OutgoingEmailId("558c8a34-531d-4ad4-8580-22b554ed00c1"), response.MessageId);
            Assert.Single(response.To);
            var singleRecipient = response.To.Single();
            Assert.Equal("Test", singleRecipient.DisplayName);
            Assert.Equal("test@inmobile.com", singleRecipient.EmailAddress);
        }
    }

    [Fact]
    public void SendEmail_ApiError_Test()
    {
        var expectedRequestJson = @"{""From"":{""EmailAddress"":""support@inmobile.com"",""DisplayName"":""X from inMobile""},""To"":[{""EmailAddress"":""test@inmobile.com"",""DisplayName"":""Test""}],""Tracking"":true,""Subject"":""This is a subject!"",""Html"":""This is my HTML""}";
        var responseJson = @"{
    ""errorMessage"": ""Domain not validated"",
    ""details"": []
}";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/email/outgoing", jsonOrNull: expectedRequestJson);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "400 BadRequest");
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var ex = Assert.Throws<InMobileApiException>(() => client.EmailOutgoing.SendEmail(new OutgoingEmailCreateInfo(
                subject: "This is a subject!",
                html: "This is my HTML",
                from: new EmailSender(emailAddress: "support@inmobile.com", displayName: "X from inMobile"),
                to: new List<EmailRecipient> { new EmailRecipient(emailAddress: "test@inmobile.com", displayName: "Test") })));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ErrorHttpStatusCode);
        }
    }
    
    [Fact]
    public async Task SendEmailAsync_ApiError_Test()
    {
        var expectedRequestJson = @"{""From"":{""EmailAddress"":""support@inmobile.com"",""DisplayName"":""X from inMobile""},""To"":[{""EmailAddress"":""test@inmobile.com"",""DisplayName"":""Test""}],""Tracking"":true,""Subject"":""This is a subject!"",""Html"":""This is my HTML""}";
        var responseJson = @"{
    ""errorMessage"": ""Domain not validated"",
    ""details"": []
}";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/email/outgoing", jsonOrNull: expectedRequestJson);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "400 BadRequest");
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var ex = await Assert.ThrowsAsync<InMobileApiException>(() => client.EmailOutgoing.SendEmailAsync(new OutgoingEmailCreateInfo(
                subject: "This is a subject!",
                html: "This is my HTML",
                from: new EmailSender(emailAddress: "support@inmobile.com", displayName: "X from inMobile"),
                to: new List<EmailRecipient> { new EmailRecipient(emailAddress: "test@inmobile.com", displayName: "Test") })));

            Assert.Equal(HttpStatusCode.BadRequest, ex.ErrorHttpStatusCode);
        }
    }
}