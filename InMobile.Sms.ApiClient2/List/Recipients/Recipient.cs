using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// A recipient
    /// </summary>
    public class Recipient : IRecipientUpdateInfo
    {
        /// <summary>
        /// The creation date of the recipient (in UTC time).
        /// </summary>
        [JsonProperty]
        public DateTime Created { get; private set; }

        /// <summary>
        /// The external created date (in UTC time). If specified, this value represents the date of which the recipient was created in a given source outside of inMobiles system.
        /// </summary>
        [JsonProperty]
        public DateTime? ExternalCreated { get; private set; }

        /// <summary>
        /// The id of the recipient.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public RecipientId Id { get; private set; }

        /// <summary>
        /// The id of the list of which the recipient belongs.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public RecipientListId ListId { get; private set; }

        /// <summary>
        /// The number information.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public NumberInfo NumberInfo { get; private set; }

        /// <summary>
        /// Additional fields.
        /// </summary>
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
