using System;
using System.Collections.Generic;
using System.Text;

namespace InMobile.Sms.ApiClient
{
    public class NumberDetails
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
        /// <summary>
        /// The input msisdn in its unaltered format. This is the value provided when sending the text message.
        /// </summary>
        /// <example>"45 12 34 56 78"</example>
        public string? RawMsisdn { get; set; }
        /// <summary>
        /// True if the input msisdn was valid.
        /// </summary>
        /// <example>true</example>
        public bool IsValidMsisdn { get; set; }
        /// <summary>
        /// Specifies whether the message has been anonymized or not.
        /// </summary>
        /// <example>false</example>
        public bool IsAnonymized { get; set; }
    }
}
