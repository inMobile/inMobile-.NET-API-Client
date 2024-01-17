namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// The id type for a recipient list
    /// </summary>
    public class RecipientListId : CustomStringTypeBase<RecipientListId>
    {
        /// <summary>
        /// Create a new recipient list id
        /// </summary>
        /// <param name="value"></param>
        public RecipientListId(string value) : base(value)
        {
        }
    }
}
