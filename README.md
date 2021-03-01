# inMobile-.NET-API-Client
Official .NET client for the inMobile API 

You can always download the latest release as a NuGet package <a href="https://www.nuget.org/packages/inMobile.NET.API.Client/" >here.</a>

## Example: Send messages

```c#
// Instantiate the client to use
// NOTE: The api key can be found on top of the documentation page
var smsClient = new FacadeSmsClient(
    hostRootUrl: "https://mm.inmobile.dk",
    apiKey: "INSERT APIKEY");

// Create a list of messages to be sent
var messagesToSend = new List<ISmsMessage>();
var message = new SmsMessage(
    msisdn: "4512345678", // The mobile number including country code
    text: "This is the text to be sent.",
    senderName: "SendersName", // i.e. 1245 or FancyShop
    encoding: SmsEncoding.Gsm7,
    messageId: "[MY-MESSAGE-ID]"); // Identifier of the message (duplicate ids not allowed)
messagesToSend.Add(message);

/*
// Optional: Adding overcharge info to the message
message.OverchargeInfo = new OverchargeInfo(
    overchargePrice: 150, // Price in cents, e.g. 150 for 1,50 DKK
    shortCodeCountryCode: "45",
    shortCodeNumber: "1245",
    overchargeType: OverchargeType.Service,
    invoiceDescription: "");
*/

// Send the messages and evaluate the response
try
{
    var response = smsClient.SendMessages(
        messages: messagesToSend,
        messageStatusCallbackUrl: "http://mywebsite.com/example/messagestatus");
}
catch (SendMessageException smex)
{
    // Catch exception to see error
    Console.WriteLine(smex.Message);
}
```



## Example: Pull message statuses

```c#
// Instantiate the client to use
// NOTE: The api key can be found on top of the documentation page
var smsClient = new FacadeSmsClient(
    hostRootUrl: "https://mm.inmobile.dk",
    apiKey: "INSERT APIKEY"); // Can be found on top of the documentation page

// Get all changed message statuses since last call
// The reponse contains messageids paired with they respective message status
var response = smsClient.GetMessageStatus();

// The final response object here holds a collection of messageId and messageStatus
```



## Example: Cancel future scheduled messages

```c#
// Instantiate the client to use
// NOTE: The api key can be found on top of the documentation page
var smsClient = new FacadeSmsClient(
    hostRootUrl: "https://mm.inmobile.dk",
    apiKey: "INSERT APIKEY"); // Can be found on top of the documentation page

// Cancel the messages with id 'MessageId1' and 'MessageId2'
// The reponse contains an object with the cancelled count
// To validate the number of cancelled messages, read response.CancelCount
var response = smsClient.CancelMessage(new List<string>() { "MessageId1", "MessageId2" });
```



## Example: Refund previously overcharged message

```c#
// Instantiate the client to use
// NOTE: The api key can be found on top of the documentation page
var smsClient = new FacadeSmsClient(
    hostRootUrl: "https://mm.inmobile.dk",
    apiKey: "INSERT APIKEY");

// Create a list of messages to be sent
var messagesToSend = new List<ISmsMessage>();

// Add a refund message that points to an existing overcharged message
var message = new RefundMessage(messageIdToRefund: "MessageId123",
messageText: "We have refunded the 100 DKK previously charged from your phone bill");
messagesToSend.Add(message);

// Send the messages and evaluate the response
try
{
    var response = smsClient.SendMessages(
        messages: messagesToSend,
        messageStatusCallbackUrl: "http://mywebsite.com/example/messagestatus");
}
catch (SendMessageException smex)
{
    // Catch exception to see error
    Console.WriteLine(smex.Message);
}
```

