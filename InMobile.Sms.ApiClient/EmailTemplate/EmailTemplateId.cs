namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// The id type used for email templates.
    /// </summary>
    public class EmailTemplateId : CustomStringTypeBase<EmailTemplateId>
    {
        /// <summary>
        /// Create a new email template id.
        /// </summary>
        /// <param name="value"></param>
        public EmailTemplateId(string value) : base(value)
        {
        }
    }
}
