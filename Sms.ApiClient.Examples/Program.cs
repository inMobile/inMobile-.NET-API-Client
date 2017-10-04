using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Sms.ApiClient.V2;
using Sms.ApiClient.V2.SendMessages;

namespace Sms.ApiClient.Examples
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());
		}
	}
}
