namespace InMobile.Sms.ApiClient
{
    /// <summary>
    /// Information needed to create a new list.
    /// </summary>
    public class RecipientListCreateInfo
    {
        /// <summary>
        /// The name of the list.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Create a new create-object.
        /// </summary>
        /// <param name="name">The desired name of the list.</param>
        public RecipientListCreateInfo(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new System.ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            }

            Name = name;
        }
    }
}
