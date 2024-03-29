using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Details of the target recipient of the incoming message
    /// </summary> 
    public sealed class IncomingMessageTarget
    {
        /// <summary>
        /// Country code of the target recipient
        /// </summary>
        [JsonProperty]
        public string CountryCode { get; private set; }
        
        /// <summary>
        /// Phone number of the target recipient
        /// </summary>
        [JsonProperty]
        public string PhoneNumber { get; private set;}
        
        /// <summary>
        /// Msisdn of the target recipient
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