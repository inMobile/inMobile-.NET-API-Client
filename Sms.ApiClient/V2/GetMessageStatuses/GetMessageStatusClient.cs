using System;
using System.Collections.Generic;
using System.Net;
using System.Web;

namespace Sms.ApiClient.V2.GetMessageStatuses
{
	public class GetMessageStatusClient : IGetMessageStatusClient
	{
		private static readonly string CodeStamp = "Official GetMessageStatus Client " + ClientUtils.VersionNumber;

		private readonly string _completeUrl;
		public GetMessageStatusClient(string apiKey, string getMessagesGetUrl)
		{
			_completeUrl = getMessagesGetUrl + "?apiKey=" + HttpUtility.UrlEncode(apiKey) + "&client=" + CodeStamp;
		}

		public GetMessageStatusesResponse GetMessageStatus()
		{
			using (var webClient = new WebClient())
			{
				var response = webClient.DownloadString(_completeUrl);
				if (string.IsNullOrEmpty(response))
					return new GetMessageStatusesResponse(new List<MessageStatus>());

				if (response.StartsWith("Error:"))
					throw new GetMessageStatusesException(response);
				var lines = response.Split(new string[] {"\n"}, StringSplitOptions.RemoveEmptyEntries);
				var statuses = new List<MessageStatus>();
				foreach (var line in lines)
				{
					var chunks = line.Split(':');
					statuses.Add(new MessageStatus(messageId: chunks[0], statusCode: int.Parse(chunks[1]), statusDescription: chunks[2]));
				}
				return new GetMessageStatusesResponse(statuses);
			}
		}
	}
}
