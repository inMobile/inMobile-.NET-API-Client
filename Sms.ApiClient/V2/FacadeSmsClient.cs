using Sms.ApiClient.V2.CancelMessages;
using Sms.ApiClient.V2.GetMessageStatuses;
using Sms.ApiClient.V2.SendMessages;
using Sms.ApiClient.V2.StatisticsSummary;
using System;
using System.Collections.Generic;

namespace Sms.ApiClient.V2
{
    public interface IFacadeSmsClient
    {
        GetMessageStatusesResponse GetMessageStatus();
        SendMessagesResponse SendMessages(List<ISmsMessage> messages, string messageStatusCallbackUrl = null);
        StatisticsSummaryResponse GetStatistics(DateTime @from, DateTime to);
        CancelMessagesResponse CancelMessage(List<string> messageIds);
    }

    public class FacadeSmsClient : IFacadeSmsClient
    {
		private readonly ISendMessagesClient _sendClient;
		private readonly IGetMessageStatusClient _statusClient;
		private readonly IStatisticsSummaryClient _statisticsClient;
		private readonly ICancelMessagesClient _cancelMessagesClient;

		public FacadeSmsClient(string hostRootUrl, string apiKey)
		{
			if (string.IsNullOrEmpty(hostRootUrl))
				throw new ArgumentNullException("hostRootUrl");
			if (string.IsNullOrEmpty(apiKey))
				throw new ArgumentNullException("apiKey");

			// Ensure url not ending with / to avoid double slashes in concatenation
			if (hostRootUrl.EndsWith("/"))
				hostRootUrl = hostRootUrl.Substring(0, hostRootUrl.Length - 1);

			_statisticsClient = new StatisticsSummaryClient(apiKey: apiKey, statisticsSummaryUrl: hostRootUrl + "/Api/V2/Statistics/Summary");
			_statusClient = new GetMessageStatusClient(apiKey: apiKey, getMessagesGetUrl: hostRootUrl + "/api/v2/GET/getmessagestatus");
			_sendClient = new SendMessagesClient(apiKey: apiKey, postUrl: hostRootUrl + "/api/v2/sendmessages", requestBuilder: new SendMessagesRequestBuilder());
			_cancelMessagesClient = new CancelMessagesClient(apiKey: apiKey, getUrl: hostRootUrl + "/Api/V2/Get/CancelMessages");
		}

		public GetMessageStatusesResponse GetMessageStatus()
		{
			return _statusClient.GetMessageStatus();
		}

		public SendMessagesResponse SendMessages(List<ISmsMessage> messages, string messageStatusCallbackUrl = null)
		{
			return _sendClient.SendMessages(messages: messages, messageStatusCallbackUrl: messageStatusCallbackUrl);
		}

		public StatisticsSummaryResponse GetStatistics(DateTime @from, DateTime to)
		{
			return _statisticsClient.GetStatistics(from: from, to: to);
		}

		public CancelMessagesResponse CancelMessage(List<string> messageIds)
		{
			return _cancelMessagesClient.CancelMessage(messageIds: messageIds);
		}
	}
}