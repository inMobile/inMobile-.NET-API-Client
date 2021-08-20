using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Information for creating a new outgoing message.
    /// </summary>
    public class OutgoingSmsMessageCreateInfo
    {
        /// <summary>
        /// The msisdn (country code and number) to send to. (Remember to include countrycode in all numbers, e.g. 4512345678).
        /// If max length is exceeded, the entire api call will fail.
        /// </summary>
        /// <example>"4512345678"</example>
        public string To { get; }

        /// <summary>
        /// The text message.
        /// If the max length is exceeded (10,000 chars), the message text is truncated and sent.
        /// </summary>
        /// <example>"This is a message text to be sent"</example>
        public string Text { get; }
        /// <summary>
        /// The sender. This can either be a 3-11 chars text sender or an up to 14 digit long sender number.
        /// If the max length is exceeded, the string is truncated.
        /// </summary>
        /// <example>PetShop</example>
        public string From { get; }

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
        /// If true, the message will be shown as a flash message (also known as a class0 message) on the target phone. If false, it will be received as a standard text message.
        /// Default: false
        /// </summary>
        /// <example>false</example>
        public bool Flash { get; }

        /// <summary>
        /// The encoding of the message. Can be either "gsm7", "ucs2" or "auto".
        /// "gsm7" is the default alfabet for text messages and when using gsm7, a single sms message can contain 160 characters. If the length exceeeds 160 characters, the message is actually split up into parts of 153 characters and charged according to this. Please note, that a few, specific characters fill up 2 bytes and count for 2 letters. Ref: https://en.wikipedia.org/wiki/GSM_03.38
        /// "ucs2"" allows for more non-roman characters to be used along with smileys. When using this encoding, a single message can consist of 70 characters. If the message exceeds 70 characters, the final message is actually split into parts of 67 characters.
        /// "auto" can be used in case the sender wishes to support non-roman characters but wants to save the expenses on all the trafic that only contains gsm characters anyway.
        /// Default: "gsm7"
        /// </summary>
        /// <example>gsm7</example>
        [JsonIgnore]
        public MessageEncoding Encoding { get; set; }

        /// <summary>
        /// The actual string value of the encoding that will be sent over the API.
        /// </summary>
        [JsonProperty("Encoding")]
        public string RawEncoding
        {
            get
            {
                switch (Encoding)
                {
                    case MessageEncoding.Gsm7:
                        return "gsm7";
                    case MessageEncoding.Ucs2:
                        return "ucs2";
                    case MessageEncoding.Auto:
                        return "auto";
                    default:
                        throw new Exception($"Invalid MessageEncoding enum value: {Encoding}");
                }
            }
        }

        /// <summary>
        /// The validity period in seconds. Minimum is 60 seconds and maximum is 172800 (48 hours).
        /// If not specified, the messages is attempted to be delivered in 48 hours.
        /// </summary>
        /// <example>90</example>
        public int? ValidityPeriodInSeconds { get; }
        
        /// <summary>
        /// An optional callback url. If specified, this url is called with a status report when the message has reached its final status (either delivered, failed or cancelled).
        /// NOTE: Callbacks happen in bulks.Reports for message with identical callback urls can happen to be grouped in single callbacks.
        /// </summary>
        public string? StatusCallbackUrl { get;  }

        /// <summary>
        /// If specified, this represents the future send time of the message.
        /// </summary>
        public string? SendTime { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="to">The msisdn (country code and number) to send to. (Remember to include countrycode in all numbers, e.g. 4512345678). If max length is exceeded, the entire api call will fail.</param>
        /// <param name="text">The text message. If the max length is exceeded (10,000 chars), the message text is truncated and sent.</param>
        /// <param name="from">The sender. This can either be a 3-11 chars text sender or an up to 14 digit long sender number. If the max length is exceeded, the string is truncated.</param>
        /// <param name="messageId">/// An optional message id used to identify the message. If no message id is provided, a new message id is generated and assigned to the message. This id must be unique across all messages created on the same account.         (In case a previous message has been deleted according to GDPR deletion rules setup on the specific account, the messageId is allowed to be reused). If max length is exceeded, the entire api call will fail. Max length: 50</param>
        /// <param name="respectBlacklist">If true, this message will be blocked from sending if the target number is on the account's blacklist. If false, the message will be sent no matter blacklist settings.</param>
        /// <param name="flash">If true, the message will be shown as a flash message (also known as a class0 message) on the target phone. If false, it will be received as a standard text message.</param>
        /// <param name="encoding">The encoding of the message.</param>
        /// <param name="validityPeriod">The validity period. If the message cannot be delivered within this time frame it is dropped an concidered failed. Minimum is 60 seconds and maximum is 172800 (48 hours). If not specified, the messages is attempted to be delivered in 48 hours.</param>
        /// <param name="statusCallbackUrl">An optional callback url. If specified, this url is called with a status report when the message has reached its final status (either delivered, failed or cancelled).</param>
        /// <param name="sendTime">If specified, this message will be sent at the specified time in the future.</param>
        public OutgoingSmsMessageCreateInfo(string to, string text, string from, OutgoingMessageId? messageId = null, bool respectBlacklist = true, bool flash = false, MessageEncoding encoding = MessageEncoding.Gsm7, TimeSpan? validityPeriod = null, string? statusCallbackUrl = null, DateTime? sendTime = null)
        {
            if (string.IsNullOrEmpty(to))
            {
                throw new ArgumentException($"'{nameof(to)}' cannot be null or empty.", nameof(to));
            }

            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException($"'{nameof(text)}' cannot be null or empty.", nameof(text));
            }

            if (string.IsNullOrEmpty(from))
            {
                throw new ArgumentException($"'{nameof(from)}' cannot be null or empty.", nameof(from));
            }

            To = to;
            Text = text;
            From = from;
            MessageId = messageId;
            RespectBlacklist = respectBlacklist;
            Flash = flash;
            Encoding = encoding;

            if(validityPeriod != null)
            {
                ValidityPeriodInSeconds = (int)validityPeriod.Value.TotalSeconds;
            }
            
            StatusCallbackUrl = statusCallbackUrl;
            if(sendTime != null)
            {
                SendTime = sendTime.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH':'mm':'ssZ");
            }
            
        }
    }
}
