﻿using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Number details
    /// </summary>
    public class NumberDetails : NumberInfo
    {
        /// <summary>
        /// The input msisdn in its unaltered format. This is the value provided when sending the text message.
        /// </summary>
        /// <example>"45 12 34 56 78"</example>
        [JsonProperty]
        public string RawMsisdn { get; private set; }

        /// <summary>
        /// The final cleaned msisdn.
        /// </summary>
        /// <example>"4512345678"</example>
        [JsonProperty]
        public string Msisdn { get; private set; }

        /// <summary>
        /// True if the input msisdn was valid.
        /// </summary>
        /// <example>true</example>
        [JsonProperty]
        public bool IsValidMsisdn { get; private set; }

        /// <summary>
        /// Specifies whether the message has been anonymized or not.
        /// </summary>
        /// <example>false</example>
        [JsonProperty]
        public bool IsAnonymized { get; private set; }

        /// <summary>
        /// The country code hint if provided in the request.
        /// </summary>
        /// <example>"45"</example>
        [JsonProperty]
        public string CountryHint { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [JsonConstructor]
        internal NumberDetails()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }
    }
}
