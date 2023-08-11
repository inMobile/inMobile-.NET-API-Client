using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Information required for creating a new recipient.
    /// </summary>
    public class RecipientCreateInfo
    {
        /// <summary>
        /// The id which the new recipient should be created in.
        /// </summary>
        public RecipientListId ListId { get; }

        /// <summary>
        /// The number information.
        /// </summary>
        public NumberInfo NumberInfo { get; }

        /// <summary>
        /// Additional fields.
        /// </summary>
        public Dictionary<string, string> Fields { get; }

        /// <summary>
        /// When the recipient was created in an external system e.g. 2001-02-24T14:50:23Z (UTC time).
        /// </summary>
        public DateTime? ExternalCreated { get; private set; }

        /// <summary>
        /// Create a new create-object with additional fields information.
        /// </summary>
        /// <param name="listId">The id which the new recipient should be created in.</param>
        /// <param name="numberInfo">The number information.</param>
        /// <param name="fields">Additional fields.</param>
        /// <param name="externalCreated">Optional: When the recipient was created in an external system e.g. 2001-02-24T14:50:23Z (UTC time).</param>
        public RecipientCreateInfo(RecipientListId listId, NumberInfo numberInfo, Dictionary<string, string> fields, DateTime? externalCreated = null)
        {
            ListId = listId ?? throw new ArgumentNullException(nameof(listId));
            NumberInfo = numberInfo ?? throw new ArgumentNullException(nameof(numberInfo));
            Fields = fields ?? throw new ArgumentNullException(nameof(fields));
            ExternalCreated = externalCreated;
        }

        /// <summary>
        /// Create a new create-object without any additional field information.
        /// </summary>
        /// <param name="listId">The id which the new recipient should be created in.</param>
        /// <param name="numberInfo">The number information.</param>
        /// <param name="externalCreated">Optional: When the recipient was created in an external system e.g. 2001-02-24T14:50:23Z (UTC time).</param>
        public RecipientCreateInfo(RecipientListId listId, NumberInfo numberInfo, DateTime? externalCreated = null) 
            : this(listId: listId, numberInfo: numberInfo, fields: new Dictionary<string, string>(), externalCreated: externalCreated)
        {
        }
    }
}
