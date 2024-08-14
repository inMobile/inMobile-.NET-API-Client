namespace InMobile.Sms.ApiClient.Test;

public class UnitTestResponseInfo
{
    public string JsonOrNull { get; }
    public string StatusCodeString { get; }

    public UnitTestResponseInfo(string jsonOrNull, string statusCodeString = "200 Ok")
    {
        JsonOrNull = jsonOrNull;
        StatusCodeString = statusCodeString;
    }
}