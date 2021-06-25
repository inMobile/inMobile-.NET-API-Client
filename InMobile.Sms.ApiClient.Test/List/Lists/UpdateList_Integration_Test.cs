using System;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.Blacklist
{
    public class UpdateList_Integration_Test
    {
        [Fact]
        public void UpdateList_Test()
        {
            var responseJson = @"{                
                ""name"": ""New name"",
                ""id"": ""some_list_id""
            }";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "PUT /v4/lists/some_list_id", jsonOrNull: @"{""name"":""New name""}");
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var entry = client.Lists.UpdateList(list: new RecipientList() { Name = "New name", ListId = "some_list_id" });
                Assert.Equal("New name", entry.Name);
                Assert.Equal("some_list_id", entry.ListId);
            }
        }
    }
}
