using System;
using System.Collections.Generic;
using System.Text;
using InMobile.Sms.ApiClient.List;

namespace InMobile.Sms.ApiClient
{
    public interface IInmobileApiClient
    {
        
    }

    public class InmobileApiClient : IInmobileApiClient
    {
        private readonly InmobileApiKey _apiKey;
        public ISmsOutgoingClient SmsOutgoing { get; private set; }
        public IBlacklistClient Blacklist { get; private set; }
        public IListClient Lists { get; private set; }
        public InmobileApiClient(InmobileApiKey apiKey)
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            SmsOutgoing = new SmsOutgoingClient(apiKey);
            Blacklist = new BlacklistClient(apiKey: apiKey);
            Lists = new ListClient(apiKey: apiKey);
        }
    }
}
