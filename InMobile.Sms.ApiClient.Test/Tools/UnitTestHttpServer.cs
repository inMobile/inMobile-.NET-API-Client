using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using Xunit;

namespace InMobile.Sms.ApiClient.Test
{
    public class UnitTestHttpServer : IDisposable
    {
        public IPEndPoint EndPoint { get; private set; }
        public TcpListener _tcpListener;
        public string ReceivedInput { get; private set; }

        private List<IDisposable> Disposables = new List<IDisposable>();
        private UnitTestHttpServer(IPEndPoint endPoint, string expectedRequest, string responseToSendBack)
        {
            EndPoint = endPoint ?? throw new ArgumentNullException(nameof(endPoint));
            _expectedRequest = expectedRequest ?? throw new ArgumentNullException(nameof(expectedRequest));
            _responseToSendBack = responseToSendBack ?? throw new ArgumentNullException(nameof(responseToSendBack));
        }

        private void StartListening()
        {
            if (_tcpListener != null)
                throw new Exception("Already listening");
            _tcpListener = new TcpListener(localaddr: EndPoint.Address, port: EndPoint.Port);
            _tcpListener.Start();
            _tcpListener.BeginAcceptSocket(DoAcceptSocketCallback, _tcpListener);
        }

        public void Dispose()
        {
            try
            {
                _tcpListener?.Stop();
            }
            catch(Exception ex) {
                ex.ToString();
            }

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

        private static object _syncLock = new object();
        public static UnitTestHttpServer StartOnAnyAvailablePort(string expectedRequest, string responseToSendBack)
        {
            lock (_syncLock) // Ensures no race conditions ending up having multiple test server listening on the same port at the same time
            {
                var endPoint = new IPEndPoint(address: IPAddress.Loopback, port: GetAvailablePort());
                var server = new UnitTestHttpServer(endPoint: endPoint, expectedRequest: expectedRequest, responseToSendBack: responseToSendBack);
                server.StartListening();
                return server;
            }
        }

        private readonly string _expectedRequest;
        private readonly string _responseToSendBack;

        private static int _lastUsedPort = 2021;
        private static int GetAvailablePort()
        {
            // Gather info about existing ports
            var portList = new List<int>();
            var properties = IPGlobalProperties.GetIPGlobalProperties();
            portList.AddRange(properties.GetActiveTcpConnections().Select(c => c.LocalEndPoint.Port));
            portList.AddRange(properties.GetActiveTcpListeners().Select(c => c.Port));
            portList.AddRange(properties.GetActiveUdpListeners().Select(c => c.Port));

            _lastUsedPort++;
            while (portList.Contains(_lastUsedPort))
            {
                _lastUsedPort++;
            }

            return _lastUsedPort;
        }

        public class UnexpectedRequestDataException : Exception
        {
            public UnexpectedRequestDataException(string msg) : base(msg) { }
        }

    }
}
