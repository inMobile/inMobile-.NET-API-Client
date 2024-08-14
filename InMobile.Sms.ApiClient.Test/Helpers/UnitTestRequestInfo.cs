using System;

namespace InMobile.Sms.ApiClient.Test;

public class UnitTestRequestInfo
{
    public InMobileApiKey ApiKey { get; }
    public string MethodAndPath { get; }
    public string JsonOrNull { get; }

    public UnitTestRequestInfo(InMobileApiKey apiKey, string methodAndPath, string jsonOrNull)
    {
        ApiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        MethodAndPath = methodAndPath ?? throw new ArgumentNullException(nameof(methodAndPath));
        JsonOrNull = jsonOrNull;
    }
}