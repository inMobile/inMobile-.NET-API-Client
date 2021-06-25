using System;
using System.Collections.Generic;
using System.Text;

namespace InMobile.Sms.ApiClient
{
    public interface IBlacklistClient
    {
        void AddNumberToBlacklist();
        void GetBlacklistingById();
    }

    internal class BlacklistClient : IBlacklistClient
    {
        private readonly IApiRequestHelper _requestHelper;

        public BlacklistClient(IApiRequestHelper requestHelper)
        {
            _requestHelper = requestHelper ?? throw new ArgumentNullException(nameof(requestHelper));
        }

        public void AddNumberToBlacklist()
        {
            throw new NotImplementedException();
        }

        public void GetBlacklistingById()
        {
            throw new NotImplementedException();
        }
    }
}
