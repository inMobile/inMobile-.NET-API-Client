namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// The id type used for templates.
    /// </summary>
    public class TemplateId : CustomStringTypeBase<TemplateId>
    {
        /// <summary>
        /// Create a new template id.
        /// </summary>
        /// <param name="value"></param>
        public TemplateId(string value) : base(value)
        {
        }
    }
}
