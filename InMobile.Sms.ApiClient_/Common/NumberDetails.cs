using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    public class NumberDetails : NumberInfo
    {
        /// <summary>
        /// The input msisdn in its unaltered format. This is the value provided when sending the text message.
        /// </summary>
        /// <example>"45 12 34 56 78"</example>
        [JsonProperty]
        public string RawMsisdn { get; private set; }
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

        [JsonConstructor]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public NumberDetails()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }
    }
}
