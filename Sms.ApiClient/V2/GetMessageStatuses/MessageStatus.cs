namespace Sms.ApiClient.V2.GetMessageStatuses
{
	public class MessageStatus
	{
		public int StatusCode { get; set; }
		public string StatusDescription { get; set; }
		public string MessageId { get; set; }

		public MessageStatus(string messageId, int statusCode, string statusDescription)
		{
			StatusDescription = statusDescription;
			MessageId = messageId;
			StatusCode = statusCode;
		}

		public override string ToString()
		{
			return "[Id: " + MessageId + ", StatusCode:" + StatusCode + ", StatusDescription: " + StatusDescription + "]";
		}
	}
}