namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// The id type of a blacklist entry
    /// </summary>
    public class BlacklistEntryId : CustomStringTypeBase<BlacklistEntryId>
    {
        /// <summary>
        /// Creates a new BlacklistId
        /// </summary>
        /// <param name="value"></param>
        public BlacklistEntryId(string value) : base(value)
        {
        }
    }
}
