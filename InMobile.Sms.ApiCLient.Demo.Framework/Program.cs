using System;
using System.IO;
using InMobile.Sms.ApiClient;
using InMobile.Sms.ApiClient.Demo.Common;

namespace InMobile.Sms.ApiCLient.Demo.Framework
{
    class Program
    {
        static void Main(string[] args)
        {
            var apiKey = new InMobileApiKey(File.ReadAllText("c:\\temp\\DOTNET_API_CLIENT\\apikey.txt"));
            var runner = new ApiTestRunner();
            runner.RunTest(apiKey: apiKey, msisdn: "45...");

            Console.WriteLine("Done");
            Console.Read();
        }
    }
}
