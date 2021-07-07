using System;

namespace InMobile.Sms.ApiClient
{
    public class StatusReport
    {
        /// <summary>
        /// The id of the message.
        /// </summary>
        /// <example>id1</example>
        public string MessageId { get; }
        public NumberDetails NumberDetails { get; }
        public DeliveryInfo DeliveryInfo { get; }
        public ChargeInfo ChargeInfo { get; }

        public StatusReport(string messageId, NumberDetails numberDetails, DeliveryInfo deliveryInfo, ChargeInfo chargeInfo)
        {
            if (string.IsNullOrWhiteSpace(messageId))
            {
                throw new ArgumentException($"'{nameof(messageId)}' cannot be null or whitespace", nameof(messageId));
            }

            MessageId = messageId;
            NumberDetails = numberDetails ?? throw new ArgumentNullException(nameof(numberDetails));
            DeliveryInfo = deliveryInfo ?? throw new ArgumentNullException(nameof(deliveryInfo));
            ChargeInfo = chargeInfo ?? throw new ArgumentNullException(nameof(chargeInfo));
        }
    }
}
