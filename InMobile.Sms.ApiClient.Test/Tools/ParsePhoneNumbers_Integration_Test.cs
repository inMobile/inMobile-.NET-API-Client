using System.Collections.Generic;
using System.Net;
using static InMobile.Sms.ApiClient.Test.UnitTestHttpServer;
using Xunit;
using System.Linq;
using System.Threading.Tasks;

namespace InMobile.Sms.ApiClient.Test.Tools;

public class ParsePhoneNumbers_Integration_Test
{
    [Fact]
    public void ParsePhoneNumbers_SingleEntry_Test()
    {
        var requestJson = @"{""NumbersToParse"":[{""CountryHint"":""DK"",""RawMsisdn"":""+45 12 34 56 78""}]}";
        var responseJson = @"{
                ""results"": [
                    {
                        ""countryCode"": ""45"",
                        ""phoneNumber"": ""12345678"",
                        ""rawMsisdn"": ""+45 12 34 56 78"",
                        ""msisdn"": ""4512345678"",
                        ""isValidMsisdn"": true,
                        ""countryHint"": ""DK""
                    }
                ]
            }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/tools/parsephonenumbers", jsonOrNull: requestJson);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var response = client.Tools.ParsePhoneNumbers(new List<ParsePhoneNumberInfo> { new ParsePhoneNumberInfo("DK", "+45 12 34 56 78") });

            Assert.NotNull(response);
            Assert.Single(response.Results);

            var singleResult = response.Results.Single();
            Assert.NotNull(singleResult);
            Assert.Equal("45", singleResult.CountryCode);
            Assert.Equal("12345678", singleResult.PhoneNumber);
            Assert.Equal("+45 12 34 56 78", singleResult.RawMsisdn);
            Assert.Equal("4512345678", singleResult.Msisdn);
            Assert.True(singleResult.IsValidMsisdn);
            Assert.Equal("DK", singleResult.CountryHint);
        }
    }

    [Fact]
    public async Task ParsePhoneNumbersAsync_SingleEntry_Test()
    {
        var requestJson = @"{""NumbersToParse"":[{""CountryHint"":""DK"",""RawMsisdn"":""+45 12 34 56 78""}]}";
        var responseJson = @"{
                ""results"": [
                    {
                        ""countryCode"": ""45"",
                        ""phoneNumber"": ""12345678"",
                        ""rawMsisdn"": ""+45 12 34 56 78"",
                        ""msisdn"": ""4512345678"",
                        ""isValidMsisdn"": true,
                        ""countryHint"": ""DK""
                    }
                ]
            }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/tools/parsephonenumbers", jsonOrNull: requestJson);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson);
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var response = await client.Tools.ParsePhoneNumbersAsync(new List<ParsePhoneNumberInfo> { new ParsePhoneNumberInfo("DK", "+45 12 34 56 78") });

            Assert.NotNull(response);
            Assert.Single(response.Results);

            var singleResult = response.Results.Single();
            Assert.NotNull(singleResult);
            Assert.Equal("45", singleResult.CountryCode);
            Assert.Equal("12345678", singleResult.PhoneNumber);
            Assert.Equal("+45 12 34 56 78", singleResult.RawMsisdn);
            Assert.Equal("4512345678", singleResult.Msisdn);
            Assert.True(singleResult.IsValidMsisdn);
            Assert.Equal("DK", singleResult.CountryHint);
        }
    }

    [Fact]
    public void ParsePhoneNumbers_ApiError_Test()
    {
        var requestJson = @"{""NumbersToParse"":[{""CountryHint"":""DK"",""RawMsisdn"":""+45 12 34 56 78""}]}";
        var responseJson = @"{
                ""errorMessage"": ""Forbidden thing"",
                ""details"": [
                    ""You shall not pass"",
                    ""Go away""
                ]
            }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/tools/parsephonenumbers", jsonOrNull: requestJson);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "500 ServerError");
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var ex = Assert.Throws<InMobileApiException>(() => client.Tools.ParsePhoneNumbers(new List<ParsePhoneNumberInfo> { new ParsePhoneNumberInfo("DK", "+45 12 34 56 78") }));

            Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
        }
    }

    [Fact]
    public async Task ParsePhoneNumbersAsync_ApiError_Test()
    {
        var requestJson = @"{""NumbersToParse"":[{""CountryHint"":""DK"",""RawMsisdn"":""+45 12 34 56 78""}]}";
        var responseJson = @"{
                ""errorMessage"": ""Forbidden thing"",
                ""details"": [
                    ""You shall not pass"",
                    ""Go away""
                ]
            }";

        var apiKey = new InMobileApiKey("UnitTestKey123");
        var expectedRequest = new UnitTestRequestInfo(apiKey: apiKey, methodAndPath: "POST /v4/tools/parsephonenumbers", jsonOrNull: requestJson);
        var responseToSendback = new UnitTestResponseInfo(jsonOrNull: responseJson, statusCodeString: "500 ServerError");
        using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(new RequestResponsePair(request: expectedRequest, response: responseToSendback)))
        {
            var client = new InMobileApiClient(apiKey, baseUrl: $"http://{server.EndPoint.Address}:{server.EndPoint.Port}");
            var ex = await Assert.ThrowsAsync<InMobileApiException>(() => client.Tools.ParsePhoneNumbersAsync(new List<ParsePhoneNumberInfo> { new ParsePhoneNumberInfo("DK", "+45 12 34 56 78") }));

            Assert.Equal(HttpStatusCode.InternalServerError, ex.ErrorHttpStatusCode);
        }
    }
}