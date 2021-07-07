using System;
using System.Collections.Generic;
using System.IO;

namespace InMobile.Sms.ApiClient.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var apiKey = new InmobileApiKey(File.ReadAllText("c:\\temp\\DOTNET_API_CLIENT\\apikey.txt"));
            var msisdn = File.ReadAllText("c:\\temp\\DOTNET_API_CLIENT\\msisdn.txt");
            var client = new InmobileApiClient(apiKey: apiKey);
            var result = client.SmsOutgoing.SendSmsMessages(new List<OutgoingSmsMessageCreateInfo>() {
                new OutgoingSmsMessageCreateInfo(
                    to: msisdn,
                    from: "1245",
                    text: "Hello world",
                    messageId: "demo_message_" + DateTime.Now.Ticks,
                    respectBlacklist: true,
                    flash: false,
                    encoding: MessageEncoding.UCS2,
                    validityPeriod: TimeSpan.FromMinutes(2) )
            });

            result.ToString();
            Console.WriteLine("Done");
            Console.Read();
        }
    }
}
