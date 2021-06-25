using System;
using System.Collections.Generic;
using RestSharp;

namespace InMobile.Sms.ApiClient
{
    public interface ISmsOutgoingApiMethod
    {
        ResultsList<OutgoingSmsMessageCreateResponse> SendSmsMessages(List<OutgoingSmsMessageCreateRequest> messageList);
        ResultsList<MessageCancelResult> CancelMessages(List<string> messageIds);
        ReportsList<StatusReport> GetStatusReports();
    }

    internal class SmsOutgoingMethods : ISmsOutgoingApiMethod
    {
        private readonly IApiRequestHelper _requestHelper;

        public SmsOutgoingMethods(IApiRequestHelper requestHelper)
        {
            _requestHelper = requestHelper ?? throw new System.ArgumentNullException(nameof(requestHelper));
        }

        public ResultsList<MessageCancelResult> CancelMessages(List<string> messageIds)
        {
            if (messageIds is null)
            {
                throw new ArgumentNullException(nameof(messageIds));
            }

            return _requestHelper.Execute<ResultsList<MessageCancelResult>>(
                method: Method.POST,
                resource: "/v4/sms/outgoing/cancel",
                payload: new
                {
                    MessageIds = messageIds
                });
        }

        public ReportsList<StatusReport> GetStatusReports()
        {
            return _requestHelper.Execute<ReportsList<StatusReport>>(
                method: Method.GET,
                resource: "/v4/sms/outgoing/reports"
                );
        }

        public ResultsList<OutgoingSmsMessageCreateResponse> SendSmsMessages(List<OutgoingSmsMessageCreateRequest> messageList)
        {
            if (messageList is null)
            {
                throw new ArgumentNullException(nameof(messageList));
            }

            return _requestHelper.Execute<ResultsList<OutgoingSmsMessageCreateResponse>>(
                method: Method.POST,
                resource: "/v4/sms/outgoing",
                payload: new
                {
                    Messages = messageList
                });
        }
    }
}
