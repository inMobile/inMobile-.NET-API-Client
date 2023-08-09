using System;
using System.IO;
using InMobile.Sms.ApiClient.Demo.Common;

namespace InMobile.Sms.ApiClient.Demo
{
    static class Program
    {
        // REPLACE VALUES BEFORE RUN
        const string TEST_MSISDN = "45...";
        const string TEST_STATUSCALLBACKURL = null;
        const string TEST_TEMPLATEID = "";

        static void Main(string[] args)
        {
            var apiKey = new InMobileApiKey(File.ReadAllText("c:\\temp\\DOTNET_API_CLIENT\\apikey.txt"));

            var runner = new ApiTestRunner();
            runner.RunTest(
                apiKey: apiKey, 
                msisdn: TEST_MSISDN, 
                statusCallbackUrl: TEST_STATUSCALLBACKURL, 
                templateId: new TemplateId(TEST_TEMPLATEID));

            Console.WriteLine("Done");
            Console.Read();
        }
    }
}
