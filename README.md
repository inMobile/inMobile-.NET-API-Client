# inMobile-.NET-API-Client
Official .NET client for the inMobile V4 API

You can always download the latest release as a NuGet package <a href="https://www.nuget.org/packages/inMobile.NET.API.Client/" >here.</a>

## Sending SMS messages

### SMS: Send messages

```c#
// Wrap the following in a try/catch - should also be done for all calls to the api
try
{
    var result = client.SmsOutgoing.SendSmsMessages(new List<OutgoingSmsMessageCreateInfo>() {
        new OutgoingSmsMessageCreateInfo(to: "4511223344", text: "Hello world", from: "FancyShop", statusCallbackUrl: null)
    });
    // statusCallbackUrl: Specify a url if you want report callbacks
}
catch (InMobileApiException ex)
{
    Console.WriteLine("Unexpected exception: " + ex.Message);
}

```

### SMS: Pull message statuses

Note that this is the PULL version of getting statuses.

```c#
var reports = client.SmsOutgoing.GetStatusReports();
```



## Blacklist

### Blacklist: Add

```c#
var blacklistEntry = client.Blacklist.Add(new NumberInfo(countryCode: "45", phoneNumber: "11223344"));
```

### Blacklist: Get all

```c#
var allBlacklistEntries = client.Blacklist.GetAll();
```

### Blacklist: Get by id

```c#
var blacklistEntry = client.Blacklist.GetById(blacklistEntryId: new BlacklistEntryId("7b69cc8c-aafd-4697-917a-6ecd314def5e"));
```

### Blacklist: Get by number

```c#
var blacklistEntry = client.Blacklist.GetByNumber(new NumberInfo(countryCode: "45", phoneNumber: "12345678"));
```

### Blacklist: Remove by id

```c#
client.Blacklist.RemoveById(blacklistEntryId: new BlacklistEntryId("f0d0767b-5f9e-4d33-8155-1c29fefd8238"));
```

### Blacklist: Remove by number

```c#
client.Blacklist.RemoveByNumber(new NumberInfo(countryCode: "45", phoneNumber: "12345678"));
```



## Lists and recipients

### Lists: Create

```c#
var createdList = client.Lists.CreateList(name: "My list name");
```

### Lists: Get all

```c#
var allLists = client.Lists.GetAllLists();
```

### Lists: Get by id

```c#
var list = client.Lists.GetListById(listId: new RecipientListId("af20c37d-c9d2-4343-8c46-8c8fbc5c5b14"));
```

### Lists: Update (without loading the list first)

```c#
var updatedList = client.Lists.UpdateList(new RecipientListUpdateInfo(
                        listId: new RecipientListId("ff5d0e4f-02a3-4930-8bb8-11da43bd7ab8"),
                        name: "New list name"));
```

This will reduce the network latency during the whole operation and considerably reduce the risk of lost updates compared to the next example with (load, update, save).

### Lists: Update (load, update, save)

```c#
var list = client.Lists.GetListById(listId: new RecipientListId("ff5d0e4f-02a3-4930-8bb8-11da43bd7ab8"));
            list.Name = "New list name";
            client.Lists.UpdateList(list);
```

There could be a risk of lost updates, if multiple sources manipulate the recipients at the same time.

### Lists: Delete by id

```c#
client.Lists.DeleteListById(listId: new RecipientListId("1b6415e9-9f94-419a-9d9b-21974f6586e7"));
```

### Recipients: Create

```c#
var newRecipient = new RecipientCreateInfo(listId: new RecipientListId("a2e2dfee-4a45-44b7-98fd-223399c31dba"),
                                           numberInfo: new NumberInfo(countryCode: "45", phoneNumber: "11223344"));
newRecipient.Fields.Add("email", "some_email@mywebsite.com"); // Optional
var createdRecipient = client.Lists.CreateRecipient(recipient: newRecipient);
```

### Recipients: Get all

```c#
var allRecipients = client.Lists.GetAllRecipientsInList(listId: new RecipientListId("6e076753-3d8e-4603-8ff8-66b6b6d8ff82"));
```

### Recipients: Get by id

```c#
var recipient = client.Lists.GetRecipientById(listId: new RecipientListId("6e076753-3d8e-4603-8ff8-66b6b6d8ff82"),
                                              recipientId: new RecipientId("d317de6f-234c-401d-9bd8-6eaa3b5f3b35"));
```

### Recipients: Get by number

```c#
var recipient = client.Lists.GetRecipientByNumber(
                                listId: new RecipientListId("0dd17a28-c392-486c-8f8d-8ab897b07c39"),
                                numberInfo: new NumberInfo(countryCode: "45",
                                phoneNumber: "12345678"));
```

### Recipients: Update (without loading the recipient first)

```C#
client.Lists.UpdateRecipient(new RecipientUpdateInfo(
                             recipientId: new RecipientId("d317de6f-234c-401d-9bd8-6eaa3b5f3b35"),
                             listId: new RecipientListId("6e076753-3d8e-4603-8ff8-66b6b6d8ff82"),
                             numberInfo: new NumberInfo("33", "999999"),
                             fields: new Dictionary<string, string>() {
                               { "firstname", "Bill" },
                               { "Custom2", "Val2" }
                             }));
```

This will reduce the network latency during the whole operation and considerably reduce the risk of lost updates compared to the next example with (load, update, save).

### Recipients: Update (load, update, save)

```c#
// Load recipient
var recipient = client.Lists.GetRecipientById(listId: new RecipientListId("6e076753-3d8e-4603-8ff8-66b6b6d8ff82"),
                                                          recipientId: new RecipientId("d317de6f-234c-401d-9bd8-6eaa3b5f3b35"));
// Update desired values (In the example we update the msisdn and the email)
recipient.NumberInfo = new NumberInfo(countryCode: "45", phoneNumber: "99998888");
recipient.Fields["email"] = "some_new_email@mydomain.com";
```

There could be a risk of lost updates, if multiple sources manipulate the recipients at the same time.

### Recipients: Delete by id

```c#
client.Lists.DeleteRecipientById(listId: new RecipientListId("8b481e37-8709-455a-9b74-74efe99ac7de"),
                                 recipientId: new RecipientId("a99317fc-141a-4848-8672-367750bc61b0"));
```

### Recipients: Delete by number

```c#
client.Lists.DeleteRecipientByNumber(listId: new RecipientListId("8b481e37-8709-455a-9b74-74efe99ac7de"),
                                     numberInfo: new NumberInfo(
                                        countryCode: "45",
                                        phoneNumber: "12345678"));
```

