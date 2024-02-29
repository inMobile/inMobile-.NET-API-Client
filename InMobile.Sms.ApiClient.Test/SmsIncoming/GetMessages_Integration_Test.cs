using System;
using System.Linq;
using System.Net;
using Xunit;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;

namespace InMobile.Sms.ApiClient.Test.SmsIncoming;

public class GetMessages_Integration_Test
{
    [Fact]
    public void GetMessages_Success_MsisdnSender_Test()
    {
        var responseJson = @"{
                    ""messages"": [
                    {
                        ""from"": {
                            ""countryCode"": ""45"",
                            ""phoneNumber"": ""11223344"",
                            ""rawSource"": ""45 11 22 33 44"",
                            ""isValidMsisdn"": true,
                        },
                        ""to"": {
                            ""countryCode"": ""46"",
                            ""phoneNumber"": ""88776655"",
                            ""msisdn"": ""4588776655"",
                        },
                        ""text"": ""Hello, World!"",
                        ""receivedAt"": ""2001-02-24T14:50:23Z"",
                        ""future_field_not_yet_known"": ""Hello""
                    }
                    ]}";
            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/sms/incoming/messages?limit=8", jsonOrNull: null);
            var responseToSendBack = new UnitTestResponseInfo(jsonOrNull: responseJson);
            
            using var server = StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendBack));
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var response = client.SmsIncoming.GetMessages(limit: 8);
            
            Assert.Multiple(
                () => Assert.NotNull(response),
                () => Assert.NotNull(response.Messages),
                () => Assert.Single(response.Messages!),
                () => Assert.Equal("45", response.Messages!.Single().From.CountryCode),
                () => Assert.Equal("11223344", response.Messages!.Single().From.PhoneNumber),
                () => Assert.Equal("45 11 22 33 44", response.Messages!.Single().From.RawSource),
                () => Assert.True(response.Messages!.Single().From.IsValidMsisdn),
                () => Assert.Equal("46", response.Messages!.Single().To.CountryCode),
                () => Assert.Equal("88776655", response.Messages!.Single().To.PhoneNumber),
                () => Assert.Equal("4588776655", response.Messages!.Single().To.Msisdn),
                () => Assert.Equal("Hello, World!", response.Messages!.Single().Text),
                () => Assert.Equal(new DateTime(2001, 2, 24, 14, 50, 23, DateTimeKind.Utc), response.Messages!.Single().ReceivedAt));
    }
    
    [Fact]
    public void GetMessages_Success_TextSender_Test()
    {
        var responseJson = @"{
                    ""messages"": [
                    {
                        ""from"": {
                            ""countryCode"": null,
                            ""phoneNumber"": null,
                            ""rawSource"": ""inMobile"",
                            ""isValidMsisdn"": false,
                        },
                        ""to"": {
                            ""countryCode"": ""46"",
                            ""phoneNumber"": ""88776655"",
                            ""msisdn"": ""4588776655"",
                        },
                        ""text"": ""Hello, World!"",
                        ""receivedAt"": ""2001-02-24T14:50:23Z"",
                        ""future_field_not_yet_known"": ""Hello""
                    }
                    ]}";
            var apiKey = new InMobileApiKey("UnitTestKey123");
            var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/sms/incoming/messages?limit=8", jsonOrNull: null);
            var responseToSendBack = new UnitTestResponseInfo(jsonOrNull: responseJson);
            
            using var server = StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendBack));
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var response = client.SmsIncoming.GetMessages(limit: 8);
            
            Assert.Multiple(
                () => Assert.NotNull(response),
                () => Assert.NotNull(response.Messages),
                () => Assert.Single(response.Messages!),
                () => Assert.Null(response.Messages!.Single().From.CountryCode),
                () => Assert.Null(response.Messages!.Single().From.PhoneNumber),
                () => Assert.Equal("inMobile", response.Messages!.Single().From.RawSource),
                () => Assert.False(response.Messages!.Single().From.IsValidMsisdn),
                () => Assert.Equal("46", response.Messages!.Single().To.CountryCode),
                () => Assert.Equal("88776655", response.Messages!.Single().To.PhoneNumber),
                () => Assert.Equal("4588776655", response.Messages!.Single().To.Msisdn),
                () => Assert.Equal("Hello, World!", response.Messages!.Single().Text),
                () => Assert.Equal(new DateTime(2001, 2, 24, 14, 50, 23, DateTimeKind.Utc), response.Messages!.Single().ReceivedAt));
    }
    
    [Fact]
    public void GetReports_ApiError_Test()
    {
        var responseJson = @"{
""errorMessage"": ""Forbidden thing"",
""details"": [
""You shall not pass"",
""Go away""
]
}";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "GET /v4/sms/incoming/messages?limit=1", jsonOrNull: null);
        var responseToSendBack = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "500 ServerError");
        
        using var server = StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendBack));
        
        var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
        var ex = Assert.Throws<InMobileApiException>(() => client.SmsIncoming.GetMessages(limit: 1));

        Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(249)]
    [InlineData(250)]
    public void GetMessages_Limit_ValidLimitsTest(int limit)
    {
        var emptyResponseJson = @"{
                    ""messages"": [
                    ]}";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: $"GET /v4/sms/incoming/messages?limit={limit}", jsonOrNull: null);
        var responseToSendBack = new UnitTestResponseInfo(jsonOrNull: emptyResponseJson);
        
        using var server = StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendBack));
        
        var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
        
        var reports = client.SmsIncoming.GetMessages(limit);
        Assert.Empty(reports.Messages!);
    }
    
    [Theory]
    [InlineData(-1000)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(251)]
    [InlineData(1000)]
    public void GetMessages_Limit_InvalidLimits_Test(int limit)
    {
        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: $"GET /v4/sms/incoming/messages?limit={limit}", jsonOrNull: null);
        var responseToSendBack = new UnitTestResponseInfo(jsonOrNull: null);
        
        using var server = StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendBack));
        
        var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
        
        Assert.Throws<ArgumentException>(() => client.SmsIncoming.GetMessages(limit: limit));
    }
}