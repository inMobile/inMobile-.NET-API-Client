using System;

namespace Sms.ApiClient.V2.SendMessages
{
	public class SendMessageException : Exception
	{
		public int ErrorCode { get; set; }
		public SendMessageException(int errorCode) : base("Error code received when sending message: " + errorCode)
		{
			ErrorCode = errorCode;
		}
	}
}
