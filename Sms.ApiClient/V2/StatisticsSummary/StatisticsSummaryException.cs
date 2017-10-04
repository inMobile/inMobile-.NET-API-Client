using System;

namespace Sms.ApiClient.V2.StatisticsSummary
{
	public class StatisticsSummaryException : Exception
	{
		public StatisticsSummaryException(int errorCode) : base("Error code received when retrieving statistics summary: " + errorCode)
		{
		}
	}
}
