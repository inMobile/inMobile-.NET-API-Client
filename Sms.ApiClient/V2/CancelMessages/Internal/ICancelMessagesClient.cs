using System.Collections.Generic;

namespace Sms.ApiClient.V2.CancelMessages
{
	internal interface ICancelMessagesClient
	{
		CancelMessagesResponse CancelMessage(List<string> messageIds);
	}
}