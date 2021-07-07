using System;

namespace InMobile.Sms.ApiClient
{
    public class DeliveryInfo
    {
        /// <summary>
        /// Possible values are: "DELIVERED", "FAILED", "CANCELLED"
        /// </summary>
        /// <example>FAILED</example>
        public MessageState State { get; set; }

        public int? ErrorCode { get; set; }

        /// <summary>
        /// A description describing the error if the state is not "DELIVERED". This property will be absent in case the state is "DELIVERED".
        /// </summary>
        /// <example>Undeliverable message</example>
        public string? ErrorDetails { get; set; }
    }

    
}
