using System;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.EmailOutgoing
{
    public class GetEmailEvents_Integration_Test
    {
        [Fact]
        public void GetEmailEvents_Success_Test()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void GetEmailEvents_ApiError_Test()
        {
            throw new NotImplementedException();
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
                    var events = client.EmailOutgoing.GetEmailEvents(limit: limit);
                    Assert.Empty(events.Events);
                }
                else
                {
                    Assert.Throws<ArgumentException>(() => client.EmailOutgoing.GetEmailEvents(limit: limit));
                }
            }
        }
    }
}
