namespace InMobile.Sms.ApiClient
{
    public class MessageCancelResult
    {
        public string? MessageId { get; set; }
    }

    public enum CancelResultCode
    {
        Unknown = 0
    }
}
