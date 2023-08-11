using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Xunit;

namespace InMobile.Sms.ApiClient.Test
{
#pragma warning disable S3881 // "IDisposable" should be implemented correctly
    public class UnitTestHttpServer : IDisposable
#pragma warning restore S3881 // "IDisposable" should be implemented correctly
    {
        public IPEndPoint EndPoint => ((IPEndPoint)_tcpListener.LocalEndpoint);
        public TcpListener _tcpListener;

        private readonly List<IDisposable> Disposables = new List<IDisposable>();
        private readonly List<Exception> _exceptions = new List<Exception>();
        private readonly Queue<RequestResponsePair> _requestPairsQueue;

        public string Host => $"{EndPoint.Address}:{EndPoint.Port}";

        private UnitTestHttpServer(RequestResponsePair[] requests)
        {
            _requestPairsQueue = new Queue<RequestResponsePair>(requests);
        }

        private void StartListening()
        {
            if (_tcpListener != null)
                throw new Exception("Already listening");
            _tcpListener = new TcpListener(localaddr: IPAddress.Loopback, port: 0);
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
                catch 
                {
                    // Ignore this
                }
            }

#pragma warning disable S3877 // Exceptions should not be thrown from unexpected methods
            // Check if server did not receive expected http data
            if (_exceptions.Any())
                throw _exceptions[0];
#pragma warning restore S3877 // Exceptions should not be thrown from unexpected methods
        }

        // Process the client connection.
        public void DoAcceptSocketCallback(IAsyncResult ar)
        {
            try
            {
                // End the operation and display the received data on
                // the console.
                var socket = _tcpListener.EndAcceptSocket(ar);

                HandleSocket(socket);

                _tcpListener.BeginAcceptSocket(DoAcceptSocketCallback, _tcpListener);
            }
            catch (Exception ex)
            {
                // A vast amount of different exception can occur here as a result of tests disposing before this accept call is actually done. Of this reason, any exceptions here are ignored.
                ex.ToString();
            }
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

                        if (!_requestPairsQueue.Any())
                        {
                            _exceptions.Add(new NoMoreRequestsExceptionException($"Got unexpected request: {request}"));
                            Assert.Fail($"Got unexpected request: {request}");
                            return;
                        }

                        var nextPair = _requestPairsQueue.Dequeue();

                        // Find authorization header line
                        var authLine = requestTextLines.SingleOrDefault(line => line.StartsWith("Authorization: Basic "));
                        if (authLine == null)
                        {
                            _exceptions.Add(new UnexpectedAuthorizationException("No auth line found. Request: " + request));
                        }
                        else
                        {
                            var base64EncodedToken = authLine.Substring("Authorization: Basic ".Length);
                            var tokenBytes = Convert.FromBase64String(base64EncodedToken);
                            var usernameAndPassword = Encoding.ASCII.GetString(tokenBytes);
                            var apiKey = usernameAndPassword.Substring(usernameAndPassword.IndexOf(":") + 1);
                            if (apiKey != nextPair.Request.ApiKey.ApiKey)
                            {
                                _exceptions.Add(new UnexpectedAuthorizationException("Expected apikey " + nextPair.Request.ApiKey.ApiKey + " but got " + apiKey));
                            }
                        }

                        // Ensure expected method
                        if (!request.StartsWith(nextPair.Request.MethodAndPath))
                        {
                            _exceptions.Add(new UnexpectedMethodAndPathException("Expected request to start with: " + Environment.NewLine + nextPair.Request.MethodAndPath + Environment.NewLine + "Request received: " + Environment.NewLine + request));
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
                            _exceptions.Add(new UnexpectedPayloadException("Request was expected to end with '" + expectedEndOfRequest + "'\n\nbut did not. Request: " + request));
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
                        }

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

        private static readonly object _syncLock = new object();
        public static UnitTestHttpServer StartOnAnyAvailablePort(params RequestResponsePair[] requests)
        {
            lock (_syncLock) // Ensures no race conditions ending up having multiple test server listening on the same port at the same time
            {
                var server = new UnitTestHttpServer(requests: requests);
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
