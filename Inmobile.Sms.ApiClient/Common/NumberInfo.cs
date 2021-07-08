namespace InMobile.Sms.ApiClient
{
    public class NumberInfo
    {
        /// <summary>
        /// The country code part of the msisdn, e.g. 45.
        /// </summary>
        /// <example>"45"</example>
        public string? CountryCode { get; set; }
        /// <summary>
        /// The phone number part of the msisdn, e.g. 12345678.
        /// </summary>
        /// <example>"12345678"</example>
        public string? PhoneNumber { get; set; }

        public NumberInfo(string? countryCode, string? phoneNumber)
        {
            CountryCode = countryCode;
            PhoneNumber = phoneNumber;
        }

        // NOTE: This constructor must exist for serialization
        public NumberInfo()
        {
        }
    }
}
