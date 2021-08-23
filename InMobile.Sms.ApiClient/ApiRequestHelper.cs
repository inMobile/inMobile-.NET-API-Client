using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

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
        private readonly HttpBasicAuthenticator _authenticator;
        private readonly string _baseUrl;
        private readonly string _inmobileClientVersion;
        private readonly InMobileJsonSerializerSettings _serializerSettings;
        public ApiRequestHelper(InMobileApiKey apiKey, string baseUrl)
        {
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentException($"'{nameof(baseUrl)}' cannot be null or empty.", nameof(baseUrl));
            }

            _authenticator = new HttpBasicAuthenticator(username: "_", password: apiKey.ApiKey);
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
            var response = GetClient().Execute(request: GetRequest(method: method, resource: resource, payload: payload));
            ThrowIfNotSuccessful(response);
        }

        public T Execute<T>(Method method, string resource, object? payload = null) where T : class
        {
            // By design, we dont use the <T> overload of Execute request because that would trigger a deserialization before verifying that the request was successful and hence before ensuring the content is deserializable.
            IRestResponse response = GetClient().Execute(request: GetRequest(method: method, resource: resource, payload: payload));
            ThrowIfNotSuccessful(response);
            string json = response.Content;
            T? obj = JsonConvert.DeserializeObject<T>(json, _serializerSettings);
            if (obj == null)
                throw new Exception("Unexpected exception. Deserializing [" + json + "] returned NULL.");
            return (T)obj;
        }

        private void ThrowIfNotSuccessful(IRestResponse response)
        {
            if (response.IsSuccessful)
            {
                return;
            }

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }

            if (InMobileApiException.TryParse(response, out var apiException))
            {
#pragma warning disable CS8597 // Thrown value may be null.
                throw apiException;
#pragma warning restore CS8597 // Thrown value may be null.
            }

            throw new Exception($"Unexpected exception but new exceptions found on IRestResponse, nor could it be deserialized as an InMobileApiException. Raw content: {response.Content}");
        }

        private RestClient GetClient()
        {
            var client = new RestClient(baseUrl: _baseUrl);
            client.AddDefaultHeader("X-InmobileClientVersion", _inmobileClientVersion);
            client.UserAgent = _inmobileClientVersion;
            client.Authenticator = _authenticator;
            client.UseSerializer(() => new JsonNetSerializer());
            return client;
        }

        private RestRequest GetRequest(Method method, string resource, object? payload)
        {
            if (string.IsNullOrEmpty(resource))
            {
                throw new ArgumentException($"'{nameof(resource)}' cannot be null or empty.", nameof(resource));
            }

            var request = new RestRequest(resource: resource);
            request.AddHeader("content-type", "application/json");
            request.Method = method;
            if (payload != null)
            {
                request.AddJsonBody(payload);
            }

            return request;
        }
    }
}
