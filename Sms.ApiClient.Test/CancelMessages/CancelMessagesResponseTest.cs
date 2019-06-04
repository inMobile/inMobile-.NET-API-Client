using Sms.ApiClient.V2.CancelMessages;
using System;
using Xunit;

namespace Sms.ApiClient.Test.V2.CancelMessages
{
    public class CancelMessagesResponseTest
	{
		[Fact]
		public void ParseResponse_Success_Test()
		{
			var resp = CancelMessagesResponse.ParseResponse("success: 23 cancelled.");
			Assert.NotNull(resp);
			Assert.Equal(23, resp.CancelCount);
		}

        [Fact]
        public void ParseResponse_Error_Test()
        {
            var ex = Assert.Throws<CancelMessagesException>(() =>
            {
                CancelMessagesResponse.ParseResponse("Error: Invalid unittest error");
            });
        }

        [Fact]
		public void ParseResponse_UnexpectedException_Test()
		{
            var ex = Assert.Throws<CancelMessagesException>(() =>
            {
                CancelMessagesResponse.ParseResponse("success: INVALID_COUNT cancelled.");
            });

            Assert.Equal("Unexpected error cancelling message(s). See inner exception for details.", ex.Message);
            Assert.NotNull(ex.InnerException);
            Assert.IsType<FormatException>(ex.InnerException);
		}
	}
}