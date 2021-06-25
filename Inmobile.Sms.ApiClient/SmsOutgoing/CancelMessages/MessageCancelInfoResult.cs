namespace InMobile.Sms.ApiClient
{
    public class CancelMessageResult
    {
        public string? MessageId { get; set; }
        public CancelResultCode ResultCode { get; set; }
        public string? ResultDescription { get; set; }
    }

    public enum CancelResultCode : int
    {
        Unknown = 0,
        Success = 1,
        NotCancelled = -1,
        MessageNotFound = -2
    }
}
