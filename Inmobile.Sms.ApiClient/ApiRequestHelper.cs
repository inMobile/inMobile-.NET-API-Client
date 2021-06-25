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
        private const string UserAgent = "Inmobile .net client";
        public ApiRequestHelper(InmobileApiKey apiKey)
        {
            _authenticator = new HttpBasicAuthenticator(username: "_", password: apiKey.ApiKey);
        }

        public T Execute<T>(Method method, string resource, object payload)
        {
            IRestResponse<T> response = GetClient().Execute<T>(request: GetRequest(method: method, resource: resource));
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new InmobileApiException(response: response);
            }
        }

        private RestClient GetClient()
        {
            var client = new RestClient(baseUrl: "https://api.inmobile.com/v4");
            client.UserAgent = UserAgent;
            client.Authenticator = _authenticator;
            return client;
        }
        private RestRequest GetRequest(Method method, string resource)
        {
            if (string.IsNullOrEmpty(resource))
            {
                throw new ArgumentException($"'{nameof(resource)}' cannot be null or empty.", nameof(resource));
            }

            var request = new RestRequest(resource: resource);
            request.AddHeader("content-type", "application/json");
            request.Method = method;

            return request;
        }
    }
}
