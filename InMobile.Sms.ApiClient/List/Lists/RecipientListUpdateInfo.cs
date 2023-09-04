using System;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Information required to update a recipient.
    /// </summary>
    public class RecipientListUpdateInfo : IRecipientListUpdateInfo
    {
        /// <summary>
        /// The id of the recipient.
        /// </summary>
        public RecipientListId Id { get; }
        
        /// <summary>
        /// The name of the list.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Create a new update-object.
        /// </summary>
        /// <param name="listId">The id of the desired list to update.</param>
        /// <param name="name">The desired name.</param>
        public RecipientListUpdateInfo(RecipientListId listId, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            }

            Id = listId ?? throw new ArgumentNullException(nameof(listId));
            Name = name;
        }
    }
}
