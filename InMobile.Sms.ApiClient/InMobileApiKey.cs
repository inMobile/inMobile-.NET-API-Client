using System;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// InMobile account api key
    /// </summary>
    public class InMobileApiKey
    {
        /// <summary>
        /// The key value
        /// </summary>
        public string ApiKey { get; }
        /// <summary>
        /// Create a new api key
        /// </summary>
        /// <param name="apiKey"></param>
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
