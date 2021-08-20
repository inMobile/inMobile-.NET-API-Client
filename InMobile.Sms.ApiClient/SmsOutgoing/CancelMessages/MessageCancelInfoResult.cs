using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Represents the result of a cancel attempt.
    /// </summary>
    public class CancelMessageResult
    {
        /// <summary>
        /// The id of the outgoing message.
        /// </summary>
        [JsonProperty]
        public OutgoingMessageId MessageId { get; private set; }

        /// <summary>
        /// The code specifying whether the cancel was a success or not.
        /// </summary>
        [JsonProperty]
        public CancelResultCode ResultCode { get; private set; }
        /// <summary>
        /// A readable description of the result.
        /// </summary>
        [JsonProperty]
        public string? ResultDescription { get; private set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [JsonConstructor]
        private CancelMessageResult()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }

    }

    /// <summary>
    /// Cancel result code
    /// </summary>
    public enum CancelResultCode : int
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// The message was successfully cancelled.
        /// </summary>
        Success = 1,
        /// <summary>
        /// The message was found but not cancelled. This can happen if the message has already been sent.
        /// </summary>
        NotCancelled = -1,
        /// <summary>
        /// The message id specified did not match any messages in the database. This can happen if the id was not valid or if the message was deleted in the system.
        /// </summary>
        MessageNotFound = -2
    }
}
