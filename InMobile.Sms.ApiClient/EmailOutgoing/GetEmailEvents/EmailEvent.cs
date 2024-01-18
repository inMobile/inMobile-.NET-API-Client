using System;

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
        public OutgoingEmailId MessageId { get; }

        /// <summary>
        /// Contains a code for what event type the event is.
        /// </summary>
        public EmailEventType EventType { get; }

        /// <summary>
        /// A human readable description of the state.
        /// </summary>
        public string EventTypeDescription { get; }

        /// <summary>
        /// When the event happened.
        /// </summary>
        public DateTime EventTimestamp { get; }
    }
}
