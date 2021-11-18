using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace InMobile.Sms.ApiClient.Test.Tools
{
    public static class LocalPortUtils
    {
        private static int _lastUsedPort = 2021;
        private static object _syncLock = new object();
        public static int GetAndReserverAvailablePort()
        {
            lock (_syncLock)
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

                var portToUse = _lastUsedPort;
                _lastUsedPort++;
                return portToUse;
            }
        }

    }
}
