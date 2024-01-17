namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Outgoing message encoding.
    /// </summary>
    public enum MessageEncoding
    {
        /// <summary>
        /// Unknown encoding
        /// </summary>
        Unknown,
        /// <summary>
        /// "gsm7" is the default alfabet for text messages and when using gsm7, a single sms message can contain 160 characters. If the length exceeeds 160 characters, the message is actually split up into parts of 153 characters and charged according to this. Please note, that a few, specific characters fill up 2 bytes and count for 2 letters. Ref: https://en.wikipedia.org/wiki/GSM_03.38
        /// </summary>
        Gsm7,
        /// <summary>
        /// "ucs2"" allows for more non-roman characters to be used along with smileys. When using this encoding, a single message can consist of 70 characters. If the message exceeds 70 characters, the final message is actually split into parts of 67 characters.
        /// </summary>
        Ucs2,
        /// <summary>
        /// "auto" can be used in case the sender wishes to support non-roman characters but wants to save the expenses on all the trafic that only contains gsm characters anyway.
        /// </summary>
        Auto
    }
}
