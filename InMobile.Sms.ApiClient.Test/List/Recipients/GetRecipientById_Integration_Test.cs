using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.List.Recipients
{
    public class GetRecipientById_Integration_Test
    {
        [Fact]
        public void GetRecipientById_Test()
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
                                    ""listId"": ""some_list_id"",
                                    ""future_field_not_yet_known"": ""Hello""
                                }";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/some_list_id/recipients/recId1", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var recipient = client.Lists.GetRecipientById(listId: new RecipientListId("some_list_id"), recipientId: new RecipientId("recId1"));
                Assert.Equal("recId1", recipient.Id.Value);
                Assert.Equal("45", recipient.NumberInfo.CountryCode);
                Assert.Equal("1111", recipient.NumberInfo.PhoneNumber);
                Assert.Equal("some_list_id", recipient.ListId.Value);
                Assert.Equal("Mr", recipient.Fields["firstname"]);
                Assert.Equal("Anderson", recipient.Fields["lastname"]);
            }
        }

        [Fact]
        public void GetRecipientById_ApiError_Test()
        {
            var responseJson = @"{
""errorMessage"": ""Forbidden thing"",
""details"": [
""You shall not pass"",
""Go away""
]
}";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/some_list_id/recipients/rec_id", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "500 ServerError");
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");

                var ex = Assert.Throws<InMobileApiException>(() => client.Lists.GetRecipientById(listId: new RecipientListId("some_list_id"), recipientId: new RecipientId("rec_id")));
                Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
            }
        }
    }

}
