using System.Collections.Generic;
using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// A list of results.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultsList<T>
    {
        /// <summary>
        /// The results in the list
        /// </summary>
        [JsonProperty]
        public List<T>? Results { get; private set; }
    }
}
