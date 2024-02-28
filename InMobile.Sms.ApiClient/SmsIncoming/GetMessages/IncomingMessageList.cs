using System.Collections.Generic;
using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient.SmsIncoming.GetMessages
{
    /// <summary>
    /// A list of incoming messages
    /// </summary>
    public class IncomingMessageList
    {
        /// <summary>
        /// The incoming messages
        /// </summary>
        [JsonProperty]
        public List<IncomingMessage>? Messages { get; private set; }
    }
}