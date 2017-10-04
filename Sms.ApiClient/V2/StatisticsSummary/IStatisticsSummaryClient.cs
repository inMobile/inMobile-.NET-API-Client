using System;

namespace Sms.ApiClient.V2.StatisticsSummary
{
	public interface IStatisticsSummaryClient
	{
		StatisticsSummaryResponse GetStatistics(DateTime from, DateTime to);
	}
}
