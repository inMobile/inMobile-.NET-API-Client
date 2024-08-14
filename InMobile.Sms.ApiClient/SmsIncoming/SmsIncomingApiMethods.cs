using System;
using System.Threading.Tasks;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Incoming message specific operations.
    /// </summary>
    public interface ISmsIncomingApiMethods
    {
        /// <summary>
        /// Get 1 or more incoming messages awaiting to be fetched. After calling this method, the returned messages will be flagged as processed and never returned again.
        /// </summary>
        /// <param name="limit">The maximum amount of messages to receive. Limit bust be between 1 and 250.</param>
        IncomingMessageList GetMessages(int limit);

        /// <summary>
        /// Get 1 or more incoming messages awaiting to be fetched. After calling this method, the returned messages will be flagged as processed and never returned again (async).
        /// </summary>
        /// <param name="limit">The maximum amount of messages to receive. Limit bust be between 1 and 250.</param>
        Task<IncomingMessageList> GetMessagesAsync(int limit);
    }

    internal class SmsIncomingApiMethods : ISmsIncomingApiMethods
    {
        private const string V4_sms_incoming = "/v4/sms/incoming";

        private readonly IApiRequestHelper _requestHelper;

        public SmsIncomingApiMethods(IApiRequestHelper requestHelper)
        {
            _requestHelper = requestHelper ?? throw new ArgumentNullException(nameof(requestHelper));
        }

        public IncomingMessageList GetMessages(int limit)
            => GetMessagesInternal(limit: limit, mode: SyncMode.Sync).GetAwaiter().GetResult();

        public Task<IncomingMessageList> GetMessagesAsync(int limit)
            => GetMessagesInternal(limit: limit, mode: SyncMode.Async);

        private async Task<IncomingMessageList> GetMessagesInternal(int limit, SyncMode mode)
        {
            if (limit <= 0 || limit > 250)
                throw new ArgumentException($"Invalid limit value: {limit}. Value must be between 1 and 250.");

            const Method method = Method.GET;
            var resource = $"{V4_sms_incoming}/messages?limit={limit}";

            return mode == SyncMode.Sync
                ? _requestHelper.Execute<IncomingMessageList>(method: method, resource: resource)
                : await _requestHelper.ExecuteAsync<IncomingMessageList>(method: method, resource: resource);
        }
    }
}