using System.Collections.Generic;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Information needed for updating a recipient.
    /// </summary>
    public interface IRecipientUpdateInfo
    {
        /// <summary>
        /// The id of the recipient.
        /// </summary>
        RecipientId Id { get; }

        /// <summary>
        /// The id of the list where the recipient belongs.
        /// </summary>
        RecipientListId ListId { get; }

        /// <summary>
        /// Optional. If not specified, the number is just ignored on the API side.
        /// </summary>
        NumberInfo? NumberInfo { get; }

        /// <summary>
        /// Additional fields on the recipient.
        /// </summary>
        Dictionary<string, string> Fields { get; }
    }
}
