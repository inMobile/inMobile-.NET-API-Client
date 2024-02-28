using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient.SmsIncoming.GetMessages
{
    /// <summary>
    /// Details of the recipient of the incoming message
    /// </summary> 
    public sealed class IncomingMessageTarget
    {
        /// <summary>
        /// Country code of the recipient
        /// </summary>
        [JsonProperty]
        public string CountryCode { get; private set; }
        
        /// <summary>
        /// Phone number of the recipient
        /// </summary>
        [JsonProperty]
        public string PhoneNumber { get; private set;}
        
        /// <summary>
        /// Msisdn of the recipient
        /// </summary>
        [JsonProperty]
        public string Msisdn { get; private set;}

        [JsonConstructor]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private IncomingMessageTarget()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }
    }
}