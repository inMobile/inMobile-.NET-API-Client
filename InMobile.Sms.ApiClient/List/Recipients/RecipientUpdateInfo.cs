using System;
using System.Collections.Generic;
using System.Text;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Value holder for updates performed without retrieving a recipient object from the API first.
    /// </summary>
    public class RecipientUpdateInfo : IRecipientUpdateInfo
    {
        /// <summary>
        /// The id of the recipient.
        /// </summary>
        public RecipientId Id { get; }
        /// <summary>
        /// The id of the list
        /// </summary>
        public RecipientListId ListId { get; }
        /// <summary>
        /// If not specified, the number is just left intact.
        /// </summary>
        public NumberInfo? NumberInfo { get; }
        /// <summary>
        /// Additional fields to update. Fields not included here are not overwridden, just ignored.
        /// </summary>
        public Dictionary<string, string> Fields { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="internalConstructor">Used to allow having a constructor overload with same parameter count but with numberInfo not specified.</param>
        /// <param name="recipientId"></param>
        /// <param name="listId"></param>
        /// <param name="numberInfo"></param>
        /// <param name="fields"></param>
        private RecipientUpdateInfo(bool internalConstructor, RecipientId recipientId, RecipientListId listId, NumberInfo? numberInfo, Dictionary<string, string> fields)
        {
            Id = recipientId;
            ListId = listId;
            NumberInfo = numberInfo;
            Fields = fields ?? throw new ArgumentNullException(nameof(fields));
        }

        /// <summary>
        /// Used for updating a recipients numberInfo only.
        /// </summary>
        /// <param name="recipientId">The id of the target recipient.</param>
        /// <param name="listId">The id of the list in which the recipient belongs.</param>
        /// <param name="numberInfo">The number information</param>
        public RecipientUpdateInfo(RecipientId recipientId, RecipientListId listId, NumberInfo numberInfo) : this(internalConstructor: true, recipientId: recipientId, listId: listId, numberInfo: numberInfo, fields: new Dictionary<string, string>())
        {
            if (numberInfo is null)
            {
                throw new ArgumentNullException(nameof(numberInfo));
            }
        }

        /// <summary>
        /// Used for updating a recipients fields only. Fields names not included in the fields-object are left untouched.
        /// </summary>
        /// <param name="recipientId">The id of the target recipient.</param>
        /// <param name="listId">The id of the list in which the recipient belongs.</param>
        /// <param name="fields">Additional field information (key/value pairs)</param>
        public RecipientUpdateInfo(RecipientId recipientId, RecipientListId listId, Dictionary<string, string> fields) : this(internalConstructor: true, recipientId: recipientId, listId: listId, numberInfo: null, fields: fields)
        {
            
        }

        /// <summary>
        /// Used for updating both numberInfo and field information of a recipient.
        /// </summary>
        /// <param name="recipientId">The id of the target recipient.</param>
        /// <param name="listId">The id of the list in which the recipient belongs.</param>
        /// <param name="numberInfo">The number information</param>
        /// <param name="fields">Additional field information (key/value pairs)</param>
        public RecipientUpdateInfo(RecipientId recipientId, RecipientListId listId, NumberInfo numberInfo, Dictionary<string, string> fields) : this(internalConstructor: true, recipientId: recipientId, listId: listId, numberInfo: numberInfo, fields)
        {
        }
    }
}
