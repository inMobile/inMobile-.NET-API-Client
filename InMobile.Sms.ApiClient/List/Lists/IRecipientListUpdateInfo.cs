namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Information required for updating a list.
    /// </summary>
    public interface IRecipientListUpdateInfo
    {
        /// <summary>
        /// The id of the list.
        /// </summary>
        RecipientListId Id { get; }
        /// <summary>
        /// The name of list.
        /// </summary>
        string Name { get; }
    }
}
