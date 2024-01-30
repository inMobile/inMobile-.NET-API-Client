using System;
using System.Collections.Generic;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Information for creating an new email with template.
    /// </summary>
    public class OutgoingEmailTemplateCreateInfo
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
        /// The id of the template to use.
        /// </summary>
        public EmailTemplateId TemplateId { get; }

        /// <summary>
        /// A key-value list of placeholders to replace in the template text. Keys must be encapsulated with {}. E.g. {NAME}.
        /// </summary>
        public Dictionary<string, string>? Placeholders { get; }

        /// <summary>
        /// </summary>
        /// <param name="templateId">The id of the template to use.</param>
        /// <param name="from">The from info of the email.</param>
        /// <param name="to">A list of recipients. Allowed to contain between 1 and 100 elements.</param>
        /// <param name="replyTo">A list of optional Reply To objects.</param>
        /// <param name="sendTime">If specified, this represents the future send time of the email.</param>
        /// <param name="tracking">If true, this will add Open and Click tracking to your email. Default: true.</param>
        /// <param name="messageId">An optional message id used to identify the message. If no message id is provided, a new message id is generated and assigned to the message.</param>
        /// <param name="placeholders">A key-value list of placeholders to replace in the template text. Keys must be encapsulated with {}. E.g. {NAME}.</param>
        /// <exception cref="ArgumentException"></exception>
        public OutgoingEmailTemplateCreateInfo(EmailTemplateId templateId, EmailSender from, List<EmailRecipient> to, List<EmailRecipient>? replyTo = null, DateTime? sendTime = null, bool tracking = true, OutgoingEmailId? messageId = null, Dictionary<string, string>? placeholders = null)
        {
            if (templateId is null)
            {
                throw new ArgumentException($"'{nameof(templateId)}' cannot be null or empty.", nameof(templateId));
            }
            if (from is null)
            {
                throw new ArgumentException($"'{nameof(from)}' cannot be null or empty.", nameof(from));
            }
            if (to is null || to.Count == 0)
            {
                throw new ArgumentException($"'{nameof(to)}' cannot be null or empty.", nameof(to));
            }

            TemplateId = templateId;
            From = from;
            To = to;
            ReplyTo = replyTo;

            if (sendTime != null)
            {
                SendTime = sendTime.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH':'mm':'ssZ");
            }

            Tracking = tracking;
            MessageId = messageId;
            Placeholders = placeholders;
        }
    }
}
