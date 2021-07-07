using System.Collections.Generic;
using RestSharp;

namespace InMobile.Sms.ApiClient
{
    public interface ISmsOutgoingClient
    {
        List<OutgoingSmsMessageCreateResponse> SendSmsMessages(List<OutgoingSmsMessageCreateRequest> messageList, string statusCallbackUrl = null);
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
            throw new System.NotImplementedException();
        }

        public List<OutgoingSmsMessageCreateResponse> SendSmsMessages(List<OutgoingSmsMessageCreateRequest> messageList, string statusCallbackUrl = null)
        {
            var response = _requestHelper.Execute<GenericApiListResponse<OutgoingSmsMessageCreateResponse>>(
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
            return response.Results;
        }
    }

    public class GenericApiListResponse<T>
    {
        public List<T> Results { get; set; }
    }
}
