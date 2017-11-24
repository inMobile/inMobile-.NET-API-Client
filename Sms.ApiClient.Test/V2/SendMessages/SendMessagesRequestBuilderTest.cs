using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sms.ApiClient.V2.SendMessages;
using System;
using System.Collections.Generic;

namespace Sms.ApiClient.Test.V2.SendMessages
{
    [TestClass]
    public class SendMessagesRequestBuilderTest
    {
        [TestMethod]
        public void Build_Simple_NoMessageId_NoUrlCallback_Test()
        {
            var builder = new SendMessagesRequestBuilder();

            var result = builder.BuildPostXmlData(new List<ISmsMessage>()
                                    {
                                        new SmsMessage(msisdn: "4511111111",
                                                        text: "Text msg 1",
                                                        senderName: "SenderA",
                                                        encoding: SmsEncoding.Utf8)
                                    },
                                    "apiKey",
                                    messageStatusCallbackUrl: null);

            var expectedOutput =
@"<request source=""Official SendMessages Client 2.2.9.1"">
  <authentication apikey=""apiKey"" />
  <data>
    <message>
      <sendername>SenderA</sendername>
      <text encoding=""utf-8"" flash=""false"">Text msg 1</text>
      <recipients>
        <msisdn>4511111111</msisdn>
      </recipients>
    </message>
  </data>
</request>";

            AssertEqualXml(expectedOutput, result);
        }

        [TestMethod]
        public void Build_Simple_WithRespectBlacklistingToFalse_Test()
        {
            var builder = new SendMessagesRequestBuilder();

            var result = builder.BuildPostXmlData(new List<ISmsMessage>()
                {
                    new SmsMessage(msisdn: "4511111111",
                        text: "Text msg 1",
                        senderName: "SenderA",
                        encoding: SmsEncoding.Utf8,
                        respectBlacklist: false)
                },
                "apiKey",
                messageStatusCallbackUrl: null);

            var expectedOutput =
                @"<request source=""Official SendMessages Client 2.2.9.1"">
  <authentication apikey=""apiKey"" />
  <data>
    <message>
      <sendername>SenderA</sendername>
      <text encoding=""utf-8"" flash=""false"">Text msg 1</text>
      <recipients>
        <msisdn>4511111111</msisdn>
      </recipients>
      <respectblacklist>false</respectblacklist>
    </message>
  </data>
</request>";

            AssertEqualXml(expectedOutput, result);
        }

        [TestMethod]
        public void Build_Simple_WithMessageId_Callback_SendTime_Test()
        {
            var builder = new SendMessagesRequestBuilder();

            var result = builder.BuildPostXmlData(new List<ISmsMessage>()
                                    {
                                        new SmsMessage(messageId: "MessageId123",
                                                         msisdn: "4511111111",
                                                        text: "Text msg 1",
                                                        senderName: "SenderA",
                                                        encoding: SmsEncoding.Utf8,
                                                        sendTime: new DateTime(2001,02,03,04,05,06))
                                    },
                                    "apiKey",
                                    "http://someUrl.callback.inmobile.dk/unittest/messagestatus");

            var expectedOutput =
@"<request source=""Official SendMessages Client 2.2.9.1"">
  <authentication apikey=""apiKey"" />
  <data>
    <statuscallbackurl>http://someUrl.callback.inmobile.dk/unittest/messagestatus</statuscallbackurl>
    <message>
      <sendername>SenderA</sendername>
      <text encoding=""utf-8"" flash=""false"">Text msg 1</text>
      <recipients>
        <msisdn id=""MessageId123"">4511111111</msisdn>
      </recipients>
      <sendtime>2001-02-03 04:05:06</sendtime>
    </message>
  </data>
</request>";

            AssertEqualXml(expectedOutput, result);
        }

        [TestMethod]
        public void Build_Overcharged_Test()
        {
            var builder = new SendMessagesRequestBuilder();

            var message = new SmsMessage(messageId: "MessageId123",
                msisdn: "4511111111",
                text: "Text msg 1",
                senderName: "SenderA",
                encoding: SmsEncoding.Utf8);
            message.OverchargeInfo = new OverchargeInfo(overchargeType: OverchargeType.MobilePayment, overchargePrice: 2500, shortCodeCountryCode: "45", shortCodeNumber: "1245", invoiceDescription: "Some description");
            var result = builder.BuildPostXmlData(new List<ISmsMessage>() { message }, "apiKey", "http://someUrl.callback.inmobile.dk/unittest/messagestatus");

            var expectedOutput =
@"<request source=""Official SendMessages Client 2.2.9.1"">
  <authentication apikey=""apiKey"" />
  <data>
    <statuscallbackurl>http://someUrl.callback.inmobile.dk/unittest/messagestatus</statuscallbackurl>
    <message>
      <sendername>SenderA</sendername>
      <text encoding=""utf-8"" flash=""false"">Text msg 1</text>
      <recipients>
        <msisdn id=""MessageId123"">4511111111</msisdn>
      </recipients>
      <overchargeinfo price=""2500"" type=""3"" countrycode=""45"" shortcode=""1245"" invoicedescription=""Some description"" />
    </message>
  </data>
</request>";

            AssertEqualXml(expectedOutput, result);
        }

