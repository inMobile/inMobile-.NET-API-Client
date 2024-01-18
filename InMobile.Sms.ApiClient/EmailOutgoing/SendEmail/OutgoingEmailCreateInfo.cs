using System;
using System.Collections.Generic;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Information for creating an new email.
    /// </summary>
    public class OutgoingEmailCreateInfo
    {
        /// <summary>
        /// The from info of the email.
        /// </summary>
        public EmailSender From { get; }

        /// <summary>
        /// A list of recipients. Allowed to contain between 1 and 100 elements.
        /// </summary>
        public List<EmailRecipient> To { get; }

        /// <summary>
        /// A list of optional Reply To objects.
        /// </summary>
        public List<EmailRecipient>? ReplyTo { get; }

        /// <summary>
        /// An optional message id used to identify the message. If no message id is provided, a new message id is generated and assigned to the message. This id must be unique across all messages created on the same account.
        /// </summary>
        public OutgoingEmailId? MessageId { get; }

        /// <summary>
        /// If specified, this represents the future send time of the email.
        /// </summary>
        public string? SendTime { get; }

        /// <summary>
        /// If true, this will add Open and Click tracking to your email. Default: true.
        /// </summary>
        public bool Tracking { get; }

        /// <summary>
        /// The subject of the email. If max length is exceeded, the entire call will fail. Max length is 255 chars.
        /// </summary>
        public string Subject { get; }

        /// <summary>
        /// The HTML body of the email. Max size of 2 MB. If max size is exceeded, the entire call will fail.
        /// </summary>
        public string Html { get; }

        /// <summary>
        /// The text body of the email. If none is provided this will be generated from the HTML. Max size of 2 MB. If max size is exceeded, the entire call will fail.
        /// </summary>
        public string? Text { get; }

        /// <summary>
        /// </summary>
        /// <param name="subject">The subject of the email. If max length is exceeded, the entire call will fail. Max length is 255 chars.</param>
        /// <param name="html">The HTML body of the email. Max size of 2 MB. If max size is exceeded, the entire call will fail.</param>
        /// <param name="from">The from info of the email.</param>
        /// <param name="to">A list of recipients. Allowed to contain between 1 and 100 elements.</param>
        /// <param name="replyTo">A list of optional Reply To objects.</param>
        /// <param name="text">The text body of the email. If none is provided this will be generated from the HTML. Max size of 2 MB. If max size is exceeded, the entire call will fail.</param>
        /// <param name="sendTime">If specified, this represents the future send time of the email.</param>
        /// <param name="tracking">If true, this will add Open and Click tracking to your email. Default: true.</param>
        /// <param name="messageId">An optional message id used to identify the message. If no message id is provided, a new message id is generated and assigned to the message.</param>
        /// <exception cref="ArgumentException"></exception>
        public OutgoingEmailCreateInfo(string subject, string html, EmailSender from, List<EmailRecipient> to, List<EmailRecipient>? replyTo = null, string? text = null, DateTime? sendTime = null, bool tracking = true, OutgoingEmailId? messageId = null)
        {
            if (string.IsNullOrEmpty(subject))
            {
                throw new ArgumentException($"'{nameof(subject)}' cannot be null or empty.", nameof(subject));
            }
            if (string.IsNullOrEmpty(html))
            {
                throw new ArgumentException($"'{nameof(html)}' cannot be null or empty.", nameof(html));
            }
            if (from is null)
            {
                throw new ArgumentException($"'{nameof(from)}' cannot be null or empty.", nameof(from));
            }
            if (to is null || to.Count == 0)
            {
                throw new ArgumentException($"'{nameof(to)}' cannot be null or empty.", nameof(to));
            }

            Subject = subject;
            Html = html;
            Text = text;
            From = from;
            To = to;
            ReplyTo = replyTo;

            if (sendTime != null)
            {
                SendTime = sendTime.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH':'mm':'ssZ");
            }

            Tracking = tracking;
            MessageId = messageId;
        }
    }
}
