using System;
using System.Collections.Generic;
using System.Text;

namespace InMobile.Sms.ApiClient
{

    public class Recipient : IRecipientUpdateInfo
    {
        public string RecipientId { get; set; }
        public string ListId { get; set; }
        public NumberInfo NumberInfo { get; set; }
        public Dictionary<string, string> Fields { get; set; } = new Dictionary<string, string>();

        // This constructor must exist to ensure deserialization is possible - also in the future if more fields should be added to the recipient json from the api
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Recipient()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }

    }
}
