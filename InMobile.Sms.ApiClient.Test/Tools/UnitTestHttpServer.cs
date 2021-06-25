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

        private List<Exception> _excections = new List<Exception>();
        public string Host => $"{EndPoint.Address}:{EndPoint.Port}";
        private Queue<RequestResponsePair> _requestPairsQueue;

        private UnitTestHttpServer(IPEndPoint endPoint, RequestResponsePair[] requests)
        {
            EndPoint = endPoint ?? throw new ArgumentNullException(nameof(endPoint));
            _requestPairsQueue = new Queue<RequestResponsePair>(requests);
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

            HandleSocket(socket);

            _tcpListener.BeginAcceptSocket(DoAcceptSocketCallback, _tcpListener);
        }

        private void HandleSocket(Socket socket)
        {
            try
            {
                bool keepRunning = true;
                while (keepRunning)
                {
                    // Read input
                    var request = Receive(socket);
                    if (!string.IsNullOrEmpty(request))
                    {
                        var requestTextLines = request.Split("\r\n");

                        if (_requestPairsQueue.Count == 0)
                        {
                            _excections.Add(new NoMoreRequestsExceptionException($"Got unexpected request: {request}"));
                            Assert.True(false, $"Got unexpected request: {request}");
                            return;
                        }

                        var nextPair = _requestPairsQueue.Dequeue();

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
                            if (apiKey != nextPair.Request.ApiKey.ApiKey)
                            {
                                _excections.Add(new UnexpectedAuthorizationException("Expected apikey " + nextPair.Request.ApiKey.ApiKey + " but got " + apiKey));
                            }
                        }

                        // Ensure expected method
                        if (!request.StartsWith(nextPair.Request.MethodAndPath))
                        {
                            _excections.Add(new UnexpectedMethodAndPathException("Expected request to start with: " + Environment.NewLine + nextPair.Request.MethodAndPath + Environment.NewLine + "Request received: " + Environment.NewLine + request));
                        }

                        // Ensure expected json
                        var expectedEndOfRequest = "\r\n\r\n";
                        if (nextPair.Request.JsonOrNull != null)
                        {
                            // Expect no payload
                            expectedEndOfRequest += nextPair.Request.JsonOrNull;
                        }

                        if (!request.EndsWith(expectedEndOfRequest))
                        {
                            _excections.Add(new UnexpectedPayloadException("Request was expected to end with '" + expectedEndOfRequest + "'\n\nbut did not. Request: " + request));
                        }

                        var response = $@"HTTP/1.1 {nextPair.Response.StatusCodeString}
Date: Sun, 18 Oct 2012 10:36:20 GMT
Server: Apache/2.2.14 (Win32)
Content-Length: {nextPair.Response.JsonOrNull?.Length}
Content-Type: application/json
Connection: Closed

";

                        if (nextPair.Response.JsonOrNull != null)
                        {
                            response += $@"{nextPair.Response.JsonOrNull}";
                        };

                        Send(socket, response);
                    }
                    else
                    {
                        keepRunning = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
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
        }

        private static object _syncLock = new object();
        public static UnitTestHttpServer StartOnAnyAvailablePort(params RequestResponsePair[] requests)
        {
            lock (_syncLock) // Ensures no race conditions ending up having multiple test server listening on the same port at the same time
            {
                var endPoint = new IPEndPoint(address: IPAddress.Loopback, port: LocalPortUtils.GetAndReserverAvailablePort());
                var server = new UnitTestHttpServer(endPoint: endPoint, requests: requests);
                server.StartListening();
                return server;
            }
        }

        public void AssertNoAwaitingRequestsLeft()
        {
            Assert.Empty(_requestPairsQueue);
        }
        public class RequestResponsePair
        {
            public UnitTestRequestInfo Request { get; }
            public UnitTestResponseInfo Response { get; }

            public RequestResponsePair(UnitTestRequestInfo request, UnitTestResponseInfo response)
            {
                Request = request ?? throw new ArgumentNullException(nameof(request));
                Response = response ?? throw new ArgumentNullException(nameof(response));
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

        public class NoMoreRequestsExceptionException : Exception
        {
            public NoMoreRequestsExceptionException(string message) : base(message)
            {
            }
        }

        public class UnexpectedMethodAndPathException : Exception
        {
            public UnexpectedMethodAndPathException(string message) : base(message)
            {
            }
        }

        public class UnexpectedPayloadException : Exception
        {
            public UnexpectedPayloadException(string message) : base(message)
            {
            }
        }
    }
}
