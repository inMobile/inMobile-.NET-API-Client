using System;
using System.Collections.Generic;
using RestSharp;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Blacklist specific operations.
    /// </summary>
    public interface IBlacklistApiMethods
    {
        /// <summary>
        /// Create new entry in the blacklist.
        /// </summary>
        /// <param name="createInfo"></param>
        /// <returns></returns>
        BlacklistEntry Create(BlacklistEntryCreateInfo createInfo);
        /// <summary>
        /// Get a blacklist entry by its id.
        /// </summary>
        /// <param name="blacklistEntryId"></param>
        /// <returns></returns>
        BlacklistEntry GetById(BlacklistEntryId blacklistEntryId);
        /// <summary>
        /// Get a blacklist entry by its number.
        /// </summary>
        /// <param name="numberInfo"></param>
        /// <returns></returns>
        BlacklistEntry GetByNumber(NumberInfo numberInfo);
        /// <summary>
        /// Delete a blacklist entry by its id.
        /// </summary>
        /// <param name="blacklistEntryId"></param>
        void DeleteById(BlacklistEntryId blacklistEntryId);
        /// <summary>
        /// Delete a blacklist entry by its number.
        /// </summary>
        /// <param name="numberInfo"></param>
        void DeleteByNumber(NumberInfo numberInfo);
        /// <summary>
        /// Gets all blacklist entries.
        /// </summary>
        /// <returns></returns>
        List<BlacklistEntry> GetAll();
    }

    internal class BlacklistApiMethods : IBlacklistApiMethods
    {
        private readonly IApiRequestHelper _requestHelper;

        public BlacklistApiMethods(IApiRequestHelper requestHelper)
        {
            _requestHelper = requestHelper ?? throw new ArgumentNullException(nameof(requestHelper));
        }

        public BlacklistEntry Create(BlacklistEntryCreateInfo createInfo)
        {
            EnsureNotNullOrThrow(parameterName: nameof(createInfo), value: createInfo);
            return _requestHelper.Execute<BlacklistEntry>(
                                    method: Method.POST,
                                    resource: "/v4/blacklist",
                                    payload: new
                                    {
                                        NumberInfo = createInfo.NumberInfo,
                                        Comment = createInfo.Comment
                                    });
        }

        /// <summary>
        /// Gets all blacklist entries. This call is using a paged api and will do the api calls during the iteration of the returned enumerable.
        /// This allowes for client to not allocate memory for holding the entire list of entries.
        /// </summary>
        /// <returns></returns>
        public List<BlacklistEntry> GetAll()
        {
            return _requestHelper.ExecuteGetAndIteratePagedResult<BlacklistEntry>(resource: "/v4/blacklist?pageLimit=250");
        }

        public BlacklistEntry GetById(BlacklistEntryId blacklistEntryId)
        {
            EnsureNotNullOrThrow(parameterName: nameof(blacklistEntryId), value: blacklistEntryId);
            return _requestHelper.Execute<BlacklistEntry>(method: Method.GET, resource: $"/v4/blacklist/{blacklistEntryId}");
        }

        public BlacklistEntry GetByNumber(NumberInfo numberInfo)
        {
            EnsureNotNullOrThrow(parameterName: nameof(numberInfo), value: numberInfo);
            return _requestHelper.Execute<BlacklistEntry>(method: Method.GET, resource: $"/v4/blacklist/bynumber?countryCode={numberInfo.CountryCode}&phoneNumber={numberInfo.PhoneNumber}");
        }

        public void DeleteById(BlacklistEntryId blacklistEntryId)
        {
            EnsureNotNullOrThrow(parameterName: nameof(blacklistEntryId), value: blacklistEntryId);
            _requestHelper.ExecuteWithNoContent(method: Method.DELETE, resource: $"/v4/blacklist/{blacklistEntryId}");
        }

        public void DeleteByNumber(NumberInfo numberInfo)
        {
            EnsureNotNullOrThrow(parameterName: nameof(numberInfo), value: numberInfo);
            _requestHelper.ExecuteWithNoContent(method: Method.DELETE, resource: $"/v4/blacklist/bynumber?countryCode={numberInfo.CountryCode}&phoneNumber={numberInfo.PhoneNumber}");
        }

        private void EnsureNotNullOrThrow(string parameterName, object? value)
        {
            if (value == null)
                throw new ArgumentException($"'{parameterName}' cannot be null.", nameof(value));
        }
    }
}
