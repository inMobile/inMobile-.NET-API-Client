using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.Blacklist
{
    public class CreateList_Integration_Test
    {
        [Fact]
        public void CreateList_Test()
        {
            var requestJson = @"{""name"":""New list name""}";
            var responseJson = @"{                
                ""name"": ""New list name"",
                ""Id"": ""SomeId123""
            }";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/lists", jsonOrNull: requestJson);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var entry = client.Lists.CreateList(name: "New list name");
                Assert.Equal("New list name", entry.Name);
                Assert.Equal("SomeId123", entry.Id);
            }
        }
    }
}
