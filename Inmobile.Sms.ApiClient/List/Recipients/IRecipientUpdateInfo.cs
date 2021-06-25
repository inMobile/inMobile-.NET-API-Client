using System.Collections.Generic;

namespace InMobile.Sms.ApiClient
{
    public interface IRecipientUpdateInfo
    {
        string RecipientId { get; }
        string ListId { get; }
        /// <summary>
        /// Optional. If not specified, the number is just ignored on the API side.
        /// </summary>
        NumberInfo? NumberInfo { get; }
        Dictionary<string, string> Fields { get; }
    }
}
