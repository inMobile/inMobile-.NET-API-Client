namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Parse Phone Number Info
    /// </summary>
    public class ParsePhoneNumberInfo
    {
        /// <summary>
        /// The country part of the msisdn, e.g. 45 or DK.
        /// </summary>
        public string CountryHint { get; }

        /// <summary>
        /// The input msisdn in its unaltered format. This is the value provided when sending the text message.
        /// </summary>
        public string RawMsisdn { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="countryHint"></param>
        /// <param name="rawMsisdn"></param>
        public ParsePhoneNumberInfo(string countryHint, string rawMsisdn)
        {
            CountryHint = countryHint;
            RawMsisdn = rawMsisdn;
        }
    }
}
