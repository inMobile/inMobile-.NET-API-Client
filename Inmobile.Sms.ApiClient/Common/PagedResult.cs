using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    public class PagedResult<T>
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

    public class LinkSection
    {
        public string? Next { get; set; }
        public bool IsLastPage { get; set; }
    }
}
