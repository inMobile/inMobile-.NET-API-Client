using System;
using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// A status report for an outgoing message.
    /// </summary>
    public class StatusReport
    {
        /// <summary>
        /// The id of the message.
        /// </summary>
        /// <example>id1</example>
        [JsonProperty]
        public OutgoingMessageId MessageId { get; private set; }

        /// <summary>
        /// The detailed number information.
        /// </summary>
        [JsonProperty]
        public NumberDetails NumberDetails { get; private set; }

        /// <summary>
        /// Information about the delivery state of the outgoing message.
        /// </summary>
        [JsonProperty]
        public DeliveryInfo DeliveryInfo { get; private set; }

        /// <summary>
        /// Charging info about the outgoing message.
        /// </summary>
        [JsonProperty]
        public ChargeInfo ChargeInfo { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [JsonConstructor]
        private StatusReport()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }
    }
}
