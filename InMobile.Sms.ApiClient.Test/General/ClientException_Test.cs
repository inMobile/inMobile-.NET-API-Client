using System.Net;
using InMobile.Sms.ApiClient.Test.Tools;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.General
{
    public class ClientException_Test
    {
        [Fact]
        public void EnsureNetworkExceptionsAreVisible_Test()
        {
            // Get a port that nothing is listening on
            var unusedPort = LocalPortUtils.GetAndReserverAvailablePort();

            var client = new InMobileApiClient(new InMobileApiKey("Some_Key"), baseUrl: $"http://localhost:{unusedPort}");
            Assert.Throws<WebException>(() => client.SmsOutgoing.GetStatusReports());
        }

        [Theory]
        [InlineData("400 BadRequest", 400)]
        [InlineData("401 Unauthorized", 401)]
        [InlineData("404 NotFound", 404)]
        [InlineData("409 Conflict", 409)]
        [InlineData("500 InternalServerError", 500)]
        public void InMobileApiErrorReturned_Test(string statusCodeString, int expectedStatusCode)
        {
            var responseJson = @"{
""errorMessage"": ""Forbidden thing"",
""details"": [
""You shall not pass"",
""Go away""
]
}";
            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/sms/outgoing/reports", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: statusCodeString);
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var exceptionThrown = Assert.Throws<InMobileApiException>(() => client.SmsOutgoing.GetStatusReports());
                Assert.Equal(expectedStatusCode, (int)exceptionThrown.ErrorHttpStatusCode);
                Assert.Equal($"{statusCodeString}: Forbidden thing. You shall not pass; Go away", exceptionThrown.Message);
            }
        }
    }
}
