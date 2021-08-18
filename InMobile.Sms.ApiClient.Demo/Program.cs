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
            RunRealWorldTest_Lists(client: client);
            RunRealWorldTest_Blacklist(client: client);

            Console.WriteLine("Done");
            Console.Read();
        }

        private static void RunRealWorldTest_SendSms(InMobileApiClient client)
        {
            Log("::: SEND SMS :::");
            client.SmsOutgoing.SendSmsMessages(new List<OutgoingSmsMessageCreateInfo>() {
                    new OutgoingSmsMessageCreateInfo(to: "45...", text: "test", from: "1245", statusCallbackUrl: "http://technical.fail", validityPeriod: TimeSpan.FromMinutes(10), sendTime: DateTime.Now.AddMinutes(1))
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

            // List: Create
            var startTime = DateTime.Now;
            var testListName = "Auto-test-list_" + Guid.NewGuid();
            var listCountBeforeCreate = client.Lists.GetAllLists().Count;
            Log("List: Create");
            var list = client.Lists.CreateList(name: testListName);
            Log("List: GetAll");
            var listCountAfterCreate = client.Lists.GetAllLists().Count;
            if (listCountBeforeCreate != listCountAfterCreate - 1)
                throw new Exception("Before create: " + listCountBeforeCreate + ", after create: " + listCountAfterCreate);

            // List: Update
            {
                var newName = "Auto-create-list_" + Guid.NewGuid();
                list.Name = newName;
                Log("List: Update with list object");
                client.Lists.UpdateList(list);
                Log("List: GetById");
                list = client.Lists.GetListById(list.ListId);
                AssertEquals(newName, list.Name);
            }

            {
                Log("List: Update with ListUpdateObject");
                var newName = "Auto-create-list_" + Guid.NewGuid();
                client.Lists.UpdateList(new RecipientListUpdateInfo(listId: list.ListId, name: newName));
                var oldId = list.ListId;
                list = client.Lists.GetListById(list.ListId);
                AssertEquals(newName, list.Name);
                AssertEquals(oldId, list.ListId); // Ensure ID not changed
            }

            // Ensure no new list was created during the updates
            AssertEquals(listCountAfterCreate, client.Lists.GetAllLists().Count);

            // Recipient: Create
            var rec1CreateInfo = new RecipientCreateInfo(listId: list.ListId, new NumberInfo(countryCode: "45", phoneNumber: "111111"));
            rec1CreateInfo.Fields.Add("firstname", "initial firstname");
            rec1CreateInfo.Fields.Add("lastname", "initial lastname");
            Log("Create recipient");
            var rec1 = client.Lists.CreateRecipient(rec1CreateInfo);
            if (string.IsNullOrWhiteSpace(rec1.RecipientId))
                throw new Exception("No id on recipient");
            if (string.IsNullOrWhiteSpace(rec1.ListId))
                throw new Exception("No listId recipient");
            Log("Create recipient");
            var rec2 = client.Lists.CreateRecipient(new RecipientCreateInfo(listId: list.ListId, new NumberInfo(countryCode: "45", phoneNumber: "222222")));
            Log("Create recipient");
            var rec3 = client.Lists.CreateRecipient(new RecipientCreateInfo(listId: list.ListId, new NumberInfo(countryCode: "45", phoneNumber: "333333")));
            Log("Create recipient");

            var rec4CreatedStart = DateTime.Now;
            var rec4 = client.Lists.CreateRecipient(new RecipientCreateInfo(listId: list.ListId, new NumberInfo(countryCode: "45", phoneNumber: "444444")));
            // Ensure creating another entry gives a 409 conclift
            try
            {
                Log("Create recipient");
                client.Lists.CreateRecipient(new RecipientCreateInfo(listId: list.ListId, new NumberInfo(countryCode: "45", phoneNumber: "111111")));
                throw new Exception("Expected exception here");
            }
            catch (InMobileApiException ex) when (ex.ErrorHttpStatusCode == System.Net.HttpStatusCode.Conflict)
            {
                // Expected
            };

            // Do some externalCreatedDate assertions
            AssertEquals(null, rec4.ExternalCreated);
            if (rec4.Created < startTime)
                throw new Exception("Unexpected created: " + rec4.Created + " startTime: " + startTime);
            var now = DateTime.Now;
            if (rec4.Created > now)
                throw new Exception("Unexpected created: " + rec4.Created + " now: " + now);

            AssertEquals(null, rec4.Created);

            client.Lists.UpdateRecipient(new RecipientUpdateInfo(
                        recipientId: "d317de6f-234c-401d-9bd8-6eaa3b5f3b35",
                        listId: "6e076753-3d8e-4603-8ff8-66b6b6d8ff82",
                        numberInfo: new NumberInfo(countryCode: "45", phoneNumber: "99998888")));

            // Rcipient: Update (number on recipient)
            Log("Recipient.Update");
            client.Lists.UpdateRecipient(new RecipientUpdateInfo(recipientId: rec1.RecipientId, listId: rec1.ListId, numberInfo: new NumberInfo(countryCode: "47", phoneNumber: "99887766")));
            Log("Recipient.Load");
            rec1 = client.Lists.GetRecipientById(listId: list.ListId, rec1.RecipientId);
            AssertEquals(rec1.NumberInfo.CountryCode, "47");
            AssertEquals(rec1.NumberInfo.PhoneNumber, "99887766");
            AssertEquals(rec1.Fields["lastname"], "initial lastname");

            // Update fields on another recipient (fields only)
            Log("Recipient.Update");
            client.Lists.UpdateRecipient(new RecipientUpdateInfo(recipientId: rec2.RecipientId, listId: rec2.ListId, fields: new Dictionary<string, string>() { { "lastname", "Smith" }, { "Custom1", "Val1" } }));
            Log("Recipient.Load");
            rec2 = client.Lists.GetRecipientById(listId: list.ListId, recipientId: rec2.RecipientId);
            AssertEquals(rec2.Fields["firstname"], null);
            AssertEquals(rec2.Fields["lastname"], "Smith");
            AssertEquals(rec2.Fields["custom1"], "Val1");
            AssertEquals(rec2.NumberInfo.CountryCode, "45");
            AssertEquals(rec2.NumberInfo.PhoneNumber, "222222");

            // Recipient: Update both fields and number
            Log("Recipient.Update");
            client.Lists.UpdateRecipient(new RecipientUpdateInfo(recipientId: rec3.RecipientId, listId: rec3.ListId, numberInfo: new NumberInfo("33", "999999"), fields: new Dictionary<string, string>() { { "firstname", "Bill" }, { "Custom2", "Val2" } }));
            Log("Recipient.Load");
            rec3 = client.Lists.GetRecipientById(listId: rec3.ListId, recipientId: rec3.RecipientId);
            AssertEquals(rec3.NumberInfo.CountryCode, "33");
            AssertEquals(rec3.NumberInfo.PhoneNumber, "999999");
            AssertEquals(rec3.Fields["firstname"], "Bill");
            AssertEquals(rec3.Fields["custom2"], "Val2");

            // Recipient: Update with loaded object
            rec4.NumberInfo.CountryCode = "46";
            rec4.NumberInfo.PhoneNumber = "404040";
            rec4.Fields["custom3"] = "val3";
            rec4.Fields["firstname"] = "Linus";
            Log("Recipient.Update");
            client.Lists.UpdateRecipient(rec4);
            Log("Recipient.Load");
            rec4 = client.Lists.GetRecipientById(listId: rec4.ListId, recipientId: rec4.RecipientId);
            AssertEquals(rec4.NumberInfo.CountryCode, "46");
            AssertEquals(rec4.NumberInfo.PhoneNumber, "404040");
            AssertEquals(rec4.Fields["firstname"], "Linus");
            AssertEquals(rec4.Fields["lastname"], null);

            Log("Load and verify all recipients in test list");
            var allRecipientsInList = client.Lists.GetAllRecipientsInList(listId: list.ListId);
            if (allRecipientsInList.Count != 4)
                throw new Exception($"Unexpected recipient count: {allRecipientsInList.Count} expected 2");

            Log("Delete recipient");
            client.Lists.DeleteRecipientByNumber(listId: list.ListId, countryCode: "47", phoneNumber: "99887766");
            AssertEquals(3, client.Lists.GetAllRecipientsInList(listId: list.ListId).Count);

            try
            {
                Log("Delete but not found test");
                client.Lists.DeleteRecipientByNumber(listId: list.ListId, countryCode: "45", phoneNumber: "111111");
            }
            catch (InMobileApiException ex) when (ex.ErrorHttpStatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Log("Success");
            };

            Log("Delete all recipients");
            client.Lists.DeleteAllRecipientsInList(listId: list.ListId);

            AssertEquals(0, client.Lists.GetAllRecipientsInList(listId: list.ListId).Count);
            Log("Delete list");
            client.Lists.DeleteListById(listId: list.ListId);

            Log("Verify lists is gone");
            try
            {
                client.Lists.GetAllRecipientsInList(listId: list.ListId);
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
            if (o1 == null && o2 == null)
                return;
            if (o1 == null)
                throw new Exception($"o1 is NULL, o2 is {o2}");
            if (o2 == null)
                throw new Exception($"o1 is {o1}, o2 is NULL");
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
