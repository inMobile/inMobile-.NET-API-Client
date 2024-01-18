namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// The email event type
    /// </summary>
    public enum EmailEventType
    {
        /// <summary>
        /// Undefined
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// 
        /// </summary>
        Clicked = 1,
        /// <summary>
        /// 
        /// </summary>
        Complained = 2,
        /// <summary>
        /// Delivered = 3,
        /// </summary>
        Delivered = 3,
        /// <summary>
        /// 
        /// </summary>
        Opened = 4,
        /// <summary>
        /// 
        /// </summary>
        PermanentFail = 5,
        /// <summary>
        /// 
        /// </summary>
        TemporaryFail = 6,
        /// <summary>
        /// 
        /// </summary>
        Unsubscribed = 7,
        /// <summary>
        /// 
        /// </summary>
        Queued = 8,
        /// <summary>
        /// 
        /// </summary>
        Attempt = 9
    }
}
