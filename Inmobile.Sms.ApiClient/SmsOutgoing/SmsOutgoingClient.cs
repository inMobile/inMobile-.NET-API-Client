using System;
using System.Collections.Generic;
using RestSharp;

namespace InMobile.Sms.ApiClient
{
    public interface ISmsOutgoingClient
    {
        ResultsList<OutgoingSmsMessageCreateResponse> SendSmsMessages(List<OutgoingSmsMessageCreateRequest> messageList, string statusCallbackUrl = null);
        void CancelMessages();
        ReportsList<StatusReport> GetStatusReports();
    }

    internal class SmsOutgoingClient : ISmsOutgoingClient
    {
        private readonly IApiRequestHelper _requestHelper;

        public SmsOutgoingClient(IApiRequestHelper requestHelper)
        {
            _requestHelper = requestHelper ?? throw new System.ArgumentNullException(nameof(requestHelper));
        }
        public void CancelMessages()
        {
            throw new System.NotImplementedException();
        }

        public ReportsList<StatusReport> GetStatusReports()
        {
            return _requestHelper.Execute<ReportsList<StatusReport>>(
                method: Method.GET,
                resource: "/v4/sms/outgoing/reports",
                payload: null
                );
        }

        public ResultsList<OutgoingSmsMessageCreateResponse> SendSmsMessages(List<OutgoingSmsMessageCreateRequest> messageList, string statusCallbackUrl = null)
        {
            return _requestHelper.Execute<ResultsList<OutgoingSmsMessageCreateResponse>>(
                method: Method.POST,
                resource: "/v4/sms/outgoing",
                payload: new
                {
                    Messages = messageList,
                    StatusCallback = new
                    {
                        url = statusCallbackUrl
                    }
                });
        }
    }
}
