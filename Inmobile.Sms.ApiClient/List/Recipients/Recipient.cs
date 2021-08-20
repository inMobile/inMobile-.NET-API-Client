using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    public class Recipient : IRecipientUpdateInfo
    {
        public DateTime? ExternalCreatedDate { get; private set; }
        [JsonProperty(Required = Required.Always)]
        public RecipientId Id { get; private set; }
        [JsonProperty(Required = Required.Always)]
        public RecipientListId ListId { get; private set; }
        [JsonProperty(Required = Required.Always)]
        public NumberInfo NumberInfo { get; private set; }
        [JsonProperty]
        public Dictionary<string, string> Fields { get; private set; } = new Dictionary<string, string>();

        // This constructor must exist to ensure deserialization is possible - also in the future if more fields should be added to the recipient json from the api
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [JsonConstructor]
        private Recipient()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }
    }
}
