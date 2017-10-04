using System;
using System.Globalization;
using System.Net;
using System.Web;

namespace Sms.ApiClient.V2.StatisticsSummary
{
	internal class StatisticsSummaryClient : IStatisticsSummaryClient
	{
		private static readonly string CodeStamp = "Official StatisticsSummary Client " + ClientUtils.VersionNumber;

		private readonly string _apiKey;
		private readonly string _statisticsSummaryUrl;

		public StatisticsSummaryClient(string apiKey, string statisticsSummaryUrl)
		{
			_apiKey = apiKey;
			_statisticsSummaryUrl = statisticsSummaryUrl;
		}

		public StatisticsSummaryResponse GetStatistics(DateTime from, DateTime to)
		{
			var completeUrl = _statisticsSummaryUrl
								+ "?apiKey=" + HttpUtility.UrlEncode(_apiKey)
								+ "&client=" + CodeStamp
								+ "&from=" + from.ToString(new CultureInfo("da-DK"))
								+ "&to=" + to.ToString(new CultureInfo("da-DK"));
			var webClient = new WebClient();
			var responseString = webClient.DownloadString(completeUrl);
			int errorCode;
			if (int.TryParse(responseString, out errorCode))
				throw new StatisticsSummaryException(errorCode);

			return StatisticsSummaryResponse.Parse(xml: responseString);
		}
	}
}