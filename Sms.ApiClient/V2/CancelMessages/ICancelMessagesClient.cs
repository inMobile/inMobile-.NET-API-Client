using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sms.ApiClient.V2.CancelMessages
{
	public interface ICancelMessagesClient
	{
		CancelMessagesResponse CancelMessage(List<string> messageIds);
	}
}
