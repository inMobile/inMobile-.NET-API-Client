using System;

namespace InMobile.Sms.ApiClient
{
    public class InMobileApiKey
    {
        public string ApiKey { get; }
        public InMobileApiKey(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentException($"'{nameof(apiKey)}' cannot be null or whitespace.", nameof(apiKey));
            }
            ApiKey = apiKey;
        }
    }
}
