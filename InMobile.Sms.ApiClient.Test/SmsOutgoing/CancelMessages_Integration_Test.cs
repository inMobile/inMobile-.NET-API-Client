using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.SmsOutgoing
{
    public class CancelMessages_Integration_Test
    {
        [Fact]
        public void CancelMessages_Success_Test()
        {
            var expectedRequestJson = @"{""MessageIds"":[""id1"",""id2"",""id3""]}";
            var responseJson = @"{
                        ""results"": [
                            {
                                ""messageId"": ""id1"",
                                ""resultCode"": 1,
                                ""resultDescription"": ""Success""
                            },
                            {
                                ""messageId"": ""id2"",
                                ""resultCode"": -1,
                                ""resultDescription"": ""Not cancelled""
                            },
                            {
                                ""messageId"": ""id3"",
                                ""resultCode"": -2,
                                ""resultDescription"": ""Message not found"",
                                ""future_field_not_yet_known"": ""Hello""
                            }
                        ],
                        ""future_field_not_yet_known"": ""Hello""}";
                
            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/sms/outgoing/cancel", jsonOrNull: expectedRequestJson);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
                var response = client.SmsOutgoing.CancelMessages(messageIds: new List<OutgoingMessageId>() { new OutgoingMessageId("id1"), new OutgoingMessageId("id2"), new OutgoingMessageId("id3") });
                Assert.NotNull(response);
                Assert.Equal(3, response.Results.Count);

                {
                    Assert.Equal("id1", response.Results[0].MessageId.Value);
                    Assert.Equal(CancelResultCode.Success, response.Results[0].ResultCode);
                    Assert.Equal("Success", response.Results[0].ResultDescription);
                }

                {
                    Assert.Equal("id2", response.Results[1].MessageId.Value);
                    Assert.Equal(CancelResultCode.NotCancelled, response.Results[1].ResultCode);
                    Assert.Equal("Not cancelled", response.Results[1].ResultDescription);
                }

                {
                    Assert.Equal("id3", response.Results[2].MessageId.Value);
                    Assert.Equal(CancelResultCode.MessageNotFound, response.Results[2].ResultCode);
                    Assert.Equal("Message not found", response.Results[2].ResultDescription);
                }
            }
        }

        [Fact]
        public void CancelMessages_ApiError_Test()
        {
            var expectedRequestJson = @"{""MessageIds"":[""id1"",""id2"",""id3""]}";
            var responseJson = @"{
""errorMessage"": ""Forbidden thing"",
""details"": [
""You shall not pass"",
""Go away""
]
}";

            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/sms/outgoing/cancel", jsonOrNull: expectedRequestJson);
            var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "500 ServerError");
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
            {
                var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");

                var ex = Assert.Throws<InMobileApiException>(() => client.SmsOutgoing.CancelMessages(messageIds: new List<OutgoingMessageId>() { new OutgoingMessageId("id1"), new OutgoingMessageId("id2"), new OutgoingMessageId("id3") }));
                Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
            }
        }

    }
}

