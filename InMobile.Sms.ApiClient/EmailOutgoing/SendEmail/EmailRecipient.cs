namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Information about the email recipient.
    /// </summary>
    public class EmailRecipient
    {
        /// <summary>
        /// The email address of the recipient.
        /// </summary>
        public string EmailAddress { get; }

        /// <summary>
        /// The display name to use for the recipient.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// </summary>
        /// <param name="emailAddress">The email address of the recipient</param>
        /// <param name="displayName">The display name to use for the recipient</param>
        public EmailRecipient(string emailAddress, string displayName)
        {
            EmailAddress = emailAddress;
            DisplayName = displayName;
        }
    }
}
