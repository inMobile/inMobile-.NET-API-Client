using System;
using System.Collections.Generic;

namespace InMobile.Sms.ApiClient
{
    public class RecipientCreateInfo
    {
        public string ListId { get; }
        public NumberInfo NumberInfo { get; }
        public Dictionary<string, string> Fields { get; }

        public RecipientCreateInfo(string listId, NumberInfo numberInfo, Dictionary<string, string> fields)
        {
            if (string.IsNullOrWhiteSpace(listId))
            {
                throw new ArgumentException($"'{nameof(listId)}' cannot be null or whitespace.", nameof(listId));
            }

            ListId = listId;
            NumberInfo = numberInfo ?? throw new ArgumentNullException(nameof(numberInfo));
            Fields = fields ?? throw new ArgumentNullException(nameof(fields));
        }

        public RecipientCreateInfo(string listId, NumberInfo numberInfo) : this(listId: listId, numberInfo: numberInfo, fields: new Dictionary<string, string>())
        {
        }
    }
}
