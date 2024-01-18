using System.Collections.Generic;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Result info for an created email using template.
    /// </summary>
    public class OutgoingEmailTemplateCreateResult : OutgoingEmailCreateResult
    {
        /// <summary>
        /// A list of used used placeholder keys.
        /// </summary>
        public List<string> UsedPlaceholderKeys { get; } = new List<string>();

        /// <summary>
        /// A list of not used placeholder keys.
        /// </summary>
        public List<string> NotUsedPlaceholderKeys { get; } = new List<string>();
    }
}
