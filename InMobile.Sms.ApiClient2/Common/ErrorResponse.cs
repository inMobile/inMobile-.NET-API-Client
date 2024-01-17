namespace InMobile.Sms.ApiClient
{
    internal class ErrorResponse
    {
        public string? ErrorMessage { get; set; }
        public string[] Details { get; set; } = new string[0];
    }
}
