using System;
using System.Collections.Generic;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Outgoing message specific operations.
    /// </summary>
    public interface ISmsOutgoingApiMethod
    {
        /// <summary>
        /// Send sms messages to one or more recipients.
        /// </summary>
        /// <param name="messageList"></param>
        /// <returns></returns>
        ResultsList<OutgoingSmsMessageCreateResult> SendSmsMessages(List<OutgoingSmsMessageCreateInfo> messageList);

        /// <summary>
        /// Send sms messages using a template.
        /// </summary>
        /// <param name="templateCreateInfo"></param>
        /// <returns></returns>
        OutgoingSmsTemplateCreateResult SendSmsMessagesUsingTemplate(OutgoingSmsTemplateCreateInfo templateCreateInfo);

        /// <summary>
        /// Cancel future scheduled messages.
        /// </summary>
        /// <param name="messageIds"></param>
        /// <returns></returns>
        ResultsList<CancelMessageResult> CancelMessages(List<OutgoingMessageId> messageIds);

        /// <summary>
        /// Get 1 or more pending status reports awaiting to be fetched. After calling this method, the returned reports will be flagged as processed and never returned again.
        /// </summary>
        /// <param name="limit">The maximum amount of reports to receive. Limit bust be between 1 and 250.</param>
        /// <returns></returns>
        ReportsList<StatusReport> GetStatusReports(int limit);
    }

    internal class SmsOutgoingMethods : ISmsOutgoingApiMethod
    {
        private const string V4_sms_outgoing = "/v4/sms/outgoing";

        private readonly IApiRequestHelper _requestHelper;

        public SmsOutgoingMethods(IApiRequestHelper requestHelper)
        {
            _requestHelper = requestHelper ?? throw new ArgumentNullException(nameof(requestHelper));
        }

        public ResultsList<CancelMessageResult> CancelMessages(List<OutgoingMessageId> messageIds)
        {
            if (messageIds is null)
                throw new ArgumentNullException(nameof(messageIds));

            return _requestHelper.Execute<ResultsList<CancelMessageResult>>(
                method: Method.POST,
                resource: $"{V4_sms_outgoing}/cancel",
                payload: new
                {
                    MessageIds = messageIds
                });
        }
        
        public ReportsList<StatusReport> GetStatusReports(int limit)
        {
            if (limit <= 0 || limit > 250)
                throw new ArgumentException($"Invalid limit value: {limit}. Value must be between 1 and 250.");

            return _requestHelper.Execute<ReportsList<StatusReport>>(
                method: Method.GET,
                resource: $"{V4_sms_outgoing}/reports?limit={limit}");
        }

        public ResultsList<OutgoingSmsMessageCreateResult> SendSmsMessages(List<OutgoingSmsMessageCreateInfo> messageList)
        {
            if (messageList is null)
                throw new ArgumentNullException(nameof(messageList));

            return _requestHelper.Execute<ResultsList<OutgoingSmsMessageCreateResult>>(
                method: Method.POST,
                resource: $"{V4_sms_outgoing}",
                payload: new
                {
                    Messages = messageList
                });
        }

        public OutgoingSmsTemplateCreateResult SendSmsMessagesUsingTemplate(OutgoingSmsTemplateCreateInfo templateCreateInfo)
        {
            if (templateCreateInfo is null)
                throw new ArgumentNullException(nameof(templateCreateInfo));

            return _requestHelper.Execute<OutgoingSmsTemplateCreateResult>(
                method: Method.POST,
                resource: $"{V4_sms_outgoing}/sendusingtemplate",
                payload: templateCreateInfo);
        }
    }
}
