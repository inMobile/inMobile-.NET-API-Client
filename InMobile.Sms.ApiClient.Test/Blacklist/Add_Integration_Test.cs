using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.Blacklist
{
    public class Add_Integration_Test
    {
        [Fact]
        public void Add_Test()
        {
            var requestJson = @"{""numberInfo"":{""countryCode"":""45"",""phoneNumber"":""12345678""},""comment"":""Sometextprovidedwhencreated""}";
            var responseJson = @"{                
                ""numberInfo"": {
                    ""countryCode"": ""45"",
                    ""phoneNumber"": ""12345678""
                },
                ""comment"": ""Sometextprovidedwhencreated"",
                ""id"": ""some_blacklist_id""
            }";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/blacklist", jsonOrNull: requestJson);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var entry = client.Blacklist.Add(countryCode: "45", number: "12345678", comment: "Sometextprovidedwhencreated");
                Assert.Equal("45", entry.NumberInfo.CountryCode);
                Assert.Equal("12345678", entry.NumberInfo.PhoneNumber);
                Assert.Equal("Sometextprovidedwhencreated", entry.Comment);
                Assert.Equal("some_blacklist_id", entry.Id);
            }
        }
    }
}
