namespace InMobile.Sms.ApiClient
{
    public interface ISmsOutgoingClient
    {
        void SendSmsMessages();
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

        public void SendSmsMessages()
        {
            throw new System.NotImplementedException();
        }
    }


}
