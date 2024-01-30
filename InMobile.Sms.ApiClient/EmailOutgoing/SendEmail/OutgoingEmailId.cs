namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// The id type used for outgoing emails.
    /// </summary>
    public class OutgoingEmailId : CustomStringTypeBase<OutgoingEmailId>
    {
        /// <summary>
        /// Create a new outgoing email id.
        /// </summary>
        /// <param name="value"></param>
        public OutgoingEmailId(string value) : base(value)
        {
        }
    }
}
