using System.Collections.Generic;
using Newtonsoft.Json;

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
        [JsonProperty]
        public OutgoingEmailId MessageId { get; private set; }

        /// <summary>
        /// The list of created email recipients.
        /// </summary>
        [JsonProperty]
        public List<EmailRecipient> To { get; private set; }

        [JsonConstructor]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private OutgoingEmailCreateResult()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }
    }
}
