namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Information related to charging of an outgoing message.
    /// </summary>
    public class ChargeInfo
    {
        /// <summary>
        /// True if the message will be charged.
        /// </summary>
        /// <example>true</example>
        public bool IsCharged { get; set; }
        /// <summary>
        /// The total sms count in the message. If IsCharged is true, this is the number of sms'es that will be charged.
        /// </summary>
        /// <example>2</example>
        public int SmsCount { get; set; }

        /// <summary>
        /// The network of which the message belongs.
        /// </summary>
        public string? Network { get; set; }

        /// <summary>
        /// The encoding of the message. Can be either "gsm7" or "ucs2". In case the message was submitted with encoding "auto", this report will reveal the final encoding based on the characters in the message text.
        /// "gsm7" is the default alfabet for text messages and when using gsm7, a single sms message can contain 160 characters. If the length exceeeds 160 characters, the message is actually split up into parts of 153 characters and charged according to this. Please note, that a few, specific characters fill up 2 bytes and count for 2 letters. Ref: https://en.wikipedia.org/wiki/GSM_03.38
        /// "ucs2"" allows for more non-roman characters to be used along with smileys. When using this encoding, a single message can consist of 70 characters. If the message exceeds 70 characters, the final message is actually split into parts of 67 characters.
        /// </summary>
        /// <example>gsm7</example>
        public MessageEncoding Encoding { get; set; }
    }
}
