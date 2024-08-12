using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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

        internal static InMobileApiException? ParseOrNull(WebException webException)
            => ParseOrNullInternal(webException: webException, mode: SyncMode.Sync).GetAwaiter().GetResult();
        
        internal static Task<InMobileApiException?> ParseOrNullAsync(WebException webException)
            => ParseOrNullInternal(webException: webException, mode: SyncMode.Async);
        
        private static async Task<InMobileApiException?> ParseOrNullInternal(WebException webException, SyncMode mode)
        {
            var response = (HttpWebResponse)webException.Response;
            if (response == null)
                return null;
            if (response.StatusCode == 0)
                return null;
            
            var content = mode == SyncMode.Sync 
                ? ReadContentInternal(response, mode: SyncMode.Sync).GetAwaiter().GetResult() 
                : await ReadContentInternal(response, mode: SyncMode.Async);
            
            var responseObject = JsonConvert.DeserializeObject<ErrorResponse>(content, new InMobileJsonSerializerSettings());
            if (responseObject == null)
                return null;
            
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"{responseObject.ErrorMessage}.");
            if (responseObject.Details != null)
                stringBuilder.Append($" {string.Join("; ", responseObject.Details)}");

            return new InMobileApiException(response.StatusCode, $"{(int)response.StatusCode} {response.StatusCode}: {stringBuilder}");
        }

        private static async Task<string> ReadContentInternal(HttpWebResponse response, SyncMode mode)
        {
            using var r = new StreamReader(response.GetResponseStream());
            
            return mode == SyncMode.Sync 
                ? r.ReadToEnd()
                : await r.ReadToEndAsync();
        }
    }
}