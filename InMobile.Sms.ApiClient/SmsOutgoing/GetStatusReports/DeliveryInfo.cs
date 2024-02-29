using System;
using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Information related to the delivery of an outgoing message.
    /// </summary>
    public class DeliveryInfo
    {
        /// <summary>
        /// Specification of the delivery state
        /// </summary>
        [JsonProperty]
        public MessageStateCode StateCode { get; private set; }

        /// <summary>
        /// A human readable description of the state.
        /// </summary>
        [JsonProperty]
        public string StateDescription { get; private set; }

        /// <summary>
        /// When the message was sent
        /// </summary>
        [JsonProperty]
        public DateTime? SendTime { get; private set; }
        
        /// <summary>
        /// When the delivery of the message fails, is cancelled, or is delivered. If the operator does not support reporting the actual delivery time, this will be the time when the delivery report is received from the operator.
        /// In some cases where the message is not sent, this value can be null.
        /// </summary>
        [JsonProperty]
        public DateTime? DoneTime { get; private set; }

        /// <summary>
        /// A code  describing the type of error.
        /// </summary>
        [JsonProperty]
        public int? ErrorCode { get; private set; }

        /// <summary>
        /// A description describing the error if the state is not "DELIVERED". This property will be absent in case the state is "DELIVERED".
        /// </summary>
        /// <example>Undeliverable message</example>
        [JsonProperty]
        public string? ErrorDescription { get; private set; }

        [JsonConstructor]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private DeliveryInfo()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }
    }
}
