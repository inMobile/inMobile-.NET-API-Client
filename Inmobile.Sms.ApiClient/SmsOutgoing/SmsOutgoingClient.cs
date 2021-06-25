namespace InMobile.Sms.ApiClient
{
    public interface ISmsOutgoingClient
    {
        void SendSmsMessages();
        void CancelMessages();
        void GetStatusReports();
    }

    public class SmsOutgoingClient : ISmsOutgoingClient
    {
        public SmsOutgoingClient(InmobileApiKey inmobileApiKey)
        {

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
