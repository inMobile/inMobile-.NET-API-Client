using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace InMobile.Sms.ApiClient.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                var apiKey = new InMobileApiKey(File.ReadAllText("c:\\temp\\DOTNET_API_CLIENT\\apikey.txt"));
                var msisdn = File.ReadAllText("c:\\temp\\DOTNET_API_CLIENT\\msisdn.txt");

                var client = new InMobileApiClient(apiKey: apiKey);
                var result = client.Blacklist.GetAll().ToList();

                Console.WriteLine(result.Count);
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
            
            Console.WriteLine("Done");
            Console.Read();
        }
    }
}
