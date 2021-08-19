namespace InMobile.Sms.ApiClient
{
    public class RecipientListCreateInfo
    {
        public string Name { get; }

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
