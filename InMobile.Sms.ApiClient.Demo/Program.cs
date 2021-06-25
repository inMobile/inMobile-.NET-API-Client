using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace InMobile.Sms.ApiClient.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var apiKey = new InMobileApiKey(File.ReadAllText("c:\\temp\\DOTNET_API_CLIENT\\apikey.txt"));
            var client = new InMobileApiClient(apiKey: apiKey);

            RunRealWorldTest_SendSms(client: client);
            //RunRealWorldTest_Lists(client: client);
            //RunRealWorldTest_Blacklist(client: client);

            Console.WriteLine("Done");
            Console.Read();
        }

        private static void RunRealWorldTest_SendSms(InMobileApiClient client)
        {
            Log("::: SEND SMS :::");
            client.SmsOutgoing.SendSmsMessages(new List<OutgoingSmsMessageCreateRequest>() {
                    new OutgoingSmsMessageCreateRequest(to: "4528744985", text: "test", from: "1245", statusCallbackUrl: "http://technical.fail", validityPeriod: TimeSpan.FromMinutes(10), sendTime: DateTime.Now.AddMinutes(1))
                });
        }

        private static void RunRealWorldTest_Blacklist(InMobileApiClient client)
        {
            Log("::: BLACKLIST :::");
            var startTime = DateTime.Now;

            var countBefore = client.Blacklist.GetAll().Count;
            Log("Adding some entries");
            client.Blacklist.Add(countryCode: "45", number: "111111");
            client.Blacklist.Add(countryCode: "47", number: "222222");
            Log("Checking new entry count");
            var entries = client.Blacklist.GetAll();
            var countAfter = entries.Count;
            AssertEquals(countBefore + 2, countAfter);

            Log("deleting an entry");
            {
                var entryToDelete = client.Blacklist.GetAll().Single(e => e.NumberInfo.CountryCode == "47" && e.NumberInfo.PhoneNumber == "222222");
                client.Blacklist.RemoveById(blacklistEntryId: entryToDelete.Id);
            }

            Log("ensure deleted");
            AssertEquals(countBefore + 1, client.Blacklist.GetAll().Count);

            Log("testing deletion of invalid id");
            try
            {
                client.Blacklist.RemoveById(blacklistEntryId: "487c1687-eef3-4175-ac3c-725166bf6f07");
                throw new Exception("Expected exception here");
            }
            catch (InMobileApiException ex) when (ex.ErrorHttpStatusCode == System.Net.HttpStatusCode.NotFound)
            {
                // Expected
                Log("Exception catched as expected");
            }

            Log("deleting the other entry by number");
            {
                client.Blacklist.RemoveByNumber(countryCode: "45", phoneNumber: "111111");
            }

            Log("Verifying deleted");
            AssertEquals(countBefore, client.Blacklist.GetAll().Count);
            Log($"Done in {DateTime.Now.Subtract(startTime).TotalSeconds} seconds");
        }

        private static void RunRealWorldTest_Lists(InMobileApiClient client)
        {
            Log("::: LIST + RECIPIENT TEST :::");
            var startTime = DateTime.Now;
            var testListName = "Auto-test-list_" + Guid.NewGuid();
            var listCountBeforeCreate = client.Lists.GetAllLists().Count;
            var list = client.Lists.CreateList(name: testListName);
            var listCountAfterCreate = client.Lists.GetAllLists().Count;
            if (listCountBeforeCreate != listCountAfterCreate - 1)
                throw new Exception("Before create: " + listCountBeforeCreate + ", after create: " + listCountAfterCreate);

            Log("Create some recipients");
            var rec1CreateInfo = new Recipient(new NumberInfo(countryCode: "45", phoneNumber: "111111"));
            rec1CreateInfo.Fields.Add("firstname", "initial firstname");
            rec1CreateInfo.Fields.Add("lastname", "initial lastname");
            var rec1 = client.Lists.CreateRecipient(listId: list.Id, rec1CreateInfo);
            var rec2 = client.Lists.CreateRecipient(listId: list.Id, new Recipient(new NumberInfo(countryCode: "45", phoneNumber: "222222")));
            // Ensure creating another entry gives a 409 conclift
            try
            {
                client.Lists.CreateRecipient(listId: list.Id, new Recipient(new NumberInfo(countryCode: "45", phoneNumber: "111111")));
                throw new Exception("Expected exception here");
            }
            catch (InMobileApiException ex) when (ex.ErrorHttpStatusCode == System.Net.HttpStatusCode.Conflict)
            {
                // Expected
            };

            // Update recipients
            Log("Update a recipient");
            client.Lists.UpdateRecipient(listId: list.Id, recipientId: rec1.Id, new { Fields = new Dictionary<string, string>() { { "lastname", "new lastname" } } });

            Log("Load and verify all recipients in test list");
            var allRecipientsInList = client.Lists.GetAllRecipientsInList(listId: list.Id);
            if (allRecipientsInList.Count != 2)
                throw new Exception($"Unexpected recipient count: {allRecipientsInList.Count} expected 2");

            rec1 = allRecipientsInList.Single(r => r.NumberInfo.PhoneNumber == "111111" && r.NumberInfo.CountryCode == "45");
            rec2 = allRecipientsInList.Single(r => r.NumberInfo.PhoneNumber == "222222" && r.NumberInfo.CountryCode == "45");

            AssertEquals("initial firstname", rec1.Fields["firstname"]);
            AssertEquals("new lastname", rec1.Fields["lastname"]);

            Log("Delete recipient");
            client.Lists.DeleteRecipientByNumber(listId: list.Id, countryCode: "45", phoneNumber: "111111");
            AssertEquals(1, client.Lists.GetAllRecipientsInList(listId: list.Id).Count);

            Log("Delete all recipients");
            client.Lists.DeleteAllRecipientsInList(listId: list.Id);

            AssertEquals(0, client.Lists.GetAllRecipientsInList(listId: list.Id).Count);
            Log("Delete list");
            client.Lists.DeleteListById(listId: list.Id);

            Log("Verify lists is gone");
            try
            {
                client.Lists.GetAllRecipientsInList(listId: list.Id);
                throw new Exception("Expected NotFound");
            }
            catch (InMobileApiException ex) when (ex.ErrorHttpStatusCode == System.Net.HttpStatusCode.NotFound)
            {
                // Expected
            }

            Log($"Done in {DateTime.Now.Subtract(startTime).TotalSeconds} seconds");
        }

        private static void AssertEquals(object o1, object o2)
        {
            if (!o1.Equals(o2))
                throw new Exception($"Not equal. o1: {o1}, o2: {o2}");
        }

        private static DateTime? _lastLog = null;
        private static void Log(string msg)
        {
            var now = DateTime.Now;
            var elapsedString = _lastLog != null ? $"{(int)now.Subtract(_lastLog.Value).TotalMilliseconds}ms" : "";
            _lastLog = now;
            Console.WriteLine(" => " + elapsedString);
            Console.Write($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}: {msg}");
        }
    }
}
