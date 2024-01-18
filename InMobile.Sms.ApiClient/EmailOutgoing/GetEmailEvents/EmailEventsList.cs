using System.Collections.Generic;

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
        public List<T>? Events { get; }
    }
}
