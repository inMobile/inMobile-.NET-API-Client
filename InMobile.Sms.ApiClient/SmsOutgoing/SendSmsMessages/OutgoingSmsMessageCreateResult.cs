using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Result info for a created outgoing message.
    /// </summary>
    public class OutgoingSmsMessageCreateResult
    {
        /// <summary>
        /// The parsed details about the msisdn provided in the to-field of the request (msisdn is a number with a countrycode e.g. 4512345678)
        /// </summary>
        [JsonProperty]
        public NumberDetails NumberDetails { get; private set; }

        /// <summary>
        /// The text message.
        /// If the max length is exceeded (10,000 chars), the message text is truncated and sent.
        /// </summary>
        [JsonProperty]
        public string Text { get; private set; }

        /// <summary>
        /// The sender. This can either be a 3-11 chars text sender or an up to 14 digit long sender number.
        /// If the max length is exceeded, the string is truncated.
        /// </summary>
        /// <example>PetShop</example>
        [JsonProperty]
        public string From { get; private set; }

        /// <summary>
        /// The number of sms messages this message will be split into when sent to the operator. Charging will also be done according to this number.
        /// </summary>
        /// <example>1</example>
        [JsonProperty]
        public int SmsCount { get; private set; }

        /// <summary>
        /// An optional message id used to identify the message.
        /// If no message id was provided when sending the message, a new message id has been generated and assigned to the message. This id is unique across all messages created on the same account.
        /// </summary>
        /// <example>PetShop</example>
        [JsonProperty]
        public OutgoingMessageId MessageId { get; private set; }

        /// <summary>
        /// The encoding of the message. Can be either "gsm7", "ucs2" or "auto".
        /// </summary>
        /// <example>gsm7</example>
        [JsonProperty]
        public MessageEncoding Encoding { get; private set; }

        [JsonConstructor]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private OutgoingSmsMessageCreateResult()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }
    }
}
