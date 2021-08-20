using System;
using System.Collections.Generic;
using System.Text;

namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Message state code
    /// </summary>
    public enum MessageStateCode
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// The sms message was delivered to the device.
        /// </summary>
        Delivered = 1,
        /// <summary>
        /// Delivering failed for some reason.
        /// </summary>
        Failed = -1,
        /// <summary>
        /// The message was cancelled before sent to the device.
        /// </summary>
        Cancelled = -2
    }
}
