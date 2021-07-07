using System;
using RestSharp;
using RestSharp.Authenticators;

namespace InMobile.Sms.ApiClient
{
    public interface IApiRequestHelper
    {
        T Execute<T>(Method method, string resource, object payload);
    }

    internal class ApiRequestHelper : IApiRequestHelper
    {
        private readonly HttpBasicAuthenticator _authenticator;
        private readonly string _baseUrl;
        private const string UserAgent = "Inmobile .net client";
        public ApiRequestHelper(InMobileApiKey apiKey, string baseUrl)
        {
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentException($"'{nameof(baseUrl)}' cannot be null or empty.", nameof(baseUrl));
            }

            _authenticator = new HttpBasicAuthenticator(username: "_", password: apiKey.ApiKey);
            _baseUrl = baseUrl;
        }

        public T Execute<T>(Method method, string resource, object? payload)
        {
            IRestResponse<T> response = GetClient().Execute<T>(request: GetRequest(method: method, resource: resource, payload: payload));
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new InMobileApiException(response: response);
            }
        }

        private RestClient GetClient()
        {
            var client = new RestClient(baseUrl: _baseUrl);
            client.UserAgent = UserAgent;
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
            if(payload != null)
            {
                request.AddJsonBody(payload);
            }

            return request;
        }
    }
}
