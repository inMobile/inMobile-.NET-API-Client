using System;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.Blacklist
{
    public class UpdateList_Integration_Test
    {
        [Theory]
        [InlineData(null, @"{""Id"":null,""Name"":""New name""}")]
        [InlineData(@"8c599617-e255-4179-9911-fdc0865e85f9", @"{""Id"":""8c599617-e255-4179-9911-fdc0865e85f9"",""Name"":""New name""}")]
        public void UpdateList_Test(string listIdOnupdateObject, string expectedRequestJson)
        {
            var responseJson = @"{                
                ""name"": ""New name"",
                ""Id"": ""some_list_id""
            }";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "PUT /v4/lists/some_list_id", jsonOrNull: expectedRequestJson);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var entry = client.Lists.UpdateList(listId: "some_list_id", updateObject: new ListEntry() { Name = "New name", Id = listIdOnupdateObject });
                Assert.Equal("New name", entry.Name);
                Assert.Equal("some_list_id", entry.Id);
            }
        }
    }
}
