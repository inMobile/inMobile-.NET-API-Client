using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using InMobile.Sms.ApiClient.Demo.Common;

namespace InMobile.Sms.ApiClient.Demo
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
