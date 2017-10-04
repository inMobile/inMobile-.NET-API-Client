using System.Collections.Generic;

namespace Sms.ApiClient.V2.SendMessages
{
	public interface ISendMessagesClient
	{
		SendMessagesResponse SendMessages(List<ISmsMessage> messages, string messageStatusCallbackUrl);
	}
}