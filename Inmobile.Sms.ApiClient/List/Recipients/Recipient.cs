using System;
using System.Collections.Generic;
using System.Text;

namespace InMobile.Sms.ApiClient
{
    public class Recipient
    {
        public string Id { get; set; }
        public string ListId { get; set; }        
        public NumberInfo? NumberInfo { get; set; }
        public Dictionary<string,string>? Fields { get; set; }
    }
}
