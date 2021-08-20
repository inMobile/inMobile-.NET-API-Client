using System;
using System.Collections.Generic;
using System.Text;

namespace InMobile.Sms.ApiClient
{
    public class CustomStringTypeBase<TOwnType> : CustomDataTypeBase<string, TOwnType> where TOwnType : CustomStringTypeBase<TOwnType>
    {
        public CustomStringTypeBase(string value) : base(value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"'{nameof(value)}' cannot be null or whitespace.", nameof(value));
            }
        }
    }
}
