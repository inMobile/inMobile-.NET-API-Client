using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Info about a email template.
    /// </summary>
    public class EmailTemplate
    {
        /// <summary>
        /// The id of the email template
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public EmailTemplateId Id { get; private set; }

        /// <summary>
        /// The name of the email template
        /// </summary>
        [JsonProperty]
        public string Name { get; private set; }

        /// <summary>
        /// The email template HTML content.
        /// </summary>
        [JsonProperty]
        public string Html { get; private set; }

        /// <summary>
        /// The email template text content.
        /// </summary>
        [JsonProperty]
        public string Text { get; private set; }

        /// <summary>
        /// The subject of the email template.
        /// </summary>
        [JsonProperty]
        public string Subject { get; private set; }

        /// <summary>
        /// The preheader of the email template.
        /// </summary>
        [JsonProperty]
        public string Preheader { get; private set; }

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
        private EmailTemplate()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }
    }
}
