using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using Xunit;

namespace InMobile.Sms.ApiClient.Test.Tools
{
    public class UnitTestHttpServer : IDisposable
    {
        public int Port { get; }
        public IPAddress Address { get; }
        public TcpListener _tcpListener;
        public string ReceivedInput { get; private set; }

        private List<IDisposable> Disposables = new List<IDisposable>();
        private UnitTestHttpServer(IPAddress address, int port, string expectedRequest, string responseToSendBack)
        {
            Address = address ?? throw new ArgumentNullException(nameof(address));
            Port = port;
            _expectedRequest = expectedRequest ?? throw new ArgumentNullException(nameof(expectedRequest));
            _responseToSendBack = responseToSendBack ?? throw new ArgumentNullException(nameof(responseToSendBack));
        }

        private void StartListening()
        {
            if (_tcpListener != null)
                throw new Exception("Already listening");
            _tcpListener = new TcpListener(localaddr: Address, port: Port);
            _tcpListener.Start();
            _tcpListener.BeginAcceptSocket(DoAcceptSocketCallback, _tcpListener);
        }

        public void Dispose()
        {
            try
            {
                _tcpListener?.Stop();
            }
            catch { }

            foreach (var d in Disposables)
            {
                try
                {
                    d?.Dispose();
                }
                catch { }
            }

            // Check if server did not receive expected http data
            Assert.Equal(_expectedRequest, ReceivedInput);
        }

        // Process the client connection.
        public void DoAcceptSocketCallback(IAsyncResult ar)
        {
            // End the operation and display the received data on
            // the console.
            var socket = _tcpListener.EndAcceptSocket(ar);

            // Stop listener after first connect
            _tcpListener.Stop();

            // Read input
            var buffer = new byte[10000];
            var receivedByteCount = socket.Receive(buffer: buffer);
            ReceivedInput = Encoding.ASCII.GetString(bytes: buffer, index: 0, count: receivedByteCount);

            // Send back reply
            socket.NoDelay = true; // This will disable the Nagle algorthm and hereby prevent buffering and instead sending right away. https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.socket.nodelay?view=net-5.0
            socket.Send(Encoding.ASCII.GetBytes(_responseToSendBack));
            socket.Close();
        }

        public static UnitTestHttpServer StartOnAnyAvailablePort(string expectedRequest, string responseToSendBack)
        {
            var server = new UnitTestHttpServer(address: IPAddress.Loopback, port: GetAvailablePort(), expectedRequest: expectedRequest, responseToSendBack: responseToSendBack);
            server.StartListening();
            return server;
        }

        private static object _syncLock = new object();
        private readonly string _expectedRequest;
        private readonly string _responseToSendBack;

        private static int GetAvailablePort()
        {
            lock (_syncLock)
            {
                // Gather info about existing ports
                var portList = new List<int>();
                var properties = IPGlobalProperties.GetIPGlobalProperties();
                portList.AddRange(properties.GetActiveTcpConnections().Select(c => c.LocalEndPoint.Port));
                portList.AddRange(properties.GetActiveTcpListeners().Select(c => c.Port));
                portList.AddRange(properties.GetActiveUdpListeners().Select(c => c.Port));
                if (!portList.Any())
                    return 2000;

                return Math.Max(portList.Max(), 2000);
            }
        }

        public class UnexpectedRequestDataException : Exception
        {
            public UnexpectedRequestDataException(string msg) : base(msg) { }
        }

    }
}
