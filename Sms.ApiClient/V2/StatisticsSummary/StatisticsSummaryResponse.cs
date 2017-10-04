using System;
using System.Globalization;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Sms.ApiClient.V2.StatisticsSummary
{
	public class StatisticsSummaryResponse
	{
		public DateTime From { get; set; }
		public DateTime To { get; set; }
		public MessagesInfo Messages { get; set; }

		public static StatisticsSummaryResponse Parse(string xml)
		{
			var doc = XDocument.Parse(xml);
			var result = new StatisticsSummaryResponse();
			var summaryElement = doc.Root.XPathSelectElement("./summary");
			result.From = DateTime.Parse(summaryElement.Attribute(XName.Get("from")).Value, new CultureInfo("da-DK"));
			result.To = DateTime.Parse(summaryElement.Attribute(XName.Get("to")).Value, new CultureInfo("da-DK"));

			var messageElement = summaryElement.XPathSelectElement("./messages");
			result.Messages = new MessagesInfo();
			result.Messages.TotalMessageCount = int.Parse(messageElement.Attribute(XName.Get("messagecount")).Value);
			result.Messages.TotalSmsCount = int.Parse(messageElement.Attribute(XName.Get("smscount")).Value);
			foreach (var statusElement in messageElement.XPathSelectElements("./status"))
			{
				var status = new StatusLine();
				status.MessageCount = int.Parse(statusElement.Attribute(XName.Get("messagecount")).Value);
				status.SmsCount = int.Parse(statusElement.Attribute(XName.Get("smscount")).Value);
				status.StatusCode = int.Parse(statusElement.Attribute(XName.Get("code")).Value);
				result.Messages.Statuses.Add(status);
			}
			return result;
		}
	}
}