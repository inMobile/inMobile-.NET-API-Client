using System;
using System.Collections.Generic;
using System.Text;

namespace InMobile.Sms.ApiClient
{
    public class BlacklistEntry
    {
        public NumberInfo? NumberInfo { get; set; }
        public string Comment { get; set; }
        public string Id { get; set; }
    }
}
