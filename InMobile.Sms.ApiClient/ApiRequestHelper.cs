using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    internal interface IApiRequestHelper
    {
        T Execute<T>(Method method, string resource, object? payload = null) where T : class;
        void ExecuteWithNoContent(Method method, string resource, object? payload = null);
        List<T> ExecuteGetAndIteratePagedResult<T>(string resource);
    }
    /// <summary>
    /// A general help for rest requests
    /// </summary>
    internal class ApiRequestHelper : IApiRequestHelper
    {
        private readonly InMobileApiKey _apiKey;
        private readonly string _baseUrl;
        private readonly string _inmobileClientVersion;
        private readonly InMobileJsonSerializerSettings _serializerSettings;
        public ApiRequestHelper(InMobileApiKey apiKey, string baseUrl)
        {
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentException($"'{nameof(baseUrl)}' cannot be null or empty.", nameof(baseUrl));
            }

            _apiKey = apiKey;
            _baseUrl = baseUrl;
            _inmobileClientVersion = $"Inmobile .Net Client v{GetType().Assembly.GetName().Version}";
            _serializerSettings = new InMobileJsonSerializerSettings();
        }

        public List<T> ExecuteGetAndIteratePagedResult<T>(string resource)
        {
            List<T> allEntries = new List<T>();
            PagedResult<T> currentResult;
            var nextResource = resource;
            do
            {
#pragma warning disable CS8604 // Possible null reference argument. - This is never NULL as .Next is always a string when IsLastPage is false. But the compiler hos chance if knowing this.
                currentResult = Execute<PagedResult<T>>(method: Method.GET, resource: nextResource);
#pragma warning restore CS8604 // Possible null reference argument.
                allEntries.AddRange(currentResult.Entries);
                nextResource = currentResult._links.Next;
            } while (!currentResult._links.IsLastPage);
            return allEntries;
        }

        public void ExecuteWithNoContent(Method method, string resource, object? payload = null)
        {
            string? payloadString = payload != null ? JsonConvert.SerializeObject(payload, _serializerSettings) : null;
            ExecuteInternal(method: method, resource: resource, payloadString: payloadString);
        }

        
        public T Execute<T>(Method method, string resource, object? payload = null) where T : class
        {
            string? payloadString = payload != null ? JsonConvert.SerializeObject(payload, _serializerSettings) : null;
            string responseString = ExecuteInternal(method: method, resource: resource, payloadString: payloadString);
            T? result = JsonConvert.DeserializeObject<T>(responseString, _serializerSettings);
            if (result == null)
                throw new Exception($"Unexpected NULL afer deserializing string {responseString}");
            return result;
        }

        private static Encoding _encoding = Encoding.GetEncoding("ISO-8859-1");
        private static Encoding _utf8WithoutBom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
        private string ExecuteInternal(Method method, string resource, string? payloadString = null)
        {
            var url = _baseUrl + resource;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method.ToString();
            request.AllowAutoRedirect = false;
            string encoded = Convert.ToBase64String(_encoding.GetBytes("_:" + _apiKey.ApiKey));
            request.Headers.Add("Authorization", "Basic " + encoded);
            request.Headers.Add("content-type", "application/json");
            if (payloadString != null)
            {
                request.ContentLength = payloadString.Length;
                using(var reqStream = request.GetRequestStream())
                {
                    using (var streamWriter = new StreamWriter(request.GetRequestStream(), _utf8WithoutBom))
                    {
                        streamWriter.Write(payloadString);
                    }
                }   
            }

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(dataStream))
                        {
                            string responseFromServer = reader.ReadToEnd();
                            return responseFromServer;
                        }
                    }
                }
            }catch(WebException webException)
            {
                if (InMobileApiException.TryParse(webException, out var apiException))
                {
#pragma warning disable CS8597 // Thrown value may be null.
                    throw apiException;
#pragma warning restore CS8597 // Thrown value may be null.
                }

                throw;
            }
        }
    }
}
