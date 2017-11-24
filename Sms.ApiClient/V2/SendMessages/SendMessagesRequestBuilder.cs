using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Sms.ApiClient.V2.SendMessages
{
    public interface ISendMessagesRequestBuilder
    {
        string BuildPostXmlData(List<ISmsMessage> messages, string apiKey, string messageStatusCallbackUrl);
    }

    public class SendMessagesRequestBuilder : ISendMessagesRequestBuilder
    {
        public string BuildPostXmlData(List<ISmsMessage> messages, string apiKey, string messageStatusCallbackUrl)
        {
            // Build main element
            var xml = @"<request source=""" + SendMessagesClient.CodeStamp + @""">
							<authentication apikey=""[UNSET]"" />
							<data>
							</data>
						</request>";
            var doc = XDocument.Parse(xml);

            // Insert apikey
            doc.Root.XPathSelectElement("./authentication").SetAttributeValue("apikey", apiKey);

            // Insert callback url element
            bool nonEmptyUrl = false == string.IsNullOrEmpty(messageStatusCallbackUrl);
            if (nonEmptyUrl)
            {
                var callbackElement = new XElement(XName.Get("statuscallbackurl"));
                callbackElement.Value = messageStatusCallbackUrl;
                doc.Root.XPathSelectElement("./data").Add(callbackElement);
            }

            // Construct message-element for each message group and add to final xml element
            var dataElement = doc.Root.XPathSelectElement("./data");

            // Standard and overcharged messages
            {
                var standardAndOverchargeMessages = messages.OfType<SmsMessage>().ToList();
                var elements = BuildSmsElements(standardAndOverchargeMessages);
                foreach (var e in elements)
                    dataElement.Add(e);
            }

            // Refundings
            {
                var refundMessages = messages.OfType<RefundMessage>().ToList();
                var refundElements = BuildRefundElements(refundMessages);
                foreach (var e in refundElements)
                    dataElement.Add(e);
            }

            return doc.ToString();
        }

        private List<XElement> BuildRefundElements(List<RefundMessage> refundMessages)
        {
            var refundElements = new List<XElement>();

            foreach (var refund in refundMessages)
            {
                const string recXmlString = @"<refundmessage>
												<text>[UNSET]</text>
											</refundmessage>";
                var refundElement = XElement.Parse(recXmlString);
                refundElement.XPathSelectElement("./text").Value = refund.MessageText;
                refundElement.XPathSelectElement(".").SetAttributeValue(XName.Get("messageidtorefund"), refund.MessageIdToRefund);
                if (false == string.IsNullOrEmpty(refund.MessageId))
                {
                    refundElement.XPathSelectElement(".").SetAttributeValue(XName.Get("messageid"), refund.MessageId);
                }

                refundElements.Add(refundElement);
            }

            return refundElements;
        }

        private List<XElement> BuildSmsElements(List<SmsMessage> messages)
        {
            var smsElements = new List<XElement>();

            // Group messages by senderName and text to compress final xml when multiple messages have
            // equal senderName and text
            var grouped = messages.GroupBy(m => GetGroupKey(m));

            foreach (var group in grouped)
            {
                // Get senderName, text and text encoding from the first message in the group
                // These values are equal for all messages within the same group
                var firstMessage = group.First();
                string senderName = firstMessage.SenderName;
                string text = firstMessage.Text;
                string encodingString = "gsm7";
                if (firstMessage.Encoding == SmsEncoding.Utf8)
                    encodingString = "utf-8";
                if (firstMessage.Encoding == SmsEncoding.Gsm7Extended)
                    encodingString = "gsm7extended";

                const string recXmlString = @"<message>
												<sendername>[UNSET]</sendername>
												<text>[UNSET]</text>
												<recipients>
												</recipients>
											</message>";
                var messageElement = XElement.Parse(recXmlString);
                messageElement.XPathSelectElement("./sendername").Value = senderName;
                messageElement.XPathSelectElement("./text").Value = text;
                messageElement.XPathSelectElement("./text").SetAttributeValue("encoding", encodingString);
                messageElement.XPathSelectElement("./text").SetAttributeValue("flash", firstMessage.Flash.ToString().ToLower());

                // Add msisdns
                foreach (var message in group)
                {
                    var msisdnElement = XElement.Parse("<msisdn></msisdn>");
                    msisdnElement.Value = message.Msisdn;
                    if (!string.IsNullOrEmpty(message.MessageId))
                        msisdnElement.SetAttributeValue("id", message.MessageId);
                    messageElement.XPathSelectElement("./recipients").Add(msisdnElement);
                }

                // Set sendtime if specified
                if (firstMessage.SendTime != null)
                {
                    var sendTimeElement = new XElement(XName.Get("sendtime"));
                    sendTimeElement.Value = firstMessage.SendTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    messageElement.Add(sendTimeElement);
                }

                // Set expireinseconds if specified
                if (firstMessage.ExpireIn.HasValue)
                {
                    var expireInSecondsElement = new XElement(XName.Get("expireinseconds"));
                    expireInSecondsElement.Value = Convert.ToInt32(firstMessage.ExpireIn.Value.TotalSeconds).ToString();
                    messageElement.Add(expireInSecondsElement);
                }

                // Set respectBlacklisting if specified
                if (firstMessage.RespectBlacklist.HasValue)
                {
                    var respectBlacklistingElement = new XElement(XName.Get("respectblacklist"));
                    respectBlacklistingElement.Value = firstMessage.RespectBlacklist.Value.ToString().ToLower();
                    messageElement.Add(respectBlacklistingElement);
                }

                // Handle overcharge
                if (firstMessage.OverchargeInfo != null)
                {
                    var overchargeElement = new XElement(XName.Get("overchargeinfo"));
                    overchargeElement.SetAttributeValue(XName.Get("price"), firstMessage.OverchargeInfo.Price);
                    overchargeElement.SetAttributeValue(XName.Get("type"), (int)firstMessage.OverchargeInfo.Type);
                    overchargeElement.SetAttributeValue(XName.Get("countrycode"), firstMessage.OverchargeInfo.CountryCode);
                    overchargeElement.SetAttributeValue(XName.Get("shortcode"), firstMessage.OverchargeInfo.ShortCodeNumber);
                    overchargeElement.SetAttributeValue(XName.Get("invoicedescription"), firstMessage.OverchargeInfo.InvoiceDescription);
                    messageElement.Add(overchargeElement);
                }

                smsElements.Add(messageElement);
            }

            return smsElements;
        }

        private string GetGroupKey(SmsMessage message)
        {
            var key = message.Encoding + "_" + message.SenderName + "_" + message.Text + "_" + message.SendTime;
            if (message.OverchargeInfo != null)
            {
                key += "_" + message.OverchargeInfo.CountryCode + "_" + message.OverchargeInfo.Price + "_" + message.OverchargeInfo.ShortCodeNumber + "_" + message.OverchargeInfo.Type + "_" + message.ExpireIn;
            }
            return key;
        }
    }
}