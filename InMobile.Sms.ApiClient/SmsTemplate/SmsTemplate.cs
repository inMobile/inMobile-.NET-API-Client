using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Info about a SMS template.
    /// </summary>
    public class SmsTemplate
    {
        /// <summary>
        /// The id of the SMS template
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public SmsTemplateId Id { get; private set; }

        /// <summary>
        /// The name of the SMS template
        /// </summary>
        [JsonProperty]
        public string Name { get; private set; }

        /// <summary>
        /// The SMS template text content.
        /// </summary>
        [JsonProperty]
        public string Text { get; private set; }

        /// <summary>
        /// The sendername of the SMS template.
        /// </summary>
        [JsonProperty]
        public string SenderName { get; private set; }

        /// <summary>
        /// The encoding of the message. Can be either "gsm7", "ucs2" or "auto".
        /// </summary>
        /// <example>gsm7</example>
        [JsonProperty]
        public MessageEncoding Encoding { get; private set; }

        /// <summary>
        /// The placeholders of the SMS template.
        /// </summary>
        [JsonProperty]
        public List<string> Placeholders { get; private set; }

        /// <summary>
        /// The created date of the SMS template.
        /// </summary>
        [JsonProperty]
        public DateTime Created { get; private set; }

        /// <summary>
        /// The last updated date of the SMS template.
        /// </summary>
        [JsonProperty]
        public DateTime LastUpdated { get; private set; }

        [JsonConstructor]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private SmsTemplate()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }
    }
}
