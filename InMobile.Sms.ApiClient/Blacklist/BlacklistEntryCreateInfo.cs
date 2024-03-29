﻿using System;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Represents info for creating a blacklist entry
    /// </summary>
    public class BlacklistEntryCreateInfo
    {
        /// <summary>
        /// The number information
        /// </summary>
        public NumberInfo NumberInfo { get; }
        /// <summary>
        ///  An optional comment
        /// </summary>
        public string? Comment { get; }

        /// <summary>
        /// Creates a new Create-object.
        /// </summary>
        /// <param name="numberInfo">The number information</param>
        /// <param name="comment">An optional comment</param>
        public BlacklistEntryCreateInfo(NumberInfo numberInfo, string? comment = null)
        {
            NumberInfo = numberInfo ?? throw new ArgumentNullException(nameof(numberInfo));
            Comment = comment;
        }
    }
}
