using System;
using System.IO;
using System.Threading.Tasks;
using InMobile.Sms.ApiClient.Demo.Common;

namespace InMobile.Sms.ApiClient.Demo;

static class Program
{
    // REPLACE VALUES BEFORE RUN
    const string TEST_MSISDN = "45...";
    const string TEST_STATUSCALLBACKURL = null;
    const string TEST_SMS_TEMPLATEID = "";
    const string TEST_TOEMAIL = "...@...";
    const string TEST_EMAIL_TEMPLATEID = "";

    /// <summary>
    /// SYNC TEST RUN!
    /// </summary>
    static void Main(string[] args)
    {
        var apiKey = new InMobileApiKey(File.ReadAllText("c:\\temp\\DOTNET_API_CLIENT\\apikey.txt"));
    
        var runner = new ApiTestRunner();
        runner.RunTest(
            apiKey: apiKey,
            msisdn: TEST_MSISDN,
            statusCallbackUrl: TEST_STATUSCALLBACKURL,
            smsTemplateId: new SmsTemplateId(TEST_SMS_TEMPLATEID),
            toEmail: TEST_TOEMAIL,
            emailTemplateId: new EmailTemplateId(TEST_EMAIL_TEMPLATEID));
    
        Console.WriteLine("\nDone");
        Console.Read();
    }
        
    /// <summary>
    /// ASYNC TEST RUN!
    /// </summary>
    // static async Task Main(string[] args)
    // {
    //     var apiKey = new InMobileApiKey(await File.ReadAllTextAsync("c:\\temp\\DOTNET_API_CLIENT\\apikey.txt"));
    //
    //     var runner = new ApiTestRunnerAsync();
    //     await runner.RunTestAsync(
    //         apiKey: apiKey,
    //         msisdn: TEST_MSISDN,
    //         statusCallbackUrl: TEST_STATUSCALLBACKURL,
    //         smsTemplateId: new SmsTemplateId(TEST_SMS_TEMPLATEID),
    //         toEmail: TEST_TOEMAIL,
    //         emailTemplateId: new EmailTemplateId(TEST_EMAIL_TEMPLATEID));
    //
    //     Console.WriteLine("\nDone");
    //     Console.Read();
    // }
}