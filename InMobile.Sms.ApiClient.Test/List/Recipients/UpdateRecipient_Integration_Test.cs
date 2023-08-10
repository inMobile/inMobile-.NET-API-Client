using System.Net;
using Newtonsoft.Json;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.List.Recipients
{
    public class UpdateRecipient_Integration_Test
    {
        [Fact]
        public void UpdateRecipient_WithRecipientObject_Test()
        {
            var requestJson = @"{""NumberInfo"":{""CountryCode"":""33"",""PhoneNumber"":""111111""},""Fields"":{""Email"":""some@email.com""}}";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "PUT /v4/lists/some_list_id/recipients/recId1", jsonOrNull: requestJson);
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
                    ""created"": ""2019-08-24T14:15:22Z""
                }", statusCodeString: "200 Ok");

            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");

                // Use JsonConvert to instantiate the object as its constructor is empty. This is the least ugly solution compared to using reflection.
                var recipient = JsonConvert.DeserializeObject<Recipient>(value: @"{
                        ""Id"":""recId1"",
                        ""ListId"":""some_list_id"",
                        ""Fields"":{ ""Email"": ""some@email.com"" },
                        ""NumberInfo"":{""CountryCode"":""33"", ""PhoneNumber"":""111111""}
                    }
                ");
                // Sanity check the object prior to sending it
                Assert.Equal("recId1", recipient.Id.Value);
                Assert.Equal("some_list_id", recipient.ListId.Value);
                Assert.Single(recipient.Fields);
                Assert.Equal("some@email.com", recipient.Fields["Email"]);
                Assert.Equal("33", recipient.NumberInfo.CountryCode);
                Assert.Equal("111111", recipient.NumberInfo.PhoneNumber);

                // Execute
                var resultRecipient = client.Lists.UpdateRecipient(recipient: recipient);

                // Assert
                Assert.Equal("some@email.com", resultRecipient.Fields["email"]);
                Assert.Null(resultRecipient.Fields["firstname"]);
                Assert.Equal("33", resultRecipient.NumberInfo.CountryCode);
                Assert.Equal("111111", resultRecipient.NumberInfo.PhoneNumber);
                Assert.Equal("some_new_id", resultRecipient.Id.Value);
                Assert.Equal("some_list_id", resultRecipient.ListId.Value);
                server.AssertNoAwaitingRequestsLeft();
            }
        }

        [Fact]
        public void UpdateRecipient_WithRecipientUpdateObject_Test()
        {
            var requestJson = @"{""NumberInfo"":{""CountryCode"":""33"",""PhoneNumber"":""111111""},""Fields"":{""Email"":""some@email.com""}}";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "PUT /v4/lists/some_list_id/recipients/recId1", jsonOrNull: requestJson);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull:
                @"{
                    ""externalCreated"": null,
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
                    ""created"": ""2019-08-24T14:15:22Z""
                }", statusCodeString: "200 Ok");

            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");

                var recipient = new RecipientUpdateInfo(recipientId: new RecipientId("recId1"), listId: new RecipientListId("some_list_id"), new NumberInfo(countryCode: "33", phoneNumber: "111111"));
                recipient.Fields.Add("Email", "some@email.com");

                var resultRecipient = client.Lists.UpdateRecipient(recipient: recipient);
                Assert.Equal("some@email.com", resultRecipient.Fields["email"]);
                Assert.Null(resultRecipient.Fields["firstname"]);
                Assert.False(resultRecipient.ExternalCreated.HasValue);
                Assert.Equal("33", resultRecipient.NumberInfo.CountryCode);
                Assert.Equal("111111", resultRecipient.NumberInfo.PhoneNumber);
                Assert.Equal("some_new_id", resultRecipient.Id.Value);
                Assert.Equal("some_list_id", resultRecipient.ListId.Value);
                server.AssertNoAwaitingRequestsLeft();
            }
        }

        [Fact]
        public void UpdateRecipient_ApiError_WithRecipientObject_Test()
        {
            var requestJson = @"{""NumberInfo"":{""CountryCode"":""33"",""PhoneNumber"":""111111""},""Fields"":{""Email"":""some@email.com""}}";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "PUT /v4/lists/some_list_id/recipients/recId1", jsonOrNull: requestJson);
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

                // Use JsonConvert to instantiate the object as its constructor is empty. This is the least ugly solution compared to using reflection.
                var recipient = JsonConvert.DeserializeObject<Recipient>(value: @"{
                        ""Id"":""recId1"",
                        ""ListId"":""some_list_id"",
                        ""Fields"":{ ""Email"": ""some@email.com"" },
                        ""NumberInfo"":{""CountryCode"":""33"", ""PhoneNumber"":""111111""}
                    }
                ");
                // Sanity check the object prior to sending it
                Assert.Equal("recId1", recipient.Id.Value);
                Assert.Equal("some_list_id", recipient.ListId.Value);
                Assert.Single(recipient.Fields);
                Assert.Equal("some@email.com", recipient.Fields["Email"]);
                Assert.Equal("33", recipient.NumberInfo.CountryCode);
                Assert.Equal("111111", recipient.NumberInfo.PhoneNumber);

                var ex = Assert.Throws<InMobileApiException>(() => client.Lists.UpdateRecipient(recipient: recipient));
                Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
            }
        }

        [Fact]
        public void UpdateRecipient_ApiError_WithRecipientUpdateObject_Test()
        {
            var requestJson = @"{""NumberInfo"":{""CountryCode"":""33"",""PhoneNumber"":""111111""},""Fields"":{""Email"":""some@email.com""}}";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "PUT /v4/lists/some_list_id/recipients/recId1", jsonOrNull: requestJson);
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

                var recipient = new RecipientUpdateInfo(recipientId: new RecipientId("recId1"), listId: new RecipientListId("some_list_id"), new NumberInfo(countryCode: "33", phoneNumber: "111111"));
                recipient.Fields.Add("Email", "some@email.com");

                var ex = Assert.Throws<InMobileApiException>(() => client.Lists.UpdateRecipient(recipient: recipient));
                Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
            }
        }
    }
}
