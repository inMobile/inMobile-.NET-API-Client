namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Information about the email sender.
    /// </summary>
    public class EmailSender
    {
        /// <summary>
        /// The email address of the sender.
        /// </summary>
        public string EmailAddress { get; }

        /// <summary>
        /// The display name to use for the sender.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// </summary>
        /// <param name="emailAddress">The email address of the sender</param>
        /// <param name="displayName">The display name to use for the sender</param>
        public EmailSender(string emailAddress, string displayName)
        {
            EmailAddress = emailAddress;
            DisplayName = displayName;
        }
    }
}
