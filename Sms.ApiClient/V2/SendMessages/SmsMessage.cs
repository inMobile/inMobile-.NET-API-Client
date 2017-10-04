using System;

namespace Sms.ApiClient.V2.SendMessages
{
	public class SmsMessage : ISmsMessage
	{
		public SmsMessage(string msisdn, string text, string senderName, SmsEncoding encoding, string messageId = null, DateTime? sendTime = null, bool flash = false)
		{
			MessageId = messageId ?? "";
			Msisdn = msisdn;
			Text = text;
			SenderName = senderName;
			Encoding = encoding;
			SendTime = sendTime;
			Flash = flash;
		}
		/// <summary>
		/// The message id used to identify the message.
		/// If NULL or empty string, a message id will be generated on the API server side and returned in the response.
		/// </summary>
		public string MessageId { get; set; }
		/// <summary>
		/// If true, message is a flash message
		/// </summary>
		public bool Flash { get; private set; }
		/// <summary>
		/// The phone number to send to, including country code, e.g. 4512345678
		/// </summary>
		public string Msisdn { get; private set; }
		/// <summary>
		/// The text message in the SMS
		/// </summary>
		public string Text { get; private set; }
		/// <summary>
		/// The sendername, keep this between 3 and 11 chars.
		/// </summary>
		public string SenderName { get; private set; }
		/// <summary>
		/// The encoding to use.
		/// </summary>
		public SmsEncoding Encoding { get; private set; }
		/// <summary>
		/// If NULL, the message is not overcharged.
		/// </summary>
		public OverchargeInfo OverchargeInfo { get; set; }
		/// <summary>
		/// The time for the message to be sent. use NULL to send immediately.
		/// </summary>
		public DateTime? SendTime { get; set; }
	}
}
