using System;

namespace Sms.ApiClient.V2.GetMessageStatuses
{
	public class GetMessageStatusesException : Exception
	{
		public GetMessageStatusesException(string responseError) : base(responseError)
		{
		}
	}
}
