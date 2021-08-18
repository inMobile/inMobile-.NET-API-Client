﻿using System;

namespace InMobile.Sms.ApiClient
{
    public class RecipientListUpdateInfo : IRecipientListUpdateInfo
    {
        public RecipientListId Id { get; set; }
        public string Name { get; set; }

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
