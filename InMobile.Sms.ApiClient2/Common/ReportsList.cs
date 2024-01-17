using System.Collections.Generic;
using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// A list of reports.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReportsList<T>
    {
        /// <summary>
        /// The reports in the list
        /// </summary>
        [JsonProperty]
        public List<T>? Reports { get; private set; }
    }
}
