using System;
using System.Collections.Generic;
using System.Text;

namespace InMobile.Sms.ApiClient
{
    public class Recipient
    {
        public string? Id { get; set; }
        public string? ListId { get; set; }
        public NumberInfo? NumberInfo { get; set; }
        public Dictionary<string, string>? Fields { get; set; } = new Dictionary<string, string>();

        // This constructor must exist to ensure deserialization is possible - also in the future if more fields should be added to the recipient json from the api
        public Recipient()
        {
        }

        public Recipient(NumberInfo numberInfo)
        {
            NumberInfo = numberInfo ?? throw new ArgumentNullException(nameof(numberInfo));
        }
    }
}
