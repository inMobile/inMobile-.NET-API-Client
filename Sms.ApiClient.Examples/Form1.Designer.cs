namespace Sms.ApiClient.Examples
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnSendMessage = new System.Windows.Forms.Button();
			this.btnGetMessageStatus = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.txtApiKey = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtMsisdn = new System.Windows.Forms.TextBox();
			this.btnStatisticsSummary = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.txtMessageStatusCallbackUrl = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtApiUrl = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnSendMessage
			// 
			this.btnSendMessage.Location = new System.Drawing.Point(83, 115);
			this.btnSendMessage.Name = "btnSendMessage";
			this.btnSendMessage.Size = new System.Drawing.Size(113, 23);
			this.btnSendMessage.TabIndex = 0;
			this.btnSendMessage.Text = "Send messages";
			this.btnSendMessage.UseVisualStyleBackColor = true;
			this.btnSendMessage.Click += new System.EventHandler(this.btnSendMessage_Click);
			// 
			// btnGetMessageStatus
			// 
			this.btnGetMessageStatus.Location = new System.Drawing.Point(202, 115);
			this.btnGetMessageStatus.Name = "btnGetMessageStatus";
			this.btnGetMessageStatus.Size = new System.Drawing.Size(113, 23);
			this.btnGetMessageStatus.TabIndex = 1;
			this.btnGetMessageStatus.Text = "Get message status";
			this.btnGetMessageStatus.UseVisualStyleBackColor = true;
			this.btnGetMessageStatus.Click += new System.EventHandler(this.btnGetMessageStatus_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 35);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(45, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Api-key:";
			// 
			// txtApiKey
			// 
			this.txtApiKey.Location = new System.Drawing.Point(138, 33);
			this.txtApiKey.Name = "txtApiKey";
			this.txtApiKey.Size = new System.Drawing.Size(237, 20);
			this.txtApiKey.TabIndex = 3;
			this.txtApiKey.Text = "Insert your secret api key here";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(11, 91);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(67, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Your msisdn:";
			// 
			// txtMsisdn
			// 
			this.txtMsisdn.Location = new System.Drawing.Point(137, 88);
			this.txtMsisdn.Name = "txtMsisdn";
			this.txtMsisdn.Size = new System.Drawing.Size(238, 20);
			this.txtMsisdn.TabIndex = 5;
			this.txtMsisdn.Text = "Type msisdn here, e.g. 4512345678";
			// 
			// btnStatisticsSummary
			// 
			this.btnStatisticsSummary.Location = new System.Drawing.Point(321, 115);
			this.btnStatisticsSummary.Name = "btnStatisticsSummary";
			this.btnStatisticsSummary.Size = new System.Drawing.Size(140, 23);
			this.btnStatisticsSummary.TabIndex = 6;
			this.btnStatisticsSummary.Text = "Get statistics summary";
			this.btnStatisticsSummary.UseVisualStyleBackColor = true;
			this.btnStatisticsSummary.Click += new System.EventHandler(this.btnStatisticsSummary_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 64);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(120, 13);
			this.label3.TabIndex = 7;
			this.label3.Text = "Your status callback url:";
			// 
			// txtMessageStatusCallbackUrl
			// 
			this.txtMessageStatusCallbackUrl.Location = new System.Drawing.Point(137, 61);
			this.txtMessageStatusCallbackUrl.Name = "txtMessageStatusCallbackUrl";
			this.txtMessageStatusCallbackUrl.Size = new System.Drawing.Size(238, 20);
			this.txtMessageStatusCallbackUrl.TabIndex = 8;
			this.txtMessageStatusCallbackUrl.Text = "http://yourdomain.dk/example/messagestatus";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 9);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(62, 13);
			this.label4.TabIndex = 9;
			this.label4.Text = "Server-root:";
			// 
			// txtApiUrl
			// 
			this.txtApiUrl.Location = new System.Drawing.Point(137, 7);
			this.txtApiUrl.Name = "txtApiUrl";
			this.txtApiUrl.Size = new System.Drawing.Size(238, 20);
			this.txtApiUrl.TabIndex = 10;
			this.txtApiUrl.Text = "Type the root address for the server";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(381, 64);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(153, 13);
			this.label5.TabIndex = 11;
			this.label5.Text = "( Leave empty for no callback )";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(381, 10);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(251, 13);
			this.label6.TabIndex = 12;
			this.label6.Text = "( The value can be seen in our API documentation )";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(381, 91);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(315, 13);
			this.label7.TabIndex = 13;
			this.label7.Text = "( The msisdn is the mobile phone number including country code )";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(381, 35);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(251, 13);
			this.label8.TabIndex = 14;
			this.label8.Text = "( The value can be seen in our API documentation )";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(709, 170);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.txtApiUrl);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtMessageStatusCallbackUrl);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.btnStatisticsSummary);
			this.Controls.Add(this.txtMsisdn);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtApiKey);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnGetMessageStatus);
			this.Controls.Add(this.btnSendMessage);
			this.Name = "Form1";
			this.Text = "Sms Api Demo";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnSendMessage;
		private System.Windows.Forms.Button btnGetMessageStatus;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtApiKey;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtMsisdn;
		private System.Windows.Forms.Button btnStatisticsSummary;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtMessageStatusCallbackUrl;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtApiUrl;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
	}
}

