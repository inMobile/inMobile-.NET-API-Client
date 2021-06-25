namespace InMobile.Sms.ApiClient
{
    public class ErrorResponse
    {
        public string? ErrorMessage { get; set; }
        public string[] Details { get; set; } = new string[0];
    }
}
