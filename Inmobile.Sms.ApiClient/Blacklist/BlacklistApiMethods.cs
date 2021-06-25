using System;
using System.Collections.Generic;
using RestSharp;

namespace InMobile.Sms.ApiClient
{
    public interface IBlacklistApiMethods
    {
        BlacklistEntry Add(string countryCode, string number, string? comment = null);
        BlacklistEntry GetById(string blacklistEntryId);
        BlacklistEntry GetByNumber(string countryCode, string phoneNumber);
        void RemoveById(string blacklistEntryId);
        void RemoveByNumber(string countryCode, string phoneNumber);
        List<BlacklistEntry> GetAll();
    }

    internal class BlacklistApiMethods : IBlacklistApiMethods
    {
        private readonly IApiRequestHelper _requestHelper;

        public BlacklistApiMethods(IApiRequestHelper requestHelper)
        {
            _requestHelper = requestHelper ?? throw new ArgumentNullException(nameof(requestHelper));
        }

        public BlacklistEntry Add(string countryCode, string phoneNumber, string? comment = null)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(countryCode), value: countryCode);
            EnsureNonEmptyOrThrow(parameterName: nameof(phoneNumber), value: phoneNumber);
            return _requestHelper.Execute<BlacklistEntry>(
                                    method: Method.POST,
                                    resource: "/v4/blacklist",
                                    payload: new
                                    {
                                        numberInfo = new
                                        {
                                            countryCode = countryCode,
                                            phoneNumber = phoneNumber
                                        },
                                        comment = comment
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

        public BlacklistEntry GetById(string blacklistEntryId)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(blacklistEntryId), value: blacklistEntryId);
            return _requestHelper.Execute<BlacklistEntry>(method: Method.GET, resource: $"/v4/blacklist/{blacklistEntryId}");
        }

        public BlacklistEntry GetByNumber(string countryCode, string phoneNumber)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(countryCode), value: countryCode);
            EnsureNonEmptyOrThrow(parameterName: nameof(phoneNumber), value: phoneNumber);
            return _requestHelper.Execute<BlacklistEntry>(method: Method.GET, resource: $"/v4/blacklist/bynumber?countryCode={countryCode}&phoneNumber={phoneNumber}");
        }

        public void RemoveById(string blacklistEntryId)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(blacklistEntryId), value: blacklistEntryId);
            _requestHelper.ExecuteWithNoContent(method: Method.DELETE, resource: $"/v4/blacklist/{blacklistEntryId}");
        }

        public void RemoveByNumber(string countryCode, string phoneNumber)
        {
            EnsureNonEmptyOrThrow(parameterName: nameof(countryCode), value: countryCode);
            EnsureNonEmptyOrThrow(parameterName: nameof(phoneNumber), value: phoneNumber);
            _requestHelper.ExecuteWithNoContent(method: Method.DELETE, resource: $"/v4/blacklist/bynumber?countryCode={countryCode}&phoneNumber={phoneNumber}");
        }


        private void EnsureNonEmptyOrThrow(string parameterName, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException($"'{parameterName}' cannot be null or empty.", nameof(value));
            }
        }


    }
}
