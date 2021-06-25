using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.List
{
    public class GetListById_Integration_Test
    {
        [Fact]
        public void GetListById_Test()
        {
            var responseJson = @"{                
                ""name"": ""Some list name"",
                ""Id"": ""some_list_id"",
                ""future_field_not_yet_known"": ""Hello""
            }";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/some_list_id", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var entry = client.Lists.GetListById(listId: "some_list_id");
                Assert.Equal("Some list name", entry.Name);
                Assert.Equal("some_list_id", entry.ListId);
            }
        }
    }
}
