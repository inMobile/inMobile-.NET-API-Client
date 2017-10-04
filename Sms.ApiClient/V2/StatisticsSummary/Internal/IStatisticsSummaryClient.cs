using System;

namespace Sms.ApiClient.V2.StatisticsSummary
{
	internal interface IStatisticsSummaryClient
	{
		StatisticsSummaryResponse GetStatistics(DateTime from, DateTime to);
	}
}