using System;
using System.Collections.Generic;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Create information for message for sending SMS using template.
    /// </summary>
    public class OutgoingSmsTemplateMessageCreateInfo
    {
        /// <summary>
        /// The msisdn (country code and number) to send to. (Remember to include countrycode in all numbers, e.g. 4512345678).
        /// If max length is exceeded, the entire api call will fail.
        /// </summary>
        /// <example>"4512345678"</example>
        public string To { get; }

        /// <summary>
        /// For optimal phone number validation, we encourage you to provide us with a country code. This can be the numeric country code (like 44) or the two-letter suffix (like GB). 
        /// If this field is empty it is important that you add the country code (e.g 44) in front of the phone number in “to”.
        /// </summary>
        /// <example>"45"</example>
        public string? CountryHint { get; }

        /// <summary>
        /// An optional message id used to identify the message.
        /// If no message id is provided, a new message id is generated and assigned to the message. This id must be unique across all messages created on the same account.
        /// (In case a previous message has been deleted according to GDPR deletion rules setup on the specific account, the messageId is allowed to be reused)
        /// If max length is exceeded, the entire api call will fail.
        /// Max length: 50
        /// </summary>
        /// <example>PetShop</example>
        public OutgoingMessageId? MessageId { get; }

        /// <summary>
        /// If true, this message will be blocked from sending if the target number is on the account's blacklist. If false, the message will be sent no matter blacklist settings.
        /// Default: true
        /// </summary>
        public bool RespectBlacklist { get; }

        /// <summary>
        /// The validity period in seconds. Minimum is 60 seconds and maximum is 172800 (48 hours).
        /// If not specified, the messages is attempted to be delivered in 48 hours.
        /// </summary>
        /// <example>90</example>
        public int? ValidityPeriodInSeconds { get; }

        /// <summary>
        /// If set, the message will be cancelled if the same mobile number already have received a SMS within this specified time period. Fx. used to prevent spamming a mobile number.
        /// Minimum 60 minutes (1 hour) and maximum is 43200 minutes (30 days).
        /// </summary>
        /// <example>60</example>
        public int? MsisdnCooldownInMinutes { get; }

        /// <summary>
        /// An optional callback url. If specified, this url is called with a status report when the message has reached its final status (either delivered, failed or cancelled).
        /// NOTE: Callbacks happen in bulks.Reports for message with identical callback urls can happen to be grouped in single callbacks.
        /// </summary>
        public string? StatusCallbackUrl { get; }

        /// <summary>
        /// If specified, this represents the future send time of the message.
        /// </summary>
        public string? SendTime { get; }

        /// <summary>
        /// A key-value list of placeholders to replace in the template text. Keys must be encapsulated with {}. E.g. {NAME}.
        /// </summary>
        public Dictionary<string, string> Placeholders { get; }

        /// <summary>
        /// </summary>
        /// <param name="placeholders"></param>
        /// <param name="to"></param>
        /// <param name="countryHint"></param>
        /// <param name="messageId"></param>
        /// <param name="respectBlacklist"></param>
        /// <param name="validityPeriod"></param>
        /// <param name="statusCallbackUrl"></param>
        /// <param name="sendTime"></param>
        /// <param name="msisdnCooldown">The msisdn cooldown period. Minimum 60 minutes (1 hour) and maximum is 43200 minutes (30 days).</param>
        public OutgoingSmsTemplateMessageCreateInfo(Dictionary<string, string> placeholders, string to, string? countryHint = null, OutgoingMessageId? messageId = null, bool respectBlacklist = true, TimeSpan? validityPeriod = null, string? statusCallbackUrl = null, DateTime? sendTime = null, TimeSpan? msisdnCooldown = null)
        {
            Placeholders = placeholders;
            To = to;
            CountryHint = countryHint;
            MessageId = messageId;
            RespectBlacklist = respectBlacklist;

            if (validityPeriod != null)
            {
                ValidityPeriodInSeconds = (int)validityPeriod.Value.TotalSeconds;
            }
            if (msisdnCooldown != null)
            {
                MsisdnCooldownInMinutes = (int)msisdnCooldown.Value.TotalMinutes;
            }

            StatusCallbackUrl = statusCallbackUrl;

            if (sendTime != null)
            {
                SendTime = sendTime.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH':'mm':'ssZ");
            }
        }
    }
}
