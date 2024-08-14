using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        /// Create new entry in the blacklist (async).
        /// </summary>
        /// <param name="createInfo"></param>
        /// <returns></returns>
        Task<BlacklistEntry> CreateAsync(BlacklistEntryCreateInfo createInfo);

        /// <summary>
        /// Get a blacklist entry by its id.
        /// </summary>
        /// <param name="blacklistEntryId"></param>
        /// <returns></returns>
        BlacklistEntry GetById(BlacklistEntryId blacklistEntryId);

        /// <summary>
        /// Get a blacklist entry by its id (async).
        /// </summary>
        /// <param name="blacklistEntryId"></param>
        /// <returns></returns>
        Task<BlacklistEntry> GetByIdAsync(BlacklistEntryId blacklistEntryId);

        /// <summary>
        /// Get a blacklist entry by its number.
        /// </summary>
        /// <param name="numberInfo"></param>
        /// <returns></returns>
        BlacklistEntry GetByNumber(NumberInfo numberInfo);

        /// <summary>
        /// Get a blacklist entry by its number (async).
        /// </summary>
        /// <param name="numberInfo"></param>
        /// <returns></returns>
        Task<BlacklistEntry> GetByNumberAsync(NumberInfo numberInfo);

        /// <summary>
        /// Delete a blacklist entry by its id.
        /// </summary>
        /// <param name="blacklistEntryId"></param>
        void DeleteById(BlacklistEntryId blacklistEntryId);

        /// <summary>
        /// Delete a blacklist entry by its id (async).
        /// </summary>
        /// <param name="blacklistEntryId"></param>
        Task DeleteByIdAsync(BlacklistEntryId blacklistEntryId);

        /// <summary>
        /// Delete a blacklist entry by its number.
        /// </summary>
        /// <param name="numberInfo"></param>
        void DeleteByNumber(NumberInfo numberInfo);

        /// <summary>
        /// Delete a blacklist entry by its number (async).
        /// </summary>
        /// <param name="numberInfo"></param>
        Task DeleteByNumberAsync(NumberInfo numberInfo);

        /// <summary>
        /// Gets all blacklist entries.
        /// </summary>
        /// <returns></returns>
        List<BlacklistEntry> GetAll();

        /// <summary>
        /// Gets all blacklist entries (async).
        /// </summary>
        /// <returns></returns>
        Task<List<BlacklistEntry>> GetAllAsync();
    }

    internal class BlacklistApiMethods : IBlacklistApiMethods
    {
        private const string V4_blacklist = "/v4/blacklist";

        private readonly IApiRequestHelper _requestHelper;

        public BlacklistApiMethods(IApiRequestHelper requestHelper)
        {
            _requestHelper = requestHelper ?? throw new ArgumentNullException(nameof(requestHelper));
        }

        public BlacklistEntry Create(BlacklistEntryCreateInfo createInfo)
            => CreateInternal(createInfo: createInfo, mode: SyncMode.Sync).GetAwaiter().GetResult();

        public Task<BlacklistEntry> CreateAsync(BlacklistEntryCreateInfo createInfo)
            => CreateInternal(createInfo: createInfo, mode: SyncMode.Async);

        private async Task<BlacklistEntry> CreateInternal(BlacklistEntryCreateInfo createInfo, SyncMode mode)
        {
            GuardHelper.EnsureNotNullOrThrow(parameterName: nameof(createInfo), value: createInfo);

            const Method method = Method.POST;
            const string resource = V4_blacklist;
            var payload = new { NumberInfo = createInfo.NumberInfo, Comment = createInfo.Comment };

            return mode == SyncMode.Sync
                ? _requestHelper.Execute<BlacklistEntry>(method: method, resource: resource, payload: payload)
                : await _requestHelper.ExecuteAsync<BlacklistEntry>(method: method, resource: resource, payload: payload);
        }

        public List<BlacklistEntry> GetAll()
            => GetAllInternal(mode: SyncMode.Sync).GetAwaiter().GetResult();

        public Task<List<BlacklistEntry>> GetAllAsync()
            => GetAllInternal(mode: SyncMode.Async);

        private async Task<List<BlacklistEntry>> GetAllInternal(SyncMode mode)
        {
            var resource = $"{V4_blacklist}?pageLimit=250";

            return mode == SyncMode.Sync
                ? _requestHelper.ExecuteGetAndIteratePagedResult<BlacklistEntry>(resource: resource)
                : await _requestHelper.ExecuteGetAndIteratePagedResultAsync<BlacklistEntry>(resource: resource);
        }

        public BlacklistEntry GetById(BlacklistEntryId blacklistEntryId)
            => GetByIdInternal(blacklistEntryId: blacklistEntryId, mode: SyncMode.Sync).GetAwaiter().GetResult();

        public Task<BlacklistEntry> GetByIdAsync(BlacklistEntryId blacklistEntryId)
            => GetByIdInternal(blacklistEntryId: blacklistEntryId, mode: SyncMode.Async);

        private async Task<BlacklistEntry> GetByIdInternal(BlacklistEntryId blacklistEntryId, SyncMode mode)
        {
            GuardHelper.EnsureNotNullOrThrow(parameterName: nameof(blacklistEntryId), value: blacklistEntryId);

            const Method method = Method.GET;
            var resource = $"{V4_blacklist}/{blacklistEntryId}";

            return mode == SyncMode.Sync
                ? _requestHelper.Execute<BlacklistEntry>(method: method, resource: resource)
                : await _requestHelper.ExecuteAsync<BlacklistEntry>(method: method, resource: resource);
        }

        public BlacklistEntry GetByNumber(NumberInfo numberInfo)
            => GetByNumberInternal(numberInfo: numberInfo, mode: SyncMode.Sync).GetAwaiter().GetResult();

        public Task<BlacklistEntry> GetByNumberAsync(NumberInfo numberInfo)
            => GetByNumberInternal(numberInfo: numberInfo, mode: SyncMode.Async);

        private async Task<BlacklistEntry> GetByNumberInternal(NumberInfo numberInfo, SyncMode mode)
        {
            GuardHelper.EnsureNotNullOrThrow(parameterName: nameof(numberInfo), value: numberInfo);

            const Method method = Method.GET;
            var resource = $"{V4_blacklist}/bynumber?countryCode={numberInfo.CountryCode}&phoneNumber={numberInfo.PhoneNumber}";

            return mode == SyncMode.Sync
                ? _requestHelper.Execute<BlacklistEntry>(method: method, resource: resource)
                : await _requestHelper.ExecuteAsync<BlacklistEntry>(method: method, resource: resource);
        }

        public void DeleteById(BlacklistEntryId blacklistEntryId)
            => DeleteByIdInternal(blacklistEntryId: blacklistEntryId, mode: SyncMode.Sync).GetAwaiter().GetResult();

        public Task DeleteByIdAsync(BlacklistEntryId blacklistEntryId)
            => DeleteByIdInternal(blacklistEntryId: blacklistEntryId, mode: SyncMode.Async);

        private async Task DeleteByIdInternal(BlacklistEntryId blacklistEntryId, SyncMode mode)
        {
            GuardHelper.EnsureNotNullOrThrow(parameterName: nameof(blacklistEntryId), value: blacklistEntryId);

            const Method method = Method.DELETE;
            var resource = $"{V4_blacklist}/{blacklistEntryId}";

            if (mode == SyncMode.Sync)
                _requestHelper.ExecuteWithNoContent(method: method, resource: resource);
            else
                await _requestHelper.ExecuteWithNoContentAsync(method: method, resource: resource);
        }

        public void DeleteByNumber(NumberInfo numberInfo)
            => DeleteByNumberInternal(numberInfo: numberInfo, mode: SyncMode.Sync).GetAwaiter().GetResult();

        public Task DeleteByNumberAsync(NumberInfo numberInfo)
            => DeleteByNumberInternal(numberInfo: numberInfo, mode: SyncMode.Async);

        private async Task DeleteByNumberInternal(NumberInfo numberInfo, SyncMode mode)
        {
            GuardHelper.EnsureNotNullOrThrow(parameterName: nameof(numberInfo), value: numberInfo);

            const Method method = Method.DELETE;
            var resource = $"{V4_blacklist}/bynumber?countryCode={numberInfo.CountryCode}&phoneNumber={numberInfo.PhoneNumber}";

            if (mode == SyncMode.Sync)
                _requestHelper.ExecuteWithNoContent(method: method, resource: resource);
            else
                await _requestHelper.ExecuteWithNoContentAsync(method: method, resource: resource);
        }
    }
}