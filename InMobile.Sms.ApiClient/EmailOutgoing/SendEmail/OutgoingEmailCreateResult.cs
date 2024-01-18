using System.Collections.Generic;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Result info for an created email.
    /// </summary>
    public class OutgoingEmailCreateResult
    {
        /// <summary>
        /// An optional message id used to identify the message. If no message id is provided, a new message id is generated and assigned to the message.
        /// </summary>
        public OutgoingEmailId MessageId { get; }

        /// <summary>
        /// The list of created email recipients.
        /// </summary>
        public List<EmailRecipient> To { get; }
    }
}
