using System;
using System.Collections.Generic;

namespace Sms.ApiClient.V2.SendMessages
{
	public class SendMessagesResponse
	{
		public List<MsisdnAndMessageId> MessageIds;

		public SendMessagesResponse(List<MsisdnAndMessageId> messageIds)
		{
			if (messageIds == null)
				throw new ArgumentNullException("messageIds");
			MessageIds = messageIds;
		}
	}
}