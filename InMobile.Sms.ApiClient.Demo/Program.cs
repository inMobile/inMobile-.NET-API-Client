using System;
using System.Collections.Generic;

namespace InMobile.Sms.ApiClient.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            string apiKey = "[INSERT KEY]";
            var client = new InmobileApiClient(apiKey: new InmobileApiKey(apiKey: apiKey));
            var result = client.SmsOutgoing.SendSmsMessages(new List<OutgoingSmsMessageCreateInfo>() {
                new OutgoingSmsMessageCreateInfo(
                    to: "4528744985",
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
