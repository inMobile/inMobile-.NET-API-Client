using System;

namespace InMobile.Sms.ApiClient
{
    public class RecipientListUpdateInfo : IRecipientListUpdateInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public RecipientListUpdateInfo(string listId, string name)
        {
            if (string.IsNullOrWhiteSpace(listId))
            {
                throw new ArgumentException($"'{nameof(listId)}' cannot be null or whitespace.", nameof(listId));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            }

            Id = listId;
            Name = name;
        }
    }
}
