using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
