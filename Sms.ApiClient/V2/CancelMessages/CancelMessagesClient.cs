using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Sms.ApiClient.V2.GetMessageStatuses;

namespace Sms.ApiClient.V2.CancelMessages
{
	public class CancelMessagesClient : ICancelMessagesClient
	{
		private const string CodeStamp = "Official CancelMessages Client " + ClientUtils.VersionNumber;

		private readonly string _apiKey;
		private readonly string _getUrl;

		public CancelMessagesClient(string apiKey, string getUrl)
		{
			_apiKey = apiKey;
			_getUrl = getUrl;
		}

		public CancelMessagesResponse CancelMessage(List<string> messageIds)
		{
			using (var webClient = new WebClient())
			{
				var finalUrl = _getUrl
				               + "?apiKey=" + HttpUtility.UrlEncode(_apiKey)
				               + "&client=" + CodeStamp
				               + "&messageids=" + string.Join(",", messageIds.ToArray());

				var responseString = webClient.DownloadString(finalUrl);
				return CancelMessagesResponse.ParseResponse(cancelResponseString: responseString);
			}
		}
	}
}
