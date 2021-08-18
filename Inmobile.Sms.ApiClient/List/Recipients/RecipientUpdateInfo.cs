using System;
using System.Collections.Generic;
using System.Text;

namespace InMobile.Sms.ApiClient
{
    public class RecipientUpdateInfo : IRecipientUpdateInfo
    {
        public RecipientId Id { get; }
        public RecipientListId ListId { get; }
        /// <summary>
        /// If not specified, the number is just left intact.
        /// </summary>
        public NumberInfo? NumberInfo { get; }

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
            NumberInfo = numberInfo ?? throw new ArgumentNullException(nameof(numberInfo));
            Fields = fields ?? throw new ArgumentNullException(nameof(fields));
        }

        public RecipientUpdateInfo(RecipientId recipientId, RecipientListId listId, NumberInfo numberInfo) : this(internalConstructor: true, recipientId: recipientId, listId: listId, numberInfo: numberInfo, fields: new Dictionary<string, string>())
        {
            if (numberInfo is null)
            {
                throw new ArgumentNullException(nameof(numberInfo));
            }
        }

        public RecipientUpdateInfo(RecipientId recipientId, RecipientListId listId, Dictionary<string, string> fields) : this(internalConstructor: true, recipientId: recipientId, listId: listId, numberInfo: null, fields: fields)
        {
            
        }

        public RecipientUpdateInfo(RecipientId recipientId, RecipientListId listId, NumberInfo numberInfo, Dictionary<string, string> fields) : this(internalConstructor: true, recipientId: recipientId, listId: listId, numberInfo: numberInfo, fields)
        {
        }
    }
}
