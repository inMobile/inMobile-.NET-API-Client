using Sms.ApiClient.V2;
using Sms.ApiClient.V2.SendMessages;
using System.Collections.Generic;
using Sms.ApiClient.V2.CancelMessages;

namespace Sms.ApiClient.AdHoc
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var apikey = "somekey";
			var messageidToRefund = "somemessageid";

			// Instantiate the client to use
			// NOTE: The api key can be found on top of the documentation page
			var smsClient = new FacadeSmsClient(
							hostRootUrl: "https://mm.inmobile.dk",
							apiKey: apikey);

			// Create a list of messages to be sent
			var messagesToSend = new List<ISmsMessage>();

			// Add a refund message that points to an existing overcharged message
			var message = new RefundMessage(messageIdToRefund: messageidToRefund,
			messageText: "Vi har refunderet din fejl-trækning på 50 kroner for din sms sent til 1245 med teksten '50 A...'. Mvh Stephan Ryer, Inmobile Aps, tlf 88 33 66 99.");
			messagesToSend.Add(message);

			// Send the messages and evaluate the response
			var response = smsClient.SendMessages(
			messages: messagesToSend,
			messageStatusCallbackUrl: "");

			response.ToString();
		}
	}
}