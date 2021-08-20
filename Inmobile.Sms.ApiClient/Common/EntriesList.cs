using System.Collections.Generic;
using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// A list of entries.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntriesList<T>
    {
        /// <summary>
        /// The entries in the list.
        /// </summary>
        [JsonProperty]
        public List<T>? Entries { get; private set; }
    }
}
