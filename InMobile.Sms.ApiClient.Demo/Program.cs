namespace InMobile.Sms.ApiClient.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new InmobileApiClient(apiKey: new InmobileApiKey(apiKey: "[Insert key]"));
        }
    }
}
