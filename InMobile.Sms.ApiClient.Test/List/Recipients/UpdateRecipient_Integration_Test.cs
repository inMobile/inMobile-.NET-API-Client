﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.List.Recipients
{
    public class UpdateRecipient_Integration_Test
    {
        [Fact]
        public void UpdateRecipient_Test()
        {
            var requestJson = @"{""Id"":null,""ListId"":null,""NumberInfo"":{""CountryCode"":""33"",""PhoneNumber"":""111111""},""Fields"":{""Email"":""some@email.com""}}";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "PUT /v4/lists/some_list_id/recipients/recId1", jsonOrNull: requestJson);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull:
                @"{
                    ""externalCreated"": {
                        ""utcTime"": ""2019-08-24T14:15:22Z"",
                        ""localServerTime"": ""2019-08-24T14:15:22Z""
                    },
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
                    ""created"": {
                        ""utcTime"": ""2019-08-24T14:15:22Z"",
                        ""localServerTime"": ""2019-08-24T14:15:22Z""
                    }
                }", statusCodeString: "200 Ok");

            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");

                var recipient = new Recipient(new NumberInfo(countryCode: "33", phoneNumber: "111111"));
                recipient.Fields.Add("Email", "some@email.com");

                var resultRecipient = client.Lists.UpdateRecipient(listId: "some_list_id", recipientId: "recId1", updateObject: recipient);
                Assert.Equal("some@email.com", resultRecipient.Fields["email"]);
                Assert.Null(resultRecipient.Fields["firstname"]);
                Assert.Equal("33", resultRecipient.NumberInfo.CountryCode);
                Assert.Equal("111111", resultRecipient.NumberInfo.PhoneNumber);
                Assert.Equal("some_new_id", resultRecipient.Id);
                Assert.Equal("some_list_id", resultRecipient.ListId);
                server.AssertNoAwaitingRequestsLeft();
            }
        }
    }
}