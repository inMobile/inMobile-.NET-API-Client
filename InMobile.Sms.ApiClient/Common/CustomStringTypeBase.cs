﻿using System;

namespace InMobile.Sms.ApiClient
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
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
