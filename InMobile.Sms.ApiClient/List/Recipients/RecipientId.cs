namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// The id type for a recipient
    /// </summary>
    public class RecipientId : CustomStringTypeBase<RecipientId>
    {
        /// <summary>
        /// Create a new recipient id
        /// </summary>
        /// <param name="value"></param>
        public RecipientId(string value) : base(value)
        {
        }
    }
}
