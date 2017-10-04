using Sms.ApiClient.V2;
using Sms.ApiClient.V2.GetMessageStatuses;
using Sms.ApiClient.V2.SendMessages;
using Sms.ApiClient.V2.StatisticsSummary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sms.ApiClient.Examples
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void btnSendMessage_Click(object sender, EventArgs e)
		{
			// Send a standard sms message
			SendStandardMessage();

			// For an example of sending an overcharged message, uncomment the line below
			// SendOverchargedMessage();
		}

		private IFacadeSmsClient CreateSmsClient()
		{
			var client = new FacadeSmsClient(hostRootUrl: txtApiUrl.Text, apiKey: txtApiKey.Text);
			return client;
		}

		private void SendStandardMessage()
		{
			// The api key can be found in the API documentation section
			var apiKey = txtApiKey.Text;

			// The callback url can be specified as NULL if the message status should not be pushed back to
			// a specific URL. Go to the API documentation for further details about the options available
			// regarding getting message statuses
			var statusCallbacKurl = txtMessageStatusCallbackUrl.Text;

			var smsClient = CreateSmsClient();

			var messagesToSend = new List<ISmsMessage>();
			var message = new SmsMessage(msisdn: txtMsisdn.Text,
										text: "This is a text message sent from the demo app.",
										senderName: "TestApp",
										encoding: SmsEncoding.Gsm7);
			messagesToSend.Add(message);
			try
			{
				var response = smsClient.SendMessages(messagesToSend, txtMessageStatusCallbackUrl.Text);
				MessageBox.Show(Format(response));

				// NOTE: The client supports sending multiple messages at once, using a single HTTP call
				// to ensure best possible performance for bulk sendings.
				// It is adviced to keep the message count at a maximum of 10.000 in a single call though
			}
			catch (Exception ex)
			{
				MessageBox.Show("ERROR: " + ex.Message);
			}
		}

		private void SendOverchargedMessage()
		{
			ISendMessagesClient smsClient = CreateSmsClient();

			var messagesToSend = new List<ISmsMessage>();
			var message = new SmsMessage(msisdn: txtMsisdn.Text, text: "This is an overcharged text message sent from the demo app.", senderName: "TestApp", encoding: SmsEncoding.Gsm7);

			// Specify 1,50 DKK overcharged message
			message.OverchargeInfo = new OverchargeInfo(overchargePrice: 150, // 150 øre
														shortCodeCountryCode: "45",
														shortCodeNumber: "1245",
														overchargeType: OverchargeType.MobilePayment,
														invoiceDescription: "");

			messagesToSend.Add(message);
			try
			{
				var response = smsClient.SendMessages(messagesToSend, txtMessageStatusCallbackUrl.Text);
				MessageBox.Show(Format(response));

				// NOTE: The client supports sending multiple messages at once, using a single HTTP call
				// to ensure best possible performance for bulk sendings.
				// It is adviced to keep the message count at a maximum of 10.000 in a single call though
			}
			catch (Exception ex)
			{
				MessageBox.Show("ERROR: " + ex.Message);
			}
		}

		private void btnGetMessageStatus_Click(object sender, EventArgs e)
		{
			IGetMessageStatusClient smsClient = CreateSmsClient();
			try
			{
				var response = smsClient.GetMessageStatus();
				MessageBox.Show(Format(response));
			}
			catch (Exception ex)
			{
				MessageBox.Show("ERROR: " + ex.Message);
			}
		}

		private void btnStatisticsSummary_Click(object sender, EventArgs e)
		{
			var today = DateTime.Now.Date.AddDays(1);
			var oneMonthAgo = today.AddMonths(-1);
			IStatisticsSummaryClient smsClient = CreateSmsClient();
			try
			{
				var response = smsClient.GetStatistics(@from: oneMonthAgo, to: today);
				MessageBox.Show(Format(response));
			}
			catch (Exception ex)
			{
				MessageBox.Show("ERROR: " + ex.Message);
			}
		}

		private string Format(SendMessagesResponse sendMessagesResponse)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("MESSAGE IDS RETURNED:");
			sb.Append(Environment.NewLine);
			sb.Append(string.Join(Environment.NewLine, sendMessagesResponse.MessageIds.Select(mid => "Msisdn: " + mid.Msisdn + ", message id: " + mid.MessageId)));
			return sb.ToString();
		}

		private string Format(GetMessageStatusesResponse response)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("MESSAGE STATUSES:");
			sb.Append(Environment.NewLine);
			if (response.MessageStatuses.Any())
				sb.Append(string.Join(Environment.NewLine, response.MessageStatuses.Select(ms => "message id: " + ms.MessageId + ", status code: " + ms.StatusCode + ", status description: " + ms.StatusDescription)));
			else
				sb.Append("No new message statuses available");
			return sb.ToString();
		}

		private string Format(StatisticsSummaryResponse response)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("TOTAL:");
			sb.Append(Environment.NewLine);
			sb.Append("Total message count: " + response.Messages.TotalMessageCount + ", total sms count: " + response.Messages.TotalSmsCount);
			if (response.Messages.Statuses.Any())
			{
				sb.Append(Environment.NewLine);
				sb.Append(Environment.NewLine);
				sb.Append("PER MESSAGE STATUS:");
				foreach (var line in response.Messages.Statuses)
				{
					sb.Append(Environment.NewLine);
					sb.Append("MessageStatus: " + line.StatusCode + ", message count: " + line.MessageCount + ", sms count: " + line.SmsCount);
				}
			}
			return sb.ToString();
		}
	}
}