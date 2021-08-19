using System;
using System.Collections.Generic;
using System.Text;

namespace InMobile.Sms.ApiClient
{
    public class BlacklistEntryCreateInfo
    {
        public NumberInfo NumberInfo { get; private set; }
        public string? Comment { get; private set; }

        public BlacklistEntryCreateInfo(NumberInfo numberInfo, string? comment = null)
        {
            NumberInfo = numberInfo ?? throw new ArgumentNullException(nameof(numberInfo));
            Comment = comment;
        }
    }
}
