using System;
using System.Text;
using Newtonsoft.Json;
using RestSharp;

namespace InMobile.Sms.ApiClient
{
    public class InMobileApiException : Exception
    {
        public InMobileApiException(string message) : base(message)
        {

        }

        public static bool TryParse(IRestResponse response, out InMobileApiException? exception)
        {
            exception = null;
            if (response.StatusCode == 0)
                return false;
            var responseObject = JsonConvert.DeserializeObject<ErrorResponse>(response.Content, JsonNetSerializer.Settings);
            if (responseObject == null)
                return false;
            StringBuilder sb = new StringBuilder();
            sb.Append($"{responseObject.ErrorMessage}.");
            if(responseObject.Details != null)
            {
                sb.Append($" {string.Join("; ", responseObject.Details)}");
            }

            exception = new InMobileApiException(sb.ToString());
            return true;
        }
    }
}
