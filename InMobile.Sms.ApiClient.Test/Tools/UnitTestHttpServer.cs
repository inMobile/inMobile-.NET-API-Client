using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using InMobile.Sms.ApiClient.Test.Tools;
using Xunit;

namespace InMobile.Sms.ApiClient.Test
{
    public class UnitTestHttpServer : IDisposable
    {
        public IPEndPoint EndPoint { get; private set; }
        public TcpListener _tcpListener;
        
        private List<IDisposable> Disposables = new List<IDisposable>();

        private UnitTestRequestInfo _expectedRequest;
        private UnitTestResponseInfo _response;

        private List<Exception> _excections = new List<Exception>();
        public string Host => $"{EndPoint.Address}:{EndPoint.Port}";
        private UnitTestHttpServer(IPEndPoint endPoint, UnitTestRequestInfo expectedRequest, UnitTestResponseInfo response)
        {
            EndPoint = endPoint ?? throw new ArgumentNullException(nameof(endPoint));
            _expectedRequest = expectedRequest ?? throw new ArgumentNullException(nameof(expectedRequest));
            _response = response ?? throw new ArgumentNullException(nameof(response));
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
            catch (Exception ex)
            {
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
            if (_excections.Any())
                throw _excections.First();
        }

        // Process the client connection.
        public void DoAcceptSocketCallback(IAsyncResult ar)
        {
            // End the operation and display the received data on
            // the console.
            var socket = _tcpListener.EndAcceptSocket(ar);

            // Stop listener after first connect
            _tcpListener.Stop();

            HandleSocket(socket);

        }

        private void HandleSocket(Socket socket)
        {
            // Read input
            var request = Receive(socket);
            var requestTextLines = request.Split("\r\n");

            // Find authorization header line
            var authLine = requestTextLines.SingleOrDefault(line => line.StartsWith("Authorization: Basic "));
            if (authLine == null)
            {
                _excections.Add(new UnexpectedAuthorizationException("No auth line found. Request: " + request));
            }
            else
            {
                var base64EncodedToken = authLine.Substring("Authorization: Basic ".Length);
                var tokenBytes = Convert.FromBase64String(base64EncodedToken);
                var usernameAndPassword = Encoding.ASCII.GetString(tokenBytes);
                var apiKey = usernameAndPassword.Substring(usernameAndPassword.IndexOf(":") + 1);
                if (apiKey != _expectedRequest.ApiKey.ApiKey)
                {
                    _excections.Add(new UnexpectedAuthorizationException("Expected apikey " + _expectedRequest.ApiKey.ApiKey + " but got " + apiKey));
                }
            }

            // Ensure expected method
            if (!request.StartsWith(_expectedRequest.MethodAndPath))
            {
                _excections.Add(new UnexpectedMethodAndPathException("Expected request to start with " + _expectedRequest.MethodAndPath + ". Request received: " + request));
            }

            

            // Ensure expected json

            var response = $@"HTTP/1.1 200 Ok
Date: Sun, 18 Oct 2012 10:36:20 GMT
Server: Apache/2.2.14 (Win32)
Content-Length: {_response.JsonOrNull?.Length}
Content-Type: application/json
Connection: Closed";

            if(_response.JsonOrNull != null)
            {
                response += $@"

{_response.JsonOrNull}";
            };

            Send(socket, response);
        }

        private static string Receive(Socket socket)
        {
            var buffer = new byte[10000];
            var receivedByteCount = socket.Receive(buffer: buffer);
            var request = Encoding.ASCII.GetString(bytes: buffer, index: 0, count: receivedByteCount);
            return request;
        }

        private static void Send(Socket socket, string data)
        {
            socket.NoDelay = true; // This will disable the Nagle algorthm and hereby prevent buffering and instead sending right away. https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.socket.nodelay?view=net-5.0
            socket.Send(Encoding.ASCII.GetBytes(data));
            socket.Close();
        }

        private static object _syncLock = new object();
        public static UnitTestHttpServer StartOnAnyAvailablePort(UnitTestRequestInfo expectedRequest, UnitTestResponseInfo response)
        {
            lock (_syncLock) // Ensures no race conditions ending up having multiple test server listening on the same port at the same time
            {
                var endPoint = new IPEndPoint(address: IPAddress.Loopback, port: LocalPortUtils.GetAvailablePort());
                var server = new UnitTestHttpServer(endPoint: endPoint, expectedRequest: expectedRequest, response: response);
                server.StartListening();
                return server;
            }
        }


        public class UnexpectedRequestDataException : Exception
        {
            public UnexpectedRequestDataException(string msg) : base(msg) { }
        }

        public class UnexpectedAuthorizationException : Exception
        {
            public UnexpectedAuthorizationException(string message) : base(message)
            {
            }
        }

        public class UnexpectedMethodAndPathException : Exception
        {
            public UnexpectedMethodAndPathException(string message) : base(message)
            {
            }
        }
    }
}
