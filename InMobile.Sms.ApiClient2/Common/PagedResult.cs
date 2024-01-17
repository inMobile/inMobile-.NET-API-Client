using System.Collections.Generic;
using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    internal class PagedResult<T>
    {
        [JsonProperty(Required = Required.Always)]
        public List<T> Entries { get; set; }
        public LinkSection _links { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [JsonConstructor]
        private PagedResult()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {

        }
    }

    internal class LinkSection
    {
        public string? Next { get; set; }
        [JsonProperty(Required = Required.Always)]
        public bool IsLastPage { get; set; }
    }
}
