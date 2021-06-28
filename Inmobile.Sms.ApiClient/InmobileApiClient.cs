using System;

namespace InMobile.Sms.ApiClient
{
    public interface IInmobileApiClient
    {
        
    }

    public class InmobileApiClient : IInmobileApiClient
    {
        public ISmsOutgoingClient SmsOutgoing { get; private set; }
        public IBlacklistClient Blacklist { get; private set; }
        public IListClient Lists { get; private set; }
        public InmobileApiClient(InmobileApiKey apiKey)
        {
            if (apiKey is null)
            {
                throw new ArgumentNullException(nameof(apiKey));
            }

            var requestHelper = new ApiRequestHelper(apiKey: apiKey);
            
            SmsOutgoing = new SmsOutgoingClient(requestHelper);
            Blacklist = new BlacklistClient(requestHelper);
            Lists = new ListClient(requestHelper);
        }
    }
}
