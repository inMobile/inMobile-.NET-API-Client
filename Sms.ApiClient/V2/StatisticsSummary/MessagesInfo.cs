using System.Collections.Generic;

namespace Sms.ApiClient.V2.StatisticsSummary
{
	public class MessagesInfo
	{
		public int TotalMessageCount { get; set; }
		public int TotalSmsCount { get; set; }
		public List<StatusLine> Statuses { get; set; }

		public MessagesInfo()
		{
			Statuses = new List<StatusLine>();
		}
	}
}