﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace InMobile.Sms.ApiClient.Demo.Common
{
    public class ApiTestRunner
    {
        public void RunTest(
            InMobileApiKey apiKey,
            string msisdn,
            string statusCallbackUrl,
            SmsTemplateId smsTemplateId,
            string toEmail,
            EmailTemplateId emailTemplateId)
        {
            var client = new InMobileApiClient(apiKey: apiKey);

            // SMS
            RunRealWorldTest_SmsOutgoing(client: client, msisdn: msisdn, statusCallbackUrl: statusCallbackUrl, templateId: smsTemplateId);
            RunRealWorldTest_SmsIncoming(client: client);
            RunRealWorldTest_SmsTemplates(client: client, templateId: smsTemplateId);
            RunRealWorldTest_Lists(client: client);
            RunRealWorldTest_Blacklist(client: client);
            RunRealWorldTest_SmsGdpr(client: client);

            // Email
            RunRealWorldTest_EmailOutgoing(client: client, toEmail: toEmail, templateId: emailTemplateId);
            RunRealWorldTest_EmailTemplates(client: client, templateId: emailTemplateId);

            // Other
            RunRealWorldTest_Tools(client: client);
        }

        private static void RunRealWorldTest_SmsOutgoing(InMobileApiClient client, string msisdn, string statusCallbackUrl, SmsTemplateId templateId)
        {
            Log("::: SEND SMS :::");
            client.SmsOutgoing.SendSmsMessages(new List<OutgoingSmsMessageCreateInfo>
            {
                new OutgoingSmsMessageCreateInfo(to: msisdn, text: "test", from: "1245", statusCallbackUrl: statusCallbackUrl, validityPeriod: TimeSpan.FromMinutes(10))
            });

            Log("::: SEND SMS USING TEMPLATE :::");
            client.SmsOutgoing.SendSmsMessagesUsingTemplate(new OutgoingSmsTemplateCreateInfo(
                templateId: templateId,
                new List<OutgoingSmsTemplateMessageCreateInfo>
                {
                    new OutgoingSmsTemplateMessageCreateInfo(
                        placeholders: new Dictionary<string, string>(),
                        to: msisdn,
                        statusCallbackUrl: statusCallbackUrl)
                }));

            Log("::: CALLING REPORTS ENDPOINT :::");
            var reports = client.SmsOutgoing.GetStatusReports(limit: 250);
            Log($"Received {reports.Reports.Count} reports");
        }
        
        private static void RunRealWorldTest_SmsIncoming(InMobileApiClient client)
        {
            Log("::: GET INCOMING SMS :::");
            var messages = client.SmsIncoming.GetMessages(limit: 10);

            Log($"Received {messages.Messages.Count} reports");
        }

        private static void RunRealWorldTest_SmsTemplates(InMobileApiClient client, SmsTemplateId templateId)
        {
            Log("::: SMS TEMPLATES :::");
            var startTime = DateTime.Now;

            Log("Get all");
            var all = client.SmsTemplates.GetAll();
            if (!all.Any())
                throw new Exception("Expected at least 1 SMS Template");
            if (!all.Exists(x => x.Id == templateId))
                throw new Exception($"Expected SMS template with ID: {templateId}");

            Log("Get by id");
            var reload = client.SmsTemplates.GetById(templateId);
            AssertEquals(templateId, reload.Id);

            Log($"Done in {DateTime.Now.Subtract(startTime).TotalSeconds} seconds");
        }

        private static void RunRealWorldTest_Blacklist(InMobileApiClient client)
        {
            Log("::: BLACKLIST :::");
            var startTime = DateTime.Now;

            Log("Get all");
            var all = client.Blacklist.GetAll();
            foreach (var entry in all)
            {
                Log("Delete by id");
                client.Blacklist.DeleteById(blacklistEntryId: entry.Id);
            }

            Log("Adding some entries");
            var blacklistId1 = client.Blacklist.Create(new BlacklistEntryCreateInfo(new NumberInfo(countryCode: "45", phoneNumber: "111111"), comment: "Some comment 1")).Id;
            var blacklistId2 = client.Blacklist.Create(new BlacklistEntryCreateInfo(new NumberInfo(countryCode: "47", phoneNumber: "222222"))).Id;
            Log("Checking new entry count");
            var entries = client.Blacklist.GetAll();

            AssertEquals(2, entries.Count);

            {
                Log("Get by id");
                var reload = client.Blacklist.GetById(blacklistId1);
                AssertEquals(blacklistId1, reload.Id);
                AssertEquals("45", reload.NumberInfo.CountryCode);
                AssertEquals("111111", reload.NumberInfo.PhoneNumber);
                AssertEquals("Some comment 1", reload.Comment);
            }

            {
                Log("Get by id");
                var reload = client.Blacklist.GetById(blacklistId2);
                AssertEquals(blacklistId2, reload.Id);
                AssertEquals("47", reload.NumberInfo.CountryCode);
                AssertEquals("222222", reload.NumberInfo.PhoneNumber);
                AssertEquals(null, reload.Comment);
            }

            Log("Get by id (not found)");
            AssertThrows(HttpStatusCode.NotFound, () => client.Blacklist.GetById(new BlacklistEntryId("37f3bb8c-d609-4c61-b0ed-5446651f1986")));

            Log("Get by number");
            var reload2 = client.Blacklist.GetByNumber(new NumberInfo("47", "222222"));
            AssertEquals(blacklistId2, reload2.Id);
            AssertEquals("47", reload2.NumberInfo.CountryCode);
            AssertEquals("222222", reload2.NumberInfo.PhoneNumber);

            Log("Get by number (not found)");
            AssertThrows(HttpStatusCode.NotFound, () => client.Blacklist.GetByNumber(new NumberInfo("47", "9999")));

            Log("Get all and pinpoint target entry");
            var entryToDelete = client.Blacklist.GetAll().Single(e => e.NumberInfo.CountryCode == "47" && e.NumberInfo.PhoneNumber == "222222");

            Log("deleting an entry");
            client.Blacklist.DeleteById(blacklistEntryId: entryToDelete.Id);

            Log("ensure deleted");
            AssertEquals(1, client.Blacklist.GetAll().Count);

            Log("testing deletion of invalid id");
            try
            {
                client.Blacklist.DeleteById(blacklistEntryId: new BlacklistEntryId("487c1687-eef3-4175-ac3c-725166bf6f07"));
                throw new Exception("Expected exception here");
            }
            catch (InMobileApiException ex) when (ex.ErrorHttpStatusCode == HttpStatusCode.NotFound)
            {
                // Expected
                Log("Exception catched as expected");
            }

            Log("deleting the other entry by number");
            {
                client.Blacklist.DeleteByNumber(new NumberInfo(countryCode: "45", phoneNumber: "111111"));
            }

            Log("Verifying deleted");
            AssertEquals(0, client.Blacklist.GetAll().Count);
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
            var list = client.Lists.CreateList(new RecipientListCreateInfo(name: testListName));
            Log("List: GetAll");
            var listCountAfterCreate = client.Lists.GetAllLists().Count;
            if (listCountBeforeCreate != listCountAfterCreate - 1)
                throw new Exception($"Before create: {listCountBeforeCreate}, after create: {listCountAfterCreate}");

            // List: Update
            {
                var newName = $"Auto-create-list_{Guid.NewGuid()}";
                list.Name = newName;
                Log("List: Update with list object");
                client.Lists.UpdateList(list);
                Log("List: GetById");
                list = client.Lists.GetListById(list.Id);
                AssertEquals(newName, list.Name);
            }

            {
                Log("List: Update with ListUpdateObject");
                var newName = $"Auto-create-list_{Guid.NewGuid()}";
                client.Lists.UpdateList(new RecipientListUpdateInfo(listId: list.Id, name: newName));
                var oldId = list.Id;
                list = client.Lists.GetListById(list.Id);
                AssertEquals(newName, list.Name);
                AssertEquals(oldId, list.Id); // Ensure ID not changed
            }

            // Ensure no new list was created during the updates
            AssertEquals(listCountAfterCreate, client.Lists.GetAllLists().Count);

            // Recipient: Create
            var rec1CreateInfo = new RecipientCreateInfo(listId: list.Id, new NumberInfo(countryCode: "45", phoneNumber: "111111"));
            rec1CreateInfo.Fields.Add("firstname", "initial firstname");
            rec1CreateInfo.Fields.Add("lastname", "initial lastname");
            Log("Create recipient");
            var rec1 = client.Lists.CreateRecipient(rec1CreateInfo);
            if (rec1.Id == null)
                throw new Exception("No id on recipient");
            if (rec1.Id == null)
                throw new Exception("No listId recipient");
            Log("Create recipient with ExternalCreated");
            var externalCreatedForRec2 = new DateTime(2023, 01, 12, 14, 30, 00, kind: DateTimeKind.Utc);
            var rec2 = client.Lists.CreateRecipient(new RecipientCreateInfo(listId: list.Id, new NumberInfo(countryCode: "45", phoneNumber: "222222"), externalCreated: externalCreatedForRec2));
            var reloadedRec2 = client.Lists.GetRecipientById(rec2.ListId, rec2.Id);
            AssertEquals(externalCreatedForRec2, rec2.ExternalCreated);
            Log("Create recipient");
            var rec3 = client.Lists.CreateRecipient(new RecipientCreateInfo(listId: list.Id, new NumberInfo(countryCode: "45", phoneNumber: "333333")));

            Log("Create recipient");
            var rec4CreatedStart = DateTime.Now;
            var rec4 = client.Lists.CreateRecipient(new RecipientCreateInfo(listId: list.Id, new NumberInfo(countryCode: "45", phoneNumber: "444444")));
            // Ensure creating another entry gives a 409 conclift
            Log("Create recipient (conflict)");
            AssertThrows(HttpStatusCode.Conflict, () => client.Lists.CreateRecipient(new RecipientCreateInfo(listId: list.Id, new NumberInfo(countryCode: "45", phoneNumber: "111111"))));

            // Rcipient: Update (number on recipient)
            Log("Recipient.Update");
            client.Lists.UpdateRecipient(new RecipientUpdateInfo(recipientId: rec1.Id, listId: rec1.ListId, numberInfo: new NumberInfo(countryCode: "47", phoneNumber: "99887766")));
            Log("Recipient.Load");
            rec1 = client.Lists.GetRecipientById(listId: list.Id, rec1.Id);
            AssertEquals(rec1.NumberInfo.CountryCode, "47");
            AssertEquals(rec1.NumberInfo.PhoneNumber, "99887766");
            AssertEquals(rec1.Fields["lastname"], "initial lastname");

            // Update fields on another recipient (fields only)
            Log("Recipient.Update");
            client.Lists.UpdateRecipient(new RecipientUpdateInfo(recipientId: rec2.Id, listId: rec2.ListId, fields: new Dictionary<string, string>() { { "lastname", "Smith" }, { "Custom1", "Val1" } }));
            Log("Recipient.Load");
            rec2 = client.Lists.GetRecipientById(listId: list.Id, recipientId: rec2.Id);
            AssertEquals(rec2.Fields["firstname"], null);
            AssertEquals(rec2.Fields["lastname"], "Smith");
            AssertEquals(rec2.Fields["custom1"], "Val1");
            AssertEquals(rec2.NumberInfo.CountryCode, "45");
            AssertEquals(rec2.NumberInfo.PhoneNumber, "222222");

            // Recipient: Update both fields and number
            Log("Recipient.Update");
            client.Lists.UpdateRecipient(new RecipientUpdateInfo(recipientId: rec3.Id, listId: rec3.ListId, numberInfo: new NumberInfo("33", "999999"), fields: new Dictionary<string, string>() { { "firstname", "Bill" }, { "Custom2", "Val2" } }));
            Log("Recipient.Load");
            rec3 = client.Lists.GetRecipientById(listId: rec3.ListId, recipientId: rec3.Id);
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
            rec4 = client.Lists.GetRecipientByNumber(listId: rec4.ListId, numberInfo: new NumberInfo("46", "404040"));
            AssertEquals(rec4.NumberInfo.CountryCode, "46");
            AssertEquals(rec4.NumberInfo.PhoneNumber, "404040");
            AssertEquals(rec4.Fields["firstname"], "Linus");
            AssertEquals(rec4.Fields["lastname"], null);

            Log("Load and verify all recipients in test list");
            var allRecipientsInList = client.Lists.GetAllRecipientsInList(listId: list.Id);
            if (allRecipientsInList.Count != 4)
                throw new Exception($"Unexpected recipient count: {allRecipientsInList.Count} expected 2");

            Log("Delete recipient by number");
            client.Lists.DeleteRecipientByNumber(listId: list.Id, numberInfo: new NumberInfo(countryCode: "47", phoneNumber: "99887766"));

            Log("Delete recipient by number (not found)");
            AssertThrows(HttpStatusCode.NotFound, () => client.Lists.DeleteRecipientByNumber(listId: list.Id, numberInfo: new NumberInfo(countryCode: "45", phoneNumber: "111111")));

            Log("Delete recipient by id");
            client.Lists.DeleteRecipientById(rec2.ListId, rec2.Id);

            Log("Delete recipient by id (not found)");
            AssertThrows(HttpStatusCode.NotFound, () => client.Lists.DeleteRecipientById(rec2.ListId, rec2.Id));

            AssertEquals(2, client.Lists.GetAllRecipientsInList(listId: list.Id).Count);

            Log("Delete all recipients");
            client.Lists.DeleteAllRecipientsInList(listId: list.Id);

            AssertEquals(0, client.Lists.GetAllRecipientsInList(listId: list.Id).Count);
            Log("Delete list");
            client.Lists.DeleteListById(listId: list.Id);

            Log("Verify lists is gone");
            AssertThrows(HttpStatusCode.NotFound, () => client.Lists.GetAllRecipientsInList(listId: list.Id));

            Log($"Done in {DateTime.Now.Subtract(startTime).TotalSeconds} seconds");
        }

        private static void RunRealWorldTest_SmsGdpr(InMobileApiClient client)
        {
            Log("::: SMS GDPR :::");
            var startTime = DateTime.Now;

            Log("Create Deletion Request");
            var result = client.SmsGdpr.CreateDeletionRequest(new NumberInfo("45", "11223344"));
            if (result.Id == null)
                throw new Exception("Expected to return ID");

            Log($"Done in {DateTime.Now.Subtract(startTime).TotalSeconds} seconds");
        }

        private static void RunRealWorldTest_EmailOutgoing(InMobileApiClient client, string toEmail, EmailTemplateId templateId)
        {
            Log("::: SEND EMAIL :::");
            client.EmailOutgoing.SendEmail(new OutgoingEmailCreateInfo(
                subject: "inMobile API Client - Test run",
                html: "<!DOCTYPE html><html><head></head><body><p>This is my HTML</p></body></html>",
                from: new EmailSender(emailAddress: "apitest@apitest.inmobile.com", displayName: "inMobile Support"),
                to: new List<EmailRecipient>
                {
                    new EmailRecipient(emailAddress: toEmail, displayName: toEmail)
                }));

            Log("::: SEND EMAIL (WITH REPLYTO) :::");
            client.EmailOutgoing.SendEmail(new OutgoingEmailCreateInfo(
                subject: "inMobile API Client - Test run",
                html: "<!DOCTYPE html><html><head></head><body><p>This is my HTML</p></body></html>",
                from: new EmailSender(emailAddress: "noreply@apitest.inmobile.com", displayName: "inMobile Support"),
                to: new List<EmailRecipient>
                {
                    new EmailRecipient(emailAddress: toEmail, displayName: toEmail)
                },
                replyTo: new List<EmailReplyToRecipient> { new EmailReplyToRecipient(emailAddress: "apitest@apitest.inmobile.com") }));

            Log("::: SEND EMAIL USING TEMPLATE :::");
            client.EmailOutgoing.SendEmailUsingTemplate(new OutgoingEmailTemplateCreateInfo(
                templateId: templateId,
                from: new EmailSender(emailAddress: "apitest@apitest.inmobile.com", displayName: "inMobile Support"),
                to: new List<EmailRecipient>
                {
                    new EmailRecipient(emailAddress: toEmail, displayName: toEmail)
                }));

                        
            Log("::: SEND EMAIL USING TEMPLATE (WITH REPLYTO) :::");
            client.EmailOutgoing.SendEmailUsingTemplate(new OutgoingEmailTemplateCreateInfo(
                templateId: templateId,
                from: new EmailSender(emailAddress: "noreply@apitest.inmobile.com", displayName: "inMobile Support"),
                to: new List<EmailRecipient>
                {
                    new EmailRecipient(emailAddress: toEmail, displayName: toEmail)
                },
                replyTo: new List<EmailReplyToRecipient> { new EmailReplyToRecipient(emailAddress: "apitest@apitest.inmobile.com") }));

            
            Log("::: CALLING EMAIL EVENTS ENDPOINT :::");
            var events = client.EmailOutgoing.GetEmailEvents(limit: 250);
            Log($"Received {events.Events.Count} events");
        }

        private static void RunRealWorldTest_EmailTemplates(InMobileApiClient client, EmailTemplateId templateId)
        {
            Log("::: EMAIL TEMPLATES :::");
            var startTime = DateTime.Now;

            Log("Get all");
            var all = client.EmailTemplates.GetAll();
            if (!all.Any())
                throw new Exception("Expected at least 1 email Template");
            if (!all.Exists(x => x.Id == templateId))
                throw new Exception($"Expected email template with ID: {templateId}");

            Log("Get by id");
            var reload = client.EmailTemplates.GetById(templateId);
            AssertEquals(templateId, reload.Id);

            Log($"Done in {DateTime.Now.Subtract(startTime).TotalSeconds} seconds");
        }

        private static void RunRealWorldTest_Tools(InMobileApiClient client)
        {
            Log("::: TOOLS :::");
            var startTime = DateTime.Now;

            Log("Parse Phone Numbers");
            var result = client.Tools.ParsePhoneNumbers(new List<ParsePhoneNumberInfo> { new ParsePhoneNumberInfo("DK", "+45 12 34 56 78") });
            var singleResult = result.Results.Single();

            AssertEquals(true, singleResult.IsValidMsisdn);
            AssertEquals("4512345678", singleResult.Msisdn);
            AssertEquals("45", singleResult.CountryCode);
            AssertEquals("12345678", singleResult.PhoneNumber);
            AssertEquals("DK", singleResult.CountryHint);
            AssertEquals("+45 12 34 56 78", singleResult.RawMsisdn);

            Log($"Done in {DateTime.Now.Subtract(startTime).TotalSeconds} seconds");
        }

        private static void AssertThrows(HttpStatusCode expectedStatusCode, Action action)
        {
            var thrown = false;
            try
            {
                action();
            }
            catch (InMobileApiException ex) when (ex.ErrorHttpStatusCode == expectedStatusCode)
            {
                thrown = true;
            }
            if (!thrown)
            {
                throw new Exception("No exception was thrown");
            }
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

        private static DateTime? _lastLog;
        private static void Log(string msg)
        {
            var now = DateTime.Now;
            var elapsedString = _lastLog != null ? $"{(int)now.Subtract(_lastLog.Value).TotalMilliseconds}ms" : "";
            _lastLog = now;
            Console.WriteLine(" => " + elapsedString);
            Console.Write($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}: {msg}");
        }
    }
}
