using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Info about a list
    /// </summary>
    public class RecipientList : IRecipientListUpdateInfo
    {
        /// <summary>
        /// The id of the list
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public RecipientListId Id { get; private set; }

        /// <summary>
        /// The name of the list
        /// </summary>
        [JsonProperty]
        public string Name { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [JsonConstructor]
        private RecipientList()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }
    }
}
