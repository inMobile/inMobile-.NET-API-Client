using System;

namespace Sms.ApiClient.V2.CancelMessages
{
	public class CancelMessagesException : Exception
	{
		public CancelMessagesException()
		{
		}

		public CancelMessagesException(string message) : base(message)
		{
		}

		public CancelMessagesException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}