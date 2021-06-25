using System;

namespace InMobile.Sms.ApiClient
{
    public interface IInMobileApiClient
    {
        
    }

    public class InMobileApiClient : IInMobileApiClient
    {
        public ISmsOutgoingApiMethod SmsOutgoing { get; private set; }
        public IBlacklistApiMethods Blacklist { get; private set; }
        public IListApiMethods Lists { get; private set; }
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
