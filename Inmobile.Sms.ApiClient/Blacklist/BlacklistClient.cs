using System;
using System.Collections.Generic;
using System.Text;

namespace InMobile.Sms.ApiClient
{
    public interface IBlacklistClient
    {
        void AddNumberToBlacklist();
        void GetBlacklistingById(BlacklistingId id);
    }

    public class BlacklistingId { }

    public class BlacklistClient : IBlacklistClient
    {
        private readonly InmobileApiKey _apiKey;

        public BlacklistClient(InmobileApiKey apiKey)
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        }

        public void AddNumberToBlacklist()
        {
            throw new NotImplementedException();
        }

        public void GetBlacklistingById(BlacklistingId id)
        {
            throw new NotImplementedException();
        }
    }
}
