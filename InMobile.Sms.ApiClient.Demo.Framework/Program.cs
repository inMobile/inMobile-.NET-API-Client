using System;
using System.IO;
using InMobile.Sms.ApiClient.Demo.Common;

namespace InMobile.Sms.ApiClient.Demo.Framework
{
    static class Program
    {
        // REPLACE VALUES BEFORE RUN
        const string TEST_MSISDN = "45...";
        const string TEST_STATUSCALLBACKURL = null;
        const string TEST_SMS_TEMPLATEID = "";
        const string TEST_TOEMAIL = "...@...";
        const string TEST_EMAIL_TEMPLATEID = "";

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
    }
}
