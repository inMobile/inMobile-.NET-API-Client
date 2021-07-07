using System;

namespace InMobile.Sms.ApiClient
{
    public class DeliveryInfo
    {
        /// <summary>
        /// Possible values are: "DELIVERED", "FAILED", "CANCELLED"
        /// </summary>
        /// <example>FAILED</example>
        public string State { get; }

        public string ErrorCode { get; }

        /// <summary>
        /// A description describing the error if the state is not "DELIVERED". This property will be absent in case the state is "DELIVERED".
        /// </summary>
        /// <example>Undeliverable message</example>
        public string? ErrorDetails { get; }

        public DeliveryInfo(string state, string errorCode, string? errorDetails = null)
        {
            if (string.IsNullOrEmpty(state))
            {
                throw new ArgumentException($"'{nameof(state)}' cannot be null or empty.", nameof(state));
            }

            State = state;
            ErrorCode = errorCode;
            ErrorDetails = errorDetails;
        }
    }

    
}
