namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Information about the email replyto recipient.
    /// </summary>
    public class EmailReplyToRecipient
    {
        /// <summary>
        /// The email address of the recipient.
        /// </summary>
        public string EmailAddress { get; }

        /// <summary>
        /// </summary>
        /// <param name="emailAddress">The email address of the recipient</param>
        public EmailReplyToRecipient(string emailAddress)
        {
            EmailAddress = emailAddress;
        }
    }
}