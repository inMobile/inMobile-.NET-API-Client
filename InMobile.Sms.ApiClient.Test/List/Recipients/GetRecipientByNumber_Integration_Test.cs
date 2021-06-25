using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.List.Recipients
{
    public class GetRecipientByNumber_Integration_Test
    {
        [Fact]
        public void GetRecipientByNumber_Test()
        {
            var responseJson = @"{
                                    ""numberInfo"": {
                                        ""countryCode"": ""45"",
                                        ""phoneNumber"": ""1111""
                                    },
                                    ""fields"": {
                                        ""firstname"": ""Mr"",
                                        ""lastname"": ""Anderson""
                                    },
                                    ""id"": ""recId1"",
                                    ""listId"": ""some_list_id""
                                }";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/some_list_id/recipients/bynumber?countryCode=45&phoneNumber=1111", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var recipient = client.Lists.GetRecipientByNumber(listId: "some_list_id", countryCode: "45", phoneNumber: "1111");
                Assert.Equal("recId1", recipient.Id);
                Assert.Equal("45", recipient.NumberInfo.CountryCode);
                Assert.Equal("1111", recipient.NumberInfo.PhoneNumber);
                Assert.Equal("some_list_id", recipient.ListId);
                Assert.Equal("Mr", recipient.Fields["firstname"]);
                Assert.Equal("Anderson", recipient.Fields["lastname"]);
            }
        }
    }
}
