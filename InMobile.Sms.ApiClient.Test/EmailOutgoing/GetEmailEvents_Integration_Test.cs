using System;
using System.Linq;
using System.Net;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.EmailOutgoing
{
    public class GetEmailEvents_Integration_Test
    {
        [Fact]
        public void GetEmailEvents_Success_Test()
        {
            var responseJson = @"{
    ""events"": [
        {
            ""messageId"": ""3baba088-4029-49f7-b90d-1e44b56e36c6"",
            ""eventType"": 3,
            ""eventTypeDescription"": ""Delivered"",
            ""eventTimestamp"": ""2001-02-22T14:50:23Z""
        }
]}";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/email/outgoing/events?limit=10", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var response = client.EmailOutgoing.GetEmailEvents(limit: 10);
                Assert.Single(response.Events);
                var singleEvent = response.Events.Single();
                Assert.NotNull(singleEvent);
                Assert.Equal(new OutgoingEmailId("3baba088-4029-49f7-b90d-1e44b56e36c6"), singleEvent.MessageId);
                Assert.Equal(EmailEventType.Delivered, singleEvent.EventType);
                Assert.Equal("Delivered", singleEvent.EventTypeDescription);
                Assert.Equal(new DateTime(2001, 02, 22, 14, 50, 23, DateTimeKind.Utc), singleEvent.EventTimestamp);
            }
        }

        [Fact]
        public void GetEmailEvents_ApiError_Test()
        {
            var responseJson = @"{
    ""errorMessage"": ""Domain not validated"",
    ""details"": []
}";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/email/outgoing/events?limit=10", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "400 BadRequest");
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var ex = Assert.Throws<InMobileApiException>(() => client.EmailOutgoing.GetEmailEvents(limit: 10));

                Assert.Equal(HttpStatusCode.BadRequest, ex.ErrorHttpStatusCode);
            }
        }

        [Theory]
        [InlineData(-1000, false)]
        [InlineData(-1, false)]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(10, true)]
        [InlineData(249, true)]
        [InlineData(250, true)]
        [InlineData(251, false)]
        [InlineData(1000, false)]
        public void GetEmailEvents_Limit_Test(int limit, bool expectApiCalled)
        {
            var emptyResponseJson = @"{
    ""events"": [
]}";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: $"GET /v4/email/outgoing/events?limit={limit}", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: emptyResponseJson);
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                if (expectApiCalled)
                {
                    var response = client.EmailOutgoing.GetEmailEvents(limit: limit);
                    Assert.Empty(response.Events);
                }
                else
                {
                    Assert.Throws<ArgumentException>(() => client.EmailOutgoing.GetEmailEvents(limit: limit));
                }
            }
        }
    }
}
