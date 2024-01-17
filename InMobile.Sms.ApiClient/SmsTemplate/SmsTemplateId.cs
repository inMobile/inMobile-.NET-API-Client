namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// The id type used for SMS templates.
    /// </summary>
    public class SmsTemplateId : CustomStringTypeBase<SmsTemplateId>
    {
        /// <summary>
        /// Create a new SMS template id.
        /// </summary>
        /// <param name="value"></param>
        public SmsTemplateId(string value) : base(value)
        {
        }
    }
}
