using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    internal interface IApiRequestHelper
    {
        T Execute<T>(Method method, string resource, object? payload = null) where T : class;
        Task<T> ExecuteAsync<T>(Method method, string resource, object? payload = null) where T : class;
        
        void ExecuteWithNoContent(Method method, string resource, object? payload = null);
        Task ExecuteWithNoContentAsync(Method method, string resource, object? payload = null);
        
        List<T> ExecuteGetAndIteratePagedResult<T>(string resource);
        Task<List<T>> ExecuteGetAndIteratePagedResultAsync<T>(string resource);
    }

    /// <summary>
    /// A general help for rest requests
    /// </summary>
    internal class ApiRequestHelper : IApiRequestHelper
    {
        private static readonly Encoding _encoding = Encoding.GetEncoding("ISO-8859-1");
        private static readonly Encoding _utf8WithoutBom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

        private readonly string _authenticationHeaderValue;
        private readonly string _baseUrl;
        private readonly string _inmobileClientVersion;
        private readonly string _dotnetVersion;
        private readonly InMobileJsonSerializerSettings _serializerSettings;

        public ApiRequestHelper(InMobileApiKey apiKey, string baseUrl)
        {
            if (string.IsNullOrEmpty(baseUrl))
                throw new ArgumentException($"'{nameof(baseUrl)}' cannot be null or empty.", nameof(baseUrl));
            if (baseUrl.EndsWith("/"))
                throw new ArgumentException($"'{nameof(baseUrl)}' must not end with a /", nameof(baseUrl));
            
            _authenticationHeaderValue = $"Basic {Convert.ToBase64String(_encoding.GetBytes("_:" + apiKey.ApiKey))}";
            _baseUrl = baseUrl;
            _inmobileClientVersion = $"Inmobile .Net Client v{GetType().Assembly.GetName().Version}";
            _dotnetVersion = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
            _serializerSettings = new InMobileJsonSerializerSettings();
        }

        public List<T> ExecuteGetAndIteratePagedResult<T>(string resource)
            => ExecuteGetAndIteratePagedResultInternal<T>(resource: resource, mode: SyncMode.Sync).GetAwaiter().GetResult();

        public Task<List<T>> ExecuteGetAndIteratePagedResultAsync<T>(string resource)
            => ExecuteGetAndIteratePagedResultInternal<T>(resource: resource, mode: SyncMode.Async);
        
        private async Task<List<T>> ExecuteGetAndIteratePagedResultInternal<T>(string resource, SyncMode mode)
        {
            var allEntries = new List<T>();
            PagedResult<T> currentResult;
            var nextResource = resource;
            do
            {
#pragma warning disable CS8604 // Possible null reference argument. - This is never NULL as .Next is always a string when IsLastPage is false. But the compiler has no chance of knowing this.
                currentResult = mode == SyncMode.Sync 
                    ? Execute<PagedResult<T>>(method: Method.GET, resource: nextResource)
                    : await ExecuteAsync<PagedResult<T>>(method: Method.GET, resource: nextResource);
#pragma warning restore CS8604 // Possible null reference argument.
                
                allEntries.AddRange(currentResult.Entries);
                nextResource = currentResult._links.Next;
            } while (!currentResult._links.IsLastPage);

            return allEntries;
        }

        public void ExecuteWithNoContent(Method method, string resource, object? payload = null)
            => ExecuteWithNoContentInternal(method: method, resource: resource, payload: payload, mode: SyncMode.Sync).GetAwaiter().GetResult();

        public Task ExecuteWithNoContentAsync(Method method, string resource, object? payload = null)
            => ExecuteWithNoContentInternal(method: method, resource: resource, payload: payload, mode: SyncMode.Async);
        
        private async Task ExecuteWithNoContentInternal(Method method, string resource, object? payload, SyncMode mode)
        {
            var payloadString = payload != null ? JsonConvert.SerializeObject(payload, _serializerSettings) : null;
            _ = mode == SyncMode.Sync 
                ? ExecuteInternal(method: method, resource: resource, payloadString: payloadString)
                : await ExecuteInternalAsync(method: method, resource: resource, payloadString: payloadString);
        }

        public T Execute<T>(Method method, string resource, object? payload = null) where T : class
            => ExecuteInternal<T>(method: method, resource: resource, payload: payload, mode: SyncMode.Sync).GetAwaiter().GetResult();

        public Task<T> ExecuteAsync<T>(Method method, string resource, object? payload = null) where T : class
            => ExecuteInternal<T>(method: method, resource: resource, payload: payload, mode: SyncMode.Async);
        
        private async Task<T> ExecuteInternal<T>(Method method, string resource, object? payload, SyncMode mode) where T : class
        {
            var payloadString = payload != null ? JsonConvert.SerializeObject(payload, _serializerSettings) : null;
            var responseString = mode == SyncMode.Sync 
                ? ExecuteInternal(method: method, resource: resource, payloadString: payloadString)
                : await ExecuteInternalAsync(method: method, resource: resource, payloadString: payloadString);
            
            var result = JsonConvert.DeserializeObject<T>(responseString, _serializerSettings);
            if (result == null)
                throw new Exception($"Unexpected NULL after deserializing string {responseString}");

            return result;
        }

        private string ExecuteInternal(Method method, string resource, string? payloadString = null)
        {
            if (!resource.StartsWith("/"))
                throw new ArgumentException("Resource must start with a /");

            // Prepare basic request options
            var request = BuildHttpWebRequest(method: method, resource: resource);

            // Add payload if specified
            if (payloadString != null)
            {
                request.ContentLength = _utf8WithoutBom.GetByteCount(payloadString);
                using (var reqStream = request.GetRequestStream())
                using (var streamWriter = new StreamWriter(reqStream, _utf8WithoutBom))
                {
                    streamWriter.Write(payloadString);
                    streamWriter.Flush();
                }
            }

            // Execute and read response
            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    if ((int)response.StatusCode < 200 || (int)response.StatusCode > 299)
                    {
                        throw new Exception($"Unexpected status code received: {response.StatusCode}");
                    }

                    using (var dataStream = response.GetResponseStream())
                    using (var reader = new StreamReader(dataStream))
                    {
                        var responseFromServer = reader.ReadToEnd();
                        return responseFromServer;
                    }
                }
            }
            catch (WebException webException)
            {
                var apiException = InMobileApiException.ParseOrNull(webException);
                if (apiException != null)
                    throw apiException;

                throw;
            }
        }

        private async Task<string> ExecuteInternalAsync(Method method, string resource, string? payloadString = null)
        {
            if (!resource.StartsWith("/"))
                throw new ArgumentException("Resource must start with a /");

            // Prepare basic request options
            var request = BuildHttpWebRequest(method: method, resource: resource);
            
            // Add payload if specified
            if (payloadString != null)
            {
                request.ContentLength = _utf8WithoutBom.GetByteCount(payloadString);
                using (var reqStream = await request.GetRequestStreamAsync())
                using (var streamWriter = new StreamWriter(reqStream, _utf8WithoutBom))
                {
                    await streamWriter.WriteAsync(payloadString);
                    await streamWriter.FlushAsync();
                }
            }

            // Execute and read response
            try
            {
                using (var response = (HttpWebResponse)await request.GetResponseAsync())
                {
                    if ((int)response.StatusCode < 200 || (int)response.StatusCode > 299)
                    {
                        throw new Exception($"Unexpected status code received: {response.StatusCode}");
                    }

                    using (var dataStream = response.GetResponseStream())
                    using (var reader = new StreamReader(dataStream))
                    {
                        var responseFromServer = await reader.ReadToEndAsync();
                        return responseFromServer;
                    }
                }
            }
            catch (WebException webException)
            {
                var apiException = await InMobileApiException.ParseOrNullAsync(webException);
                if (apiException != null)
                    throw apiException;

                throw;
            }
        }

        private HttpWebRequest BuildHttpWebRequest(Method method, string resource)
        {
            var request = (HttpWebRequest)WebRequest.Create(_baseUrl + resource);
            request.Method = method.ToString();
            request.AllowAutoRedirect = false;
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", _authenticationHeaderValue);
            request.Headers.Add("X-InmobileClientVersion", _inmobileClientVersion);
            request.Headers.Add("X-DotnetVersion", _dotnetVersion);

            return request;
        }
    }
}