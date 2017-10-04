using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sms.ApiClient.V2.SendMessages
{
	public class RefundMessage : ISmsMessage
	{
		public string MessageIdToRefund { get; set; }
		public string MessageText { get; set; }
		public string MessageId { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="messageIdToRefund">The id of the overcharged message which should be refunded.</param>
		/// <param name="messageText">The text sent to the mobile user with a confirmation of the refund</param>
		/// <param name="messageId">The id of this message (Optional). If not specified, the api will generate an id.</param>
		public RefundMessage(string messageIdToRefund, string messageText, string messageId = null)
		{
			if (string.IsNullOrEmpty(messageIdToRefund))
				throw new ArgumentNullException("messageIdToRefund", "messageIdToRefund must be specified");
			if (string.IsNullOrEmpty(messageText))
				throw new ArgumentNullException("messageText", "messageText must be specified");
			MessageIdToRefund = messageIdToRefund;
			MessageText = messageText;
			MessageId = messageId;
		}
	}
}
