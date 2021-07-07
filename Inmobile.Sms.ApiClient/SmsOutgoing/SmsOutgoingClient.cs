using System;
using System.Collections.Generic;
using RestSharp;

namespace InMobile.Sms.ApiClient
{
    public interface ISmsOutgoingClient
    {
        ResultsList<OutgoingSmsMessageCreateResponse> SendSmsMessages(List<OutgoingSmsMessageCreateRequest> messageList, string statusCallbackUrl = null);
        void CancelMessages();
        void GetStatusReports();
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

        public void GetStatusReports()
        {
            throw new NotImplementedException();
        }

        public ResultsList<OutgoingSmsMessageCreateResponse> SendSmsMessages(List<OutgoingSmsMessageCreateRequest> messageList, string statusCallbackUrl = null)
        {
            var response = _requestHelper.Execute<ResultsList<OutgoingSmsMessageCreateResponse>>(
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
            return response;
        }
    }
}
