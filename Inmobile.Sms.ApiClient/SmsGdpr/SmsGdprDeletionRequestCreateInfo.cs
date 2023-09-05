namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Information for creating a new GDPR deletion request.
    /// </summary>
    public class SmsGdprDeletionRequestCreateInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public NumberInfo NumberInfo { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberInfo"></param>
        public SmsGdprDeletionRequestCreateInfo(NumberInfo numberInfo)
        {
            NumberInfo = numberInfo;
        }
    }
}
