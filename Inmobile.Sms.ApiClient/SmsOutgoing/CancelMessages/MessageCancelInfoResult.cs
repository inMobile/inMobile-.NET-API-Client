using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    public class CancelMessageResult
    {
        [JsonProperty]
        public OutgoingMessageId MessageId { get; private set; }
        [JsonProperty]
        public CancelResultCode ResultCode { get; private set; }
        [JsonProperty]
        public string? ResultDescription { get; private set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public CancelMessageResult()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }

    }

    public enum CancelResultCode : int
    {
        Unknown = 0,
        Success = 1,
        NotCancelled = -1,
        MessageNotFound = -2
    }
}
