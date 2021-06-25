using System;
using System.Collections.Generic;
using RestSharp;
using RestSharp.Authenticators;

namespace InMobile.Sms.ApiClient
{
    public interface IApiRequestHelper
    {
        T Execute<T>(Method method, string resource, object? payload = null);
        void ExecuteWithNoContent(Method method, string resource, object? payload = null);
        List<T> ExecuteGetAndIteratePagedResult<T>(string resource);
    }

    internal class ApiRequestHelper : IApiRequestHelper
    {
        private readonly HttpBasicAuthenticator _authenticator;
        private readonly string _baseUrl;
        private readonly string _inmobileClientVersion;
        public ApiRequestHelper(InMobileApiKey apiKey, string baseUrl)
        {
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentException($"'{nameof(baseUrl)}' cannot be null or empty.", nameof(baseUrl));
            }

            _authenticator = new HttpBasicAuthenticator(username: "_", password: apiKey.ApiKey);
            _baseUrl = baseUrl;
            _inmobileClientVersion = $"Inmobile .Net Client v{GetType().Assembly.GetName().Version}";
        }

        public List<T> ExecuteGetAndIteratePagedResult<T>(string resource)
        {
            List<T> allEntries = new List<T>();
            bool lastPage;
            PagedResult<T> currentResult;
            var nextResource = resource;
            do
            {
                currentResult = Execute<PagedResult<T>>(method: Method.GET, resource: nextResource);
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

        public T Execute<T>(Method method, string resource, object? payload = null)
        {
            IRestResponse<T> response = GetClient().Execute<T>(request: GetRequest(method: method, resource: resource, payload: payload));
            ThrowIfNotSuccessful(response);
            return response.Data;
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
