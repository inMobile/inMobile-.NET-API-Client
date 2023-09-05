namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// The id type used for GDPR deletion request.
    /// </summary>
    public class SmsGdprDeletionRequestId : CustomStringTypeBase<SmsGdprDeletionRequestId>
    {
        /// <summary>
        /// Create a new GDPR deletion request id.
        /// </summary>
        /// <param name="value"></param>
        public SmsGdprDeletionRequestId(string value) : base(value)
        {
        }
    }
}
