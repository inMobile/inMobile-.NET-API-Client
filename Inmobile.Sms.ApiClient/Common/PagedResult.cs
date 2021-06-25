using System;
using System.Collections.Generic;
using System.Text;

namespace InMobile.Sms.ApiClient
{
    public class PagedResult<T>
    {
        public List<T> Entries { get; set; }
        public LinkSection _links { get; set; }
    }

    public class LinkSection
    {
        public string Next { get; set; }
        public bool IsLastPage { get; set; }
    }
}
