using System;
using System.Collections.Generic;
using System.Text;

namespace InMobile.Sms.ApiClient
{
    public enum MessageStateCode
    {
        Unknown = 0,
        Delivered = 1,
        Failed = -1,
        Cancelled = -2
    }
}