        [TestMethod]
        public void Build_RefundMessages_Test()
        {
            var builder = new SendMessagesRequestBuilder();

            var refund1 = new RefundMessage(messageIdToRefund: "RefundId1", messageText: "Refund & < > text 1", messageId: "MessageId123");
            var refund2 = new RefundMessage(messageIdToRefund: "RefundId2", messageText: "Refund text 2");

            var result = builder.BuildPostXmlData(new List<ISmsMessage>() { refund1, refund2 }, "apiKey", "http://someUrl.callback.inmobile.dk/unittest/messagestatus");

            var expectedOutput =
@"<request source=""Official SendMessages Client 2.2.9.1"">
  <authentication apikey=""apiKey"" />
  <data>
    <statuscallbackurl>http://someUrl.callback.inmobile.dk/unittest/messagestatus</statuscallbackurl>
    <refundmessage messageidtorefund=""RefundId1"" messageid=""MessageId123"">
      <text>Refund &amp; &lt; &gt; text 1</text>
    </refundmessage>
    <refundmessage messageidtorefund=""RefundId2"">
      <text>Refund text 2</text>
    </refundmessage>
  </data>
</request>";

            AssertEqualXml(expectedOutput, result);
        }

        [TestMethod]
        public void Build_MultipleMessages_Test()
        {
            var builder = new SendMessagesRequestBuilder();

            var result = builder.BuildPostXmlData(new List<ISmsMessage>()
                                    {
                                        new SmsMessage(messageId: "MessageId123",
                                                         msisdn: "4511111111",
                                                        text: "Text msg 1",
                                                        senderName: "SenderA",
                                                        encoding: SmsEncoding.Utf8,
                                                        sendTime: new DateTime(2001,02,03,04,05,06),
                                                        flash: false),
                                        new SmsMessage(msisdn: "4522222222",
                                                        text: "Text msg 2",
                                                        senderName: "SenderB",
                                                        encoding: SmsEncoding.Gsm7,
                                                        flash: true)
                                    },
                                    "apiKey",
                                    "http://someUrl.callback.inmobile.dk/unittest/messagestatus");

            var expectedOutput =
@"<request source=""Official SendMessages Client 2.2.9.1"">
  <authentication apikey=""apiKey"" />
  <data>
    <statuscallbackurl>http://someUrl.callback.inmobile.dk/unittest/messagestatus</statuscallbackurl>
    <message>
      <sendername>SenderA</sendername>
      <text encoding=""utf-8"" flash=""false"">Text msg 1</text>
      <recipients>
        <msisdn id=""MessageId123"">4511111111</msisdn>
      </recipients>
      <sendtime>2001-02-03 04:05:06</sendtime>
    </message>
    <message>
      <sendername>SenderB</sendername>
      <text encoding=""gsm7"" flash=""true"">Text msg 2</text>
      <recipients>
        <msisdn>4522222222</msisdn>
      </recipients>
    </message>
  </data>
</request>";

            AssertEqualXml(expectedOutput, result);
        }

        private void AssertEqualXml(string xml1, string xml2)
        {
            if (false == xml1.Equals(xml2))
            {
                var indexFirstDiff = GetFirstBreakIndex(xml1, xml2, true);
                if (indexFirstDiff < xml1.Length && indexFirstDiff < xml2.Length)
                    throw new Exception("Different from: " + xml2.Substring(indexFirstDiff) + "\n           AND           \n" + xml1.Substring(indexFirstDiff));
                else
                    Assert.Equals(xml1, xml2);
            }
        }

        /// <summary>
        /// Gets a first different char occurence index
        /// </summary>
        /// <param name="a">First string</param>
        /// <param name="b">Second string</param>
        /// <param name="handleLengthDifference">
        /// If true will return index of first occurence even strings are of different length
        /// and same-length parts are equals otherwise -1
        /// </param>
        /// <returns>
        /// Returns first difference index or -1 if no difference is found
        /// </returns>
        public int GetFirstBreakIndex(string a, string b, bool handleLengthDifference)
        {
            int equalsReturnCode = -1;
            if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b))
            {
                return handleLengthDifference ? 0 : equalsReturnCode;
            }

            string longest = b.Length > a.Length ? b : a;
            string shorten = b.Length > a.Length ? a : b;
            for (int i = 0; i < shorten.Length; i++)
            {
                if (shorten[i] != longest[i])
                {
                    return i;
                }
            }

            // Handles cases when length is different (a="1234", b="123")
            // index=3 would be returned for this case
            // If you do not need such behaviour - just remove this
            if (handleLengthDifference && a.Length != b.Length)
            {
                return shorten.Length;
            }

            return equalsReturnCode;
        }
    }
}