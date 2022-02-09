using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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
            runner.RunTest(apiKey);

            Console.WriteLine("Done");
            Console.Read();
        }

    }
}
