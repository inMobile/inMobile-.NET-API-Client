using System;
using System.Net;
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
                                    ""externalCreated"": ""2001-02-10T14:50:23Z"",
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
                                    ""created"": ""2002-02-25T14:50:23Z"",
                                    ""future_field_not_yet_known"": ""Hello""
                                }";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/some_list_id/recipients/bynumber?countryCode=45&phoneNumber=1111", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var recipient = client.Lists.GetRecipientByNumber(listId: new RecipientListId("some_list_id"), numberInfo: new NumberInfo(countryCode: "45", phoneNumber: "1111"));

                Assert.Equal("recId1", recipient.Id.Value);
                Assert.True(recipient.ExternalCreated.HasValue);
                Assert.Equal(DateTimeKind.Utc, recipient.ExternalCreated.Value.Kind);
                Assert.Equal(new DateTime(2001, 02, 10, 14, 50, 23, DateTimeKind.Utc), recipient.ExternalCreated.Value);
                Assert.Equal(new DateTime(2002, 02, 25, 14, 50, 23, DateTimeKind.Utc), recipient.Created);
                Assert.Equal("45", recipient.NumberInfo.CountryCode);
                Assert.Equal("1111", recipient.NumberInfo.PhoneNumber);
                Assert.Equal("some_list_id", recipient.ListId.Value);
                Assert.Equal("Mr", recipient.Fields["firstname"]);
                Assert.Equal("Anderson", recipient.Fields["lastname"]);
            }
        }

        [Fact]
        public void GetRecipientByNumber_ApiError_NotFound_Test()
        {
            // TODO: What to do in NotFound case?
            throw new NotImplementedException();
        }

        [Fact]
        public void GetRecipientByNumber_ApiError_InternalServerError_Test()
        {
            var responseJson = @"{
                ""errorMessage"": ""Forbidden thing"",
                ""details"": [
                    ""You shall not pass"",
                    ""Go away""
                ]
            }";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/lists/some_list_id/recipients/bynumber?countryCode=45&phoneNumber=1111", jsonOrNull: null);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "500 ServerError");
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var ex = Assert.Throws<InMobileApiException>(() => client.Lists.GetRecipientByNumber(listId: new RecipientListId("some_list_id"), numberInfo: new NumberInfo(countryCode: "45", phoneNumber: "1111")));

                Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
            }
        }
    }
}
