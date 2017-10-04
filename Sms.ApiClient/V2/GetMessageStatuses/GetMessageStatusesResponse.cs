using System;
using System.Collections.Generic;

namespace Sms.ApiClient.V2.GetMessageStatuses
{
	public class GetMessageStatusesResponse
	{
		public List<MessageStatus> MessageStatuses { get; set; }

		public GetMessageStatusesResponse(List<MessageStatus> messageStatuses)
		{
			if (messageStatuses == null)
				throw new ArgumentNullException("messageStatuses");
			MessageStatuses = messageStatuses;
		}
	}
}