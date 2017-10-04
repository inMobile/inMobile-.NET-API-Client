namespace Sms.ApiClient.V2.SendMessages
{
	public class MsisdnAndMessageId
	{
		public string Msisdn { get; set; }
		public string MessageId { get; set; }

		public MsisdnAndMessageId(string msisdn, string messageId)
		{
			Msisdn = msisdn;
			MessageId = messageId;
		}
	}
}
