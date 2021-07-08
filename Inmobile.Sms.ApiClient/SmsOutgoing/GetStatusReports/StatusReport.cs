using System;

namespace InMobile.Sms.ApiClient
{
    public class StatusReport
    {
        /// <summary>
        /// The id of the message.
        /// </summary>
        /// <example>id1</example>
        public string? MessageId { get; set; }
        public NumberDetails? NumberDetails { get; set; }
        public DeliveryInfo? DeliveryInfo { get; set; }
        public ChargeInfo? ChargeInfo { get; set; }
    }
}
