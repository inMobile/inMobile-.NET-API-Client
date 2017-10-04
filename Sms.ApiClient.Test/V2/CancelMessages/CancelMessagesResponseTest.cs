using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sms.ApiClient.V2.CancelMessages;

namespace Sms.ApiClient.Test.V2.CancelMessages
{
	[TestClass]
	public class CancelMessagesResponseTest
	{
		[TestMethod]
		public void ParseResponse_Success_Test()
		{
			var resp = CancelMessagesResponse.ParseResponse("success: 23 cancelled.");
			Assert.IsNotNull(resp);
			Assert.AreEqual(23, resp.CancelCount);
		}

		[TestMethod]
		public void ParseResponse_Error_Test()
		{
			try
			{
				CancelMessagesResponse.ParseResponse("Error: Invalid unittest error");
				Assert.Fail("Expected CancelMessagesException");
			}
			catch (CancelMessagesException ex)
			{
				
			}
		}
		
		[TestMethod]
		public void ParseResponse_UnexpectedException_Test()
		{
			try
			{
				CancelMessagesResponse.ParseResponse("success: INVALID_COUNT cancelled.");
			}
			catch (CancelMessagesException ex)
			{
				Assert.AreEqual("Unexpected error cancelling message(s). See inner exception for details.", ex.Message);
				Assert.IsNotNull(ex.InnerException);
				Assert.IsInstanceOfType(ex.InnerException, typeof(FormatException));
			}
		}
	}
}
