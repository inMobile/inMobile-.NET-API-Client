﻿using System;
using Newtonsoft.Json;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test
{
    public class UpdateList_Integration_Test
    {
        [Fact]
        public void UpdateList_WithRecipientList_Test()
        {
            var responseJson = @"{                
                ""name"": ""New name"",
                ""id"": ""some_list_id""
            }";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "PUT /v4/lists/some_list_id", jsonOrNull: @"{""name"":""New name""}");
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");

                var list = JsonConvert.DeserializeObject<RecipientList>(@"{ ""Id"":""some_list_id"", ""Name"":""New name"" }");

                // Sanity check object prior to sending it
                Assert.Equal("some_list_id", list.Id.Value);
                Assert.Equal("New name", list.Name);

                // Execute
                var entry = client.Lists.UpdateList(list: list);
                Assert.Equal("New name", entry.Name);
                Assert.Equal("some_list_id", entry.Id.Value);
            }
        }

        [Fact]
        public void UpdateList_WithRecipientListUpdateInfo_Test()
        {
            var responseJson = @"{                
                ""name"": ""New name"",
                ""id"": ""some_list_id""
            }";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "PUT /v4/lists/some_list_id", jsonOrNull: @"{""name"":""New name""}");
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var entry = client.Lists.UpdateList(list: new RecipientListUpdateInfo(listId: new RecipientListId("some_list_id"), name: "New name"));
                Assert.Equal("New name", entry.Name);
                Assert.Equal("some_list_id", entry.Id.Value);
            }
        }
    }
}
