using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Sms.ApiClient.V2.SendMessages
{
	public class SendMessagesClient : ISendMessagesClient
	{
		public const string CodeStamp = "Official SendMessages Client " + ClientUtils.VersionNumber;

		private readonly string _apiKey;
		private readonly string _postUrl;
		private readonly ISendMessagesRequestBuilder _requestBuilder;

		public SendMessagesClient(string apiKey, string postUrl, ISendMessagesRequestBuilder requestBuilder)
		{
			_apiKey = apiKey;
			_postUrl = postUrl;
			_requestBuilder = requestBuilder;
		}

		public SendMessagesResponse SendMessages(List<ISmsMessage> messages, string messageStatusCallbackUrl)
		{
			var xml = _requestBuilder.BuildPostXmlData(messages: messages, apiKey:_apiKey, messageStatusCallbackUrl: messageStatusCallbackUrl);
			using (var client = new WebClient())
			{
				var responseBytes = client.UploadValues(_postUrl, new NameValueCollection()
				{
					{"xml", xml}
				});
				string responseString = Encoding.UTF8.GetString(responseBytes);
				int errorCode;
				if (int.TryParse(responseString, out errorCode))
					throw new SendMessageException(errorCode);

				var replyDoc = XDocument.Parse(responseString);

				// Parse response
				var replyElements = replyDoc.Root.XPathSelectElements("./recipient").ToList();
				var messageIds = new List<MsisdnAndMessageId>(replyElements.Count);
				foreach (var replyElement in replyElements)
				{
					messageIds.Add(new MsisdnAndMessageId(msisdn: replyElement.Attribute(XName.Get("msisdn")).Value, messageId: replyElement.Attribute(XName.Get("id")).Value));
				}
				return new SendMessagesResponse(messageIds);
			}
		}

	}

}
