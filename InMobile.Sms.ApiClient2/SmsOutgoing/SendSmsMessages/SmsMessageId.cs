namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// The id type used for outgoing messages.
    /// </summary>
    public class OutgoingMessageId : CustomStringTypeBase<OutgoingMessageId>
    {
        /// <summary>
        /// Create a new outgoing message id.
        /// </summary>
        /// <param name="value"></param>
        public OutgoingMessageId(string value) : base(value)
        {
        }
    }
}
