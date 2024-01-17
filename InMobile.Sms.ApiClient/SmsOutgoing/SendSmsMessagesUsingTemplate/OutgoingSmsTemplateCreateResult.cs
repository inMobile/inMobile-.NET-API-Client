using System.Collections.Generic;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Result info for a created outgoing message using template.
    /// </summary>
    public class OutgoingSmsTemplateCreateResult : ResultsList<OutgoingSmsMessageCreateResult>
    {
        /// <summary>
        /// A list of used used placeholder keys.
        /// </summary>
        public List<string> UsedPlaceholderKeys { get; private set; } = new List<string>();

        /// <summary>
        /// A list of not used placeholder keys.
        /// </summary>
        public List<string> NotUsedPlaceholderKeys { get; private set; } = new List<string>();
    }
}
