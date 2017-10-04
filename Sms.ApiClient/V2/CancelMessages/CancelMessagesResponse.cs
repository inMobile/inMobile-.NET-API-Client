using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sms.ApiClient.V2.CancelMessages
{
	public class CancelMessagesResponse
	{
		public int CancelCount { get; set; }

		public static CancelMessagesResponse ParseResponse(string cancelResponseString)
		{
			try
			{
				const string successStartString = "success: ";
				if (cancelResponseString.StartsWith(successStartString, StringComparison.OrdinalIgnoreCase))
				{
					var cancelCount = Convert.ToInt32(cancelResponseString.Split(' ')[1]);
					return new CancelMessagesResponse()
					{
						CancelCount = cancelCount
					};
				}
				else
				{
					throw new CancelMessagesException(cancelResponseString);
				}
			}
			catch (Exception ex)
			{
				throw new CancelMessagesException("Unexpected error cancelling message(s). See inner exception for details.", ex);
			}
		}
	}
}
