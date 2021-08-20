﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Represents and entry in the blacklist
    /// </summary>
    public class BlacklistEntry
    {
        /// <summary>
        /// The id of the blacklist entry
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public BlacklistEntryId Id { get; set; }
        /// <summary>
        /// The number information
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public NumberInfo NumberInfo { get; set; }
        /// <summary>
        /// An optional comment
        /// </summary>
        [JsonProperty()]
        public string? Comment { get; set; }
        
        [JsonConstructor]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private BlacklistEntry()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }
    }
}
