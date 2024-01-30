using System;
using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// An email event for an outgoing email.
    /// </summary>
    public class EmailEvent
    {
        /// <summary>
        /// The id of the message.
        /// </summary>
        [JsonProperty]
        public OutgoingEmailId MessageId { get; private set; }

        /// <summary>
        /// Contains a code for what event type the event is.
        /// </summary>
        [JsonProperty]
        public EmailEventType EventType { get; private set; }

        /// <summary>
        /// A human readable description of the state.
        /// </summary>
        [JsonProperty]
        public string EventTypeDescription { get; private set; }

        /// <summary>
        /// When the event happened.
        /// </summary>
        [JsonProperty]
        public DateTime EventTimestamp { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [JsonConstructor]
        private EmailEvent()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }
    }
}
