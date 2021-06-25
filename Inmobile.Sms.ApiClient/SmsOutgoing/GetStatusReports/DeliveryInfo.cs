using System;

namespace InMobile.Sms.ApiClient
{
    public class DeliveryInfo
    {
        public MessageStateCode StateCode { get; set; }
        public string? StateDescription { get; set; }

        public int? ErrorCode { get; set; }

        /// <summary>
        /// A description describing the error if the state is not "DELIVERED". This property will be absent in case the state is "DELIVERED".
        /// </summary>
        /// <example>Undeliverable message</example>
        public string? ErrorDescription { get; set; }
    }

    
}
