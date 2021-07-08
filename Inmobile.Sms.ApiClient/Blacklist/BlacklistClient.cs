using System;
using System.Collections.Generic;
using RestSharp;

namespace InMobile.Sms.ApiClient
{
    public interface IBlacklistClient
    {
        BlacklistEntry GetById(string id);
        List<BlacklistEntry> GetAll();
        void GetBlacklistingById();
    }

    internal class BlacklistClient : IBlacklistClient
    {
        private readonly IApiRequestHelper _requestHelper;

        public BlacklistClient(IApiRequestHelper requestHelper)
        {
            _requestHelper = requestHelper ?? throw new ArgumentNullException(nameof(requestHelper));
        }

        public BlacklistEntry GetById(string id)
        {
            return _requestHelper.Execute<BlacklistEntry>(method: Method.GET, resource: $"v4/blacklist/{id}");
        }

        /// <summary>
        /// Gets all blacklist entries. This call is using a paged api and will do the api calls during the iteration of the returned enumerable.
        /// This allowes for client to not allocate memory for holding the entire list of entries.
        /// </summary>
        /// <returns></returns>
        public List<BlacklistEntry> GetAll()
        {
            return _requestHelper.ExecuteGetAndIteratePagedResult<BlacklistEntry>(resource: "v4/blacklist?pageLimit=250");
        }

        public void GetBlacklistingById()
        {
            throw new NotImplementedException();
        }
    }
}
