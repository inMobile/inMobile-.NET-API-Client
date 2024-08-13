using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Outgoing message specific operations.
    /// </summary>
    public interface ISmsOutgoingApiMethods
    {
        /// <summary>
        /// Send sms messages to one or more recipients.
        /// </summary>
        /// <param name="messageList"></param>
        /// <returns></returns>
        ResultsList<OutgoingSmsMessageCreateResult> SendSmsMessages(List<OutgoingSmsMessageCreateInfo> messageList);

        /// <summary>
        /// Send sms messages to one or more recipients (async).
        /// </summary>
        /// <param name="messageList"></param>
        /// <returns></returns>
        Task<ResultsList<OutgoingSmsMessageCreateResult>> SendSmsMessagesAsync(List<OutgoingSmsMessageCreateInfo> messageList);

        /// <summary>
        /// Send sms messages using a template.
        /// </summary>
        /// <param name="templateCreateInfo"></param>
        /// <returns></returns>
        OutgoingSmsTemplateCreateResult SendSmsMessagesUsingTemplate(OutgoingSmsTemplateCreateInfo templateCreateInfo);

        /// <summary>
        /// Send sms messages using a template (async).
        /// </summary>
        /// <param name="templateCreateInfo"></param>
        /// <returns></returns>
        Task<OutgoingSmsTemplateCreateResult> SendSmsMessagesUsingTemplateAsync(OutgoingSmsTemplateCreateInfo templateCreateInfo);

        /// <summary>
        /// Cancel future scheduled messages.
        /// </summary>
        /// <param name="messageIds"></param>
        /// <returns></returns>
        ResultsList<CancelMessageResult> CancelMessages(List<OutgoingMessageId> messageIds);

        /// <summary>
        /// Cancel future scheduled messages (async).
        /// </summary>
        /// <param name="messageIds"></param>
        /// <returns></returns>
        Task<ResultsList<CancelMessageResult>> CancelMessagesAsync(List<OutgoingMessageId> messageIds);

        /// <summary>
        /// Get 1 or more pending status reports awaiting to be fetched. After calling this method, the returned reports will be flagged as processed and never returned again.
        /// </summary>
        /// <param name="limit">The maximum amount of reports to receive. Limit bust be between 1 and 250.</param>
        ReportsList<StatusReport> GetStatusReports(int limit);

        /// <summary>
        /// Get 1 or more pending status reports awaiting to be fetched. After calling this method, the returned reports will be flagged as processed and never returned again (async).
        /// </summary>
        /// <param name="limit">The maximum amount of reports to receive. Limit bust be between 1 and 250.</param>
        Task<ReportsList<StatusReport>> GetStatusReportsAsync(int limit);
    }

    internal class SmsOutgoingApiMethods : ISmsOutgoingApiMethods
    {
        private const string V4_sms_outgoing = "/v4/sms/outgoing";

        private readonly IApiRequestHelper _requestHelper;

        public SmsOutgoingApiMethods(IApiRequestHelper requestHelper)
        {
            _requestHelper = requestHelper ?? throw new ArgumentNullException(nameof(requestHelper));
        }

        public ResultsList<CancelMessageResult> CancelMessages(List<OutgoingMessageId> messageIds)
            => CancelMessagesInternal(messageIds: messageIds, mode: SyncMode.Sync).GetAwaiter().GetResult();

        public Task<ResultsList<CancelMessageResult>> CancelMessagesAsync(List<OutgoingMessageId> messageIds)
            => CancelMessagesInternal(messageIds: messageIds, mode: SyncMode.Async);

        private async Task<ResultsList<CancelMessageResult>> CancelMessagesInternal(List<OutgoingMessageId> messageIds, SyncMode mode)
        {
            GuardHelper.EnsureNotNullOrThrow(nameof(messageIds), messageIds);

            const Method method = Method.POST;
            var resource = $"{V4_sms_outgoing}/cancel";
            var payload = new { MessageIds = messageIds };

            return mode == SyncMode.Sync
                ? _requestHelper.Execute<ResultsList<CancelMessageResult>>(method: method, resource: resource, payload: payload)
                : await _requestHelper.ExecuteAsync<ResultsList<CancelMessageResult>>(method: method, resource: resource, payload: payload);
        }

        public ReportsList<StatusReport> GetStatusReports(int limit)
            => GetStatusReportsInternal(limit: limit, mode: SyncMode.Sync).GetAwaiter().GetResult();

        public Task<ReportsList<StatusReport>> GetStatusReportsAsync(int limit)
            => GetStatusReportsInternal(limit: limit, mode: SyncMode.Async);

        private async Task<ReportsList<StatusReport>> GetStatusReportsInternal(int limit, SyncMode mode)
        {
            if (limit <= 0 || limit > 250)
                throw new ArgumentException($"Invalid limit value: {limit}. Value must be between 1 and 250.");

            const Method method = Method.GET;
            var resource = $"{V4_sms_outgoing}/reports?limit={limit}";

            return mode == SyncMode.Sync
                ? _requestHelper.Execute<ReportsList<StatusReport>>(method: method, resource: resource)
                : await _requestHelper.ExecuteAsync<ReportsList<StatusReport>>(method: method, resource: resource);
        }

        public ResultsList<OutgoingSmsMessageCreateResult> SendSmsMessages(List<OutgoingSmsMessageCreateInfo> messageList)
            => SendSmsMessagesInternal(messageList: messageList, mode: SyncMode.Sync).GetAwaiter().GetResult();

        public Task<ResultsList<OutgoingSmsMessageCreateResult>> SendSmsMessagesAsync(List<OutgoingSmsMessageCreateInfo> messageList)
            => SendSmsMessagesInternal(messageList: messageList, mode: SyncMode.Async);

        private async Task<ResultsList<OutgoingSmsMessageCreateResult>> SendSmsMessagesInternal(List<OutgoingSmsMessageCreateInfo> messageList, SyncMode mode)
        {
            GuardHelper.EnsureNotNullOrThrow(nameof(messageList), messageList);

            const Method method = Method.POST;
            const string resource = V4_sms_outgoing;
            var payload = new { Messages = messageList };

            return mode == SyncMode.Sync
                ? _requestHelper.Execute<ResultsList<OutgoingSmsMessageCreateResult>>(method: method, resource: resource, payload: payload)
                : await _requestHelper.ExecuteAsync<ResultsList<OutgoingSmsMessageCreateResult>>(method: method, resource: resource, payload: payload);
        }

        public OutgoingSmsTemplateCreateResult SendSmsMessagesUsingTemplate(OutgoingSmsTemplateCreateInfo templateCreateInfo)
            => SendSmsMessagesUsingTemplateInternal(templateCreateInfo: templateCreateInfo, mode: SyncMode.Sync).GetAwaiter().GetResult();

        public Task<OutgoingSmsTemplateCreateResult> SendSmsMessagesUsingTemplateAsync(OutgoingSmsTemplateCreateInfo templateCreateInfo)
            => SendSmsMessagesUsingTemplateInternal(templateCreateInfo: templateCreateInfo, mode: SyncMode.Async);

        private async Task<OutgoingSmsTemplateCreateResult> SendSmsMessagesUsingTemplateInternal(OutgoingSmsTemplateCreateInfo templateCreateInfo, SyncMode mode)
        {
            GuardHelper.EnsureNotNullOrThrow(nameof(templateCreateInfo), templateCreateInfo);

            const Method method = Method.POST;
            var resource = $"{V4_sms_outgoing}/sendusingtemplate";

            return mode == SyncMode.Sync
                ? _requestHelper.Execute<OutgoingSmsTemplateCreateResult>(method: method, resource: resource, payload: templateCreateInfo)
                : await _requestHelper.ExecuteAsync<OutgoingSmsTemplateCreateResult>(method: method, resource: resource, payload: templateCreateInfo);
        }
    }
}