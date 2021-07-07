using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Xunit;

namespace InMobile.Sms.ApiClient.Test
{
    public class UnitTestHttpServerTest
    {
        [Fact]
        public void UnitTestHttpServer_WithNativeTcpClient_Success_Test()
        {
            var expectedRequest = "expectedInput123";
            var responseToSendBack = "expectedReply123";
            using (var server = UnitTestHttpServer.StartOnAnyAvailablePort(expectedRequest: expectedRequest, responseToSendBack: responseToSendBack))
            {
                var client = new TcpClient();
                client.Connect(new IPEndPoint(server.Address, port: server.Port));
                using (var stream = client.GetStream())
                {
                    // Send request
                    var bytes = Encoding.ASCII.GetBytes(expectedRequest);
                    stream.Write(bytes);
                    stream.Flush();

                    // Receive response
                    var responseBytes = new byte[responseToSendBack.Length];
                    var byteCountRead = stream.Read(buffer: responseBytes, offset: 0, size: responseBytes.Length);
                    var responseString = Encoding.ASCII.GetString(responseBytes);

                    Assert.Equal(responseString, responseToSendBack);
                }
            }
        }

        [Fact]
        public void UnitTestHttpServer_WithNativeTcpClient_UnexpectedDataReceivedByServer_ExpectExceptionWhenServerIsDisposed_Test()
        {
            var expectedRequest = "expectedInput123";
            var responseToSendBack = "expectedReply123";
            var server = UnitTestHttpServer.StartOnAnyAvailablePort(expectedRequest: expectedRequest, responseToSendBack: responseToSendBack);

            var client = new TcpClient();
            client.Connect(new IPEndPoint(server.Address, port: server.Port));
            using (var stream = client.GetStream())
            {
                // Send request
                var bytes = Encoding.ASCII.GetBytes("Some unexpected request data");
                stream.Write(bytes);
                stream.Flush();

                // Receive response
                var responseBytes = new byte[responseToSendBack.Length];
                var byteCountRead = stream.Read(buffer: responseBytes, offset: 0, size: responseBytes.Length);
                var responseString = Encoding.ASCII.GetString(responseBytes);

                Assert.Equal(responseString, responseToSendBack);
            }

            // Ensure disposing here will throw assertion error
            var ex = Assert.Throws<Xunit.Sdk.EqualException>(() => server.Dispose());
        }
    }
}
