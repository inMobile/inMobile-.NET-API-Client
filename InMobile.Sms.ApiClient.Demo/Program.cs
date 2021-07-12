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
            var allLists = client.Lists.GetAllLists();
            foreach(var list in allLists)
            {
                var allRecipients = client.Lists.GetAllRecipientsInList(listId: list.Id);
                
                Console.WriteLine(list.Name + ": " + allRecipients.Count);
                Console.ReadLine();
            }
            allLists.ToString();

            Console.WriteLine("Done");
            Console.Read();
        }
    }
}
