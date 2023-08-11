using System;
using System.Net;
using System.Reflection.Metadata;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.List.Recipients
{
    public class CreateRecipient_Integration_Test
    {
        [Fact]
        public void CreateRecipient_Test()
        {
            var requestJson = @"{""NumberInfo"":{""CountryCode"":""33"",""PhoneNumber"":""111111""},""Fields"":{""Email"":""some@email.com""}}";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/lists/some_list_id/recipients", jsonOrNull: requestJson);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull:
                @"{
                    ""externalCreated"": ""2019-08-24T14:15:22Z"",
                    ""numberInfo"": {
                        ""countryCode"": ""33"",
                        ""phoneNumber"": ""111111""
                    },
                    ""fields"": {
                        ""email"": ""some@email.com"",
                        ""firstname"": null
                    },
                    ""id"": ""some_new_id"",
                    ""listId"": ""some_list_id"",
                    ""created"": ""2019-08-20T11:15:22Z""
                }", statusCodeString: "200 Ok");

            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");

                var recipient = new RecipientCreateInfo(listId: new RecipientListId("some_list_id"), new NumberInfo(countryCode: "33", phoneNumber: "111111"));
                recipient.Fields.Add("Email", "some@email.com");

                var resultRecipient = client.Lists.CreateRecipient(recipient: recipient);
                Assert.Equal("some@email.com", resultRecipient.Fields["email"]);
                Assert.Null(resultRecipient.Fields["firstname"]);
                Assert.True(resultRecipient.ExternalCreated.HasValue);
                Assert.Equal(DateTimeKind.Utc, resultRecipient.ExternalCreated.Value.Kind);
                Assert.Equal(new DateTime(2019, 08, 24, 14, 15, 22, DateTimeKind.Utc), resultRecipient.ExternalCreated.Value);
                Assert.Equal(new DateTime(2019, 08, 20, 11, 15, 22, DateTimeKind.Utc), resultRecipient.Created);
                Assert.Equal("33", resultRecipient.NumberInfo.CountryCode);
                Assert.Equal("111111", resultRecipient.NumberInfo.PhoneNumber);
                Assert.Equal("some_new_id", resultRecipient.Id.Value);
                Assert.Equal("some_list_id", resultRecipient.ListId.Value);
                server.AssertNoAwaitingRequestsLeft();
            }
        }

        [Fact]
        public void CreateRecipient_WithExternalCreated_Test()
        {
            var requestJson = @"{""NumberInfo"":{""CountryCode"":""33"",""PhoneNumber"":""111111""},""Fields"":{""Email"":""some@email.com""},""ExternalCreated"":""2019-08-24T14:15:22Z""}";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/lists/some_list_id/recipients", jsonOrNull: requestJson);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull:
                @"{
                    ""externalCreated"": ""2019-08-24T14:15:22Z"",
                    ""numberInfo"": {
                        ""countryCode"": ""33"",
                        ""phoneNumber"": ""111111""
                    },
                    ""fields"": {
                        ""email"": ""some@email.com"",
                        ""firstname"": null
                    },
                    ""id"": ""some_new_id"",
                    ""listId"": ""some_list_id"",
                    ""created"": ""2019-08-20T11:15:22Z""
                }", statusCodeString: "200 Ok");

            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");

                var recipient = new RecipientCreateInfo(
                    listId: new RecipientListId("some_list_id"), 
                    numberInfo: new NumberInfo(countryCode: "33", phoneNumber: "111111"),
                    externalCreated: new DateTime(2019, 08, 24, 14, 15, 22, DateTimeKind.Utc));
                recipient.Fields.Add("Email", "some@email.com");

                var resultRecipient = client.Lists.CreateRecipient(recipient: recipient);
                Assert.Equal("some@email.com", resultRecipient.Fields["email"]);
                Assert.Null(resultRecipient.Fields["firstname"]);
                Assert.True(resultRecipient.ExternalCreated.HasValue);
                Assert.Equal(DateTimeKind.Utc, resultRecipient.ExternalCreated.Value.Kind);
                Assert.Equal(new DateTime(2019, 08, 24, 14, 15, 22, DateTimeKind.Utc), resultRecipient.ExternalCreated.Value);
                Assert.Equal(new DateTime(2019, 08, 20, 11, 15, 22, DateTimeKind.Utc), resultRecipient.Created);
                Assert.Equal("33", resultRecipient.NumberInfo.CountryCode);
                Assert.Equal("111111", resultRecipient.NumberInfo.PhoneNumber);
                Assert.Equal("some_new_id", resultRecipient.Id.Value);
                Assert.Equal("some_list_id", resultRecipient.ListId.Value);
                server.AssertNoAwaitingRequestsLeft();
            }
        }

        [Fact]
        public void CreateRecipient_WithExternalCreatedUnspecifiedDateTimeKind_Test()
        {
            var apiKey = new InMobileApiKey("UnitTestKey123");
            
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort())
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");

                try
                {
                    var recipient = new RecipientCreateInfo(
                        listId: new RecipientListId("some_list_id"),
                        numberInfo: new NumberInfo(countryCode: "33", phoneNumber: "111111"),
                        externalCreated: new DateTime(2019, 08, 24, 14, 15, 22, DateTimeKind.Unspecified)); // Unspecified is the default behavior on a new DateTime

                    client.Lists.CreateRecipient(recipient: recipient);
                }
                catch (ArgumentException aex)
                {
                    Assert.Equal("externalCreated", aex.ParamName);
                    Assert.StartsWith("DateTimes with Unspecified Kind is not allowed", aex.Message);
                }

                server.AssertNoAwaitingRequestsLeft();
            }
        }

        [Fact]
        public void CreateRecipient_ApiError_Test()
        {
            var requestJson = @"{""NumberInfo"":{""CountryCode"":""33"",""PhoneNumber"":""111111""},""Fields"":{""Email"":""some@email.com""}}";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/lists/some_list_id/recipients", jsonOrNull: requestJson);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull:
                @"{
""errorMessage"": ""Forbidden thing"",
""details"": [
""You shall not pass"",
""Go away""
]
}", statusCodeString: "500 ServerError");

            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");

                var recipient = new RecipientCreateInfo(listId: new RecipientListId("some_list_id"), new NumberInfo(countryCode: "33", phoneNumber: "111111"));
                recipient.Fields.Add("Email", "some@email.com");

                var ex = Assert.Throws<InMobileApiException>(() => client.Lists.CreateRecipient(recipient: recipient));

                Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
            }
        }
    }
}
