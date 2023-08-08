using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Number info
    /// </summary>
    public class NumberInfo
    {
        /// <summary>
        /// The country code part of the msisdn, e.g. 45.
        /// </summary>
        /// <example>"45"</example>
        public string CountryCode { get; set; }
        /// <summary>
        /// The phone number part of the msisdn, e.g. 12345678.
        /// </summary>
        /// <example>"12345678"</example>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Create a new NumberInfo object
        /// </summary>
        /// <param name="countryCode">The country code part of the msisdn, e.g. 45.</param>
        /// <param name="phoneNumber">The phone number part of the msisdn, e.g. 12345678.</param>
        public NumberInfo(string countryCode, string phoneNumber)
        {
            if (string.IsNullOrEmpty(countryCode))
            {
                throw new System.ArgumentException($"'{nameof(countryCode)}' cannot be null or empty.", nameof(countryCode));
            }

            if (string.IsNullOrEmpty(phoneNumber))
            {
                throw new System.ArgumentException($"'{nameof(phoneNumber)}' cannot be null or empty.", nameof(phoneNumber));
            }

            CountryCode = countryCode;
            PhoneNumber = phoneNumber;
        }

        // NOTE: This constructor must exist for serialization
        [JsonConstructor]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        internal NumberInfo()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }
    }
}
