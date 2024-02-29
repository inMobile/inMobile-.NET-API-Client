using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Details of the sender of the incoming message
    /// </summary>
    public class IncomingMessageSender
    {
        /// <summary>
        /// Country code of the sender. Null if not available.
        /// </summary>
        [JsonProperty]
        public string? CountryCode { get; private set; }
        
        /// <summary>
        /// Phone number of the sender. Null if not available.
        /// </summary>
        [JsonProperty]
        public string? PhoneNumber { get; private set;}
        
        /// <summary>
        /// The raw details of the sender. This is the raw source of the sender, as it was received from the operator.
        /// </summary>
        [JsonProperty]
        public string RawSource { get; private set;}
        
        /// <summary>
        /// True if the source of the message is a valid msisdn, false otherwise.
        /// </summary>
        [JsonProperty]
        public bool IsValidMsisdn { get; private set;}

        [JsonConstructor]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private IncomingMessageSender()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }
    }
}