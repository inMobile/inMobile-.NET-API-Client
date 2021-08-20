using System;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Official InMobile API client
    /// </summary>
    public class InMobileApiClient
    {
        /// <summary>
        /// Outgoing message specific operations.
        /// </summary>
        public ISmsOutgoingApiMethod SmsOutgoing { get; private set; }
        /// <summary>
        /// Blacklist specific operations.
        /// </summary>
        public IBlacklistApiMethods Blacklist { get; private set; }
        /// <summary>
        /// List specific operations.
        /// </summary>
        public IListApiMethods Lists { get; private set; }
        /// <summary>
        /// Creates a new api client.
        /// </summary>
        /// <param name="apiKey">The api key to be used.</param>
        /// <param name="baseUrl">The base url of the api. This can be changed in case an internal proxy is in use.</param>
        public InMobileApiClient(InMobileApiKey apiKey, string baseUrl = "https://api.inmobile.com")
        {
            if (apiKey is null)
            {
                throw new ArgumentNullException(nameof(apiKey));
            }

            var requestHelper = new ApiRequestHelper(apiKey: apiKey, baseUrl: baseUrl);
            
            SmsOutgoing = new SmsOutgoingMethods(requestHelper);
            Blacklist = new BlacklistApiMethods(requestHelper);
            Lists = new ListApiMethods(requestHelper);
        }
    }
}
