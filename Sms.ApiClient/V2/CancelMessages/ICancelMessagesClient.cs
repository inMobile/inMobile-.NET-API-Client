using System.Collections.Generic;

namespace Sms.ApiClient.V2.CancelMessages
{
	public interface ICancelMessagesClient
	{
		CancelMessagesResponse CancelMessage(List<string> messageIds);
	}
}