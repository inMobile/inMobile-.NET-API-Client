using System.Collections.Generic;

namespace Sms.ApiClient.V2.SendMessages
{
	internal interface ISendMessagesClient
	{
		SendMessagesResponse SendMessages(List<ISmsMessage> messages, string messageStatusCallbackUrl = null);
	}
}