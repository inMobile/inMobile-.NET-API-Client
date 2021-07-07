using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    public class OutgoingSmsMessageCreateResponse
    {
        /// <summary>
        /// The parsed details about the msisdn provided in the to-field of the request (msisdn is a number with a countrycode e.g. 4512345678)
        /// </summary>
        public NumberDetails? NumberDetails { get; set; }
        /// <summary>
        /// The text message
        /// </summary>
        /// <example>"This is a message text to be sent"</example>
        public string? Text { get; set; }

        /// <summary>
        /// The sender.
        /// </summary>
        /// <example>PetShop</example>
        public string? From { get; set; }

        /// <summary>
        /// The number of sms messages this message will be split into when sent to the operator. Charging will also be done according to this number.
        /// </summary>
        /// <example>1</example>
        public int SmsCount { get; set; }

        /// <summary>
        /// An optional message id used to identify the message.
        /// If no message id is provided, a new message id is generated and assigned to the message. This id must be unique across all messages created on the same account.
        /// (In case a previous message has been deleted according to GDPR deletion rules setup on the specific account, the messageId is allowed to be reused)
        /// </summary>
        /// <example>PetShop</example>
        public string? MessageId { get; set; }

        /// <summary>
        /// The encoding of the message. Can be either "gsm7" or "ucs2". In case the message was submitted with encoding "auto", this report will reveal the final encoding based on the characters in the message text.
        /// "gsm7" is the default alfabet for text messages and when using gsm7, a single sms message can contain 160 characters. If the length exceeeds 160 characters, the message is actually split up into parts of 153 characters and charged according to this. Please note, that a few, specific characters fill up 2 bytes and count for 2 letters. Ref: https://en.wikipedia.org/wiki/GSM_03.38
        /// "ucs2"" allows for more non-roman characters to be used along with smileys. When using this encoding, a single message can consist of 70 characters. If the message exceeds 70 characters, the final message is actually split into parts of 67 characters.
        /// </summary>
        /// <example>gsm7</example>
        public MessageEncoding Encoding { get; set; }
    }
}
