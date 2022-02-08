using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;


namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Exception based on an AIP error
    /// </summary>
    public class InMobileApiException : Exception
    {
        /// <summary>
        /// The http status code
        /// </summary>
        public HttpStatusCode ErrorHttpStatusCode { get; }
        internal InMobileApiException(HttpStatusCode errorHttpStatusCode, string message) : base(message)
        {
            ErrorHttpStatusCode = errorHttpStatusCode;
        }

        internal static bool TryParse(WebException webException, out InMobileApiException? exception)
        {
            var response = (HttpWebResponse)webException.Response;
            if (response != null)
            {
                exception = null;
                if (response.StatusCode == 0)
                    return false;
                var responseObject = JsonConvert.DeserializeObject<ErrorResponse>(ReadContent(response), new InMobileJsonSerializerSettings());
                if (responseObject == null)
                    return false;
                StringBuilder sb = new StringBuilder();
                sb.Append($"{responseObject.ErrorMessage}.");
                if (responseObject.Details != null)
                {
                    sb.Append($" {string.Join("; ", responseObject.Details)}");
                }

                exception = new InMobileApiException(response.StatusCode, $"{(int)response.StatusCode} {response.StatusCode}: {sb.ToString()}");
                return true;
            }
            else
            {
                exception = null;
                return false;
            }
        }

        private static string ReadContent(HttpWebResponse response)
        {
            using(var r = new StreamReader(response.GetResponseStream()))
            {
                return r.ReadToEnd();
            }
        }
    }
}
