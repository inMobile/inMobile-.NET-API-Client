using System.Collections.Generic;
using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// A list of email events.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EmailEventsList<T>
    {
        /// <summary>
        /// The events in the list
        /// </summary>
        [JsonProperty]
        public List<T> Events { get; private set; } = new List<T>();
    }
}
