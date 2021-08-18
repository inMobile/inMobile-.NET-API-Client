using System;
using System.Collections.Generic;

namespace InMobile.Sms.ApiClient
{
    public class RecipientCreateInfo
    {
        public RecipientListId ListId { get; }
        public NumberInfo NumberInfo { get; }
        public Dictionary<string, string> Fields { get; }

        public RecipientCreateInfo(RecipientListId listId, NumberInfo numberInfo, Dictionary<string, string> fields)
        {
            ListId = listId;
            NumberInfo = numberInfo ?? throw new ArgumentNullException(nameof(numberInfo));
            Fields = fields ?? throw new ArgumentNullException(nameof(fields));
        }

        public RecipientCreateInfo(RecipientListId listId, NumberInfo numberInfo) : this(listId: listId, numberInfo: numberInfo, fields: new Dictionary<string, string>())
        {
        }
    }
}
