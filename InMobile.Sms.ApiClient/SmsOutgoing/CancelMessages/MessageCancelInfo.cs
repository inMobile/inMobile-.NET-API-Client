namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Information needed to cancel an outgoing text message.
    /// </summary>
    public class MessageCancelInfo
    {
        /// <summary>
        ///
        /// </summary>
        public OutgoingMessageId MessageId { get; set; }

        /// <summary>
        /// Create a new cancel-object
        /// </summary>
        /// <param name="messageId">The id of the message to cancel</param>
        public MessageCancelInfo(OutgoingMessageId messageId)
        {
            MessageId = messageId;
        }
    }
}
