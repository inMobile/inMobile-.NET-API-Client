using System;
using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// An incoming message
    /// </summary>
    public sealed class IncomingMessage
    {
        /// <summary>
        /// Who the message is from
        /// </summary>
        [JsonProperty]
        public IncomingMessageSender From { get; private set; }
        
        /// <summary>
        /// Who the message is to
        /// </summary>
        [JsonProperty]
        public IncomingMessageTarget To { get; private set; }
        
        /// <summary>
        /// The content of the message
        /// </summary>
        [JsonProperty]
        public string Text { get; private set; }
        
        /// <summary>
        /// When the message was received
        /// </summary>
        [JsonProperty]
        public DateTime ReceivedAt { get; private set; }

        [JsonConstructor]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private IncomingMessage()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }
    }
}