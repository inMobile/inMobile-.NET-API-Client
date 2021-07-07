using System;

namespace InMobile.Sms.ApiClient
{
    public interface IInMobileApiClient
    {
        
    }

    public class InMobileApiClient : IInMobileApiClient
    {
        public ISmsOutgoingClient SmsOutgoing { get; private set; }
        public IBlacklistClient Blacklist { get; private set; }
        public IListClient Lists { get; private set; }
        public InMobileApiClient(InMobileApiKey apiKey, string baseUrl = "https://api.inmobile.com")
        {
            if (apiKey is null)
            {
                throw new ArgumentNullException(nameof(apiKey));
            }

            var requestHelper = new ApiRequestHelper(apiKey: apiKey, baseUrl: baseUrl);
            
            SmsOutgoing = new SmsOutgoingClient(requestHelper);
            Blacklist = new BlacklistClient(requestHelper);
            Lists = new ListClient(requestHelper);
        }
    }
}
