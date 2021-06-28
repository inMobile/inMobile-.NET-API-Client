using System;
using System.Text;
using Newtonsoft.Json;
using RestSharp;

namespace InMobile.Sms.ApiClient
{
    public class InmobileApiException : Exception
    {
        public InmobileApiException(IRestResponse response) : base(message: FormatMessage(response: response))
        {

        }

        private static string FormatMessage(IRestResponse response)
        {
            var responseObject = JsonConvert.DeserializeObject<ErrorResponse>(response.Content, JsonNetSerializer.Settings);
            if (responseObject == null)
                throw new Exception($"Exception during deserialization of error message. Raw error message: {response.Content}");
            return $"{responseObject.ErrorMessage}. {string.Join("; ", responseObject.Details)}";
        }
    }
}
