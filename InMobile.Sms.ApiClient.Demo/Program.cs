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
            var apiKey = new InMobileApiKey(File.ReadAllText("c:\\temp\\DOTNET_API_CLIENT\\apikey.txt"));
            var msisdn = File.ReadAllText("c:\\temp\\DOTNET_API_CLIENT\\msisdn.txt");

            var client = new InMobileApiClient(apiKey: apiKey);
            var result = client.Lists.CreateList(name: "ApiV4TestList3");
            Console.WriteLine(result.Id + " name: " + result.Name);
            foreach (var list in client.Lists.GetAllLists())
            {
                Console.WriteLine(list.Id + ": " + list.Name);
            };

            Console.WriteLine("Done");
            Console.Read();
        }
    }
}
