using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace proxy_uid.Core
{
    /// <summary>Detects LAN IPv4 and proxy endpoint for emulator http_proxy (port 54233).</summary>
    public static class NetworkProxy
    {
        public const int Port = 54233;

        private static string _cachedIp;
        private static DateTime _cachedAt = DateTime.MinValue;
        private static readonly TimeSpan CacheTtl = TimeSpan.FromSeconds(20);

        public static string Ip => GetLocalIPv4();

        public static string Endpoint => Ip + ":" + Port;

        public static void Refresh()
        {
            _cachedIp = null;
            _cachedAt = DateTime.MinValue;
            GetLocalIPv4(forceRefresh: true);
        }

        public static string GetLocalIPv4(bool forceRefresh = false)
        {
            if (!forceRefresh
                && !string.IsNullOrEmpty(_cachedIp)
                && DateTime.UtcNow - _cachedAt < CacheTtl)
            {
                return _cachedIp;
            }

            _cachedIp = ResolveLocalIPv4();
            _cachedAt = DateTime.UtcNow;
            return _cachedIp;
        }

        public static NetworkProxyStatus GetStatus()
        {
            Refresh();
            return new NetworkProxyStatus
            {
                Ip = Ip,
                Port = Port,
                Endpoint = Endpoint,
                UsingLocalhostFallback = Ip == "127.0.0.1"
            };
        }

        private static string ResolveLocalIPv4()
        {
            foreach (var ip in GetInterfaceIPv4Candidates())
            {
                if (IsUsableLanAddress(ip))
                    return ip;
            }

            foreach (var ip in GetDnsIPv4Candidates())
            {
                if (IsUsableLanAddress(ip))
                    return ip;
            }

            return "127.0.0.1";
        }

        private static IEnumerable<string> GetInterfaceIPv4Candidates()
        {
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus != OperationalStatus.Up)
                    continue;
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Loopback
                    || ni.NetworkInterfaceType == NetworkInterfaceType.Tunnel)
                    continue;

                string desc = (ni.Description ?? "") + " " + (ni.Name ?? "");
                if (desc.IndexOf("virtual", StringComparison.OrdinalIgnoreCase) >= 0
                    || desc.IndexOf("vmware", StringComparison.OrdinalIgnoreCase) >= 0
                    || desc.IndexOf("hyper-v", StringComparison.OrdinalIgnoreCase) >= 0
                    || desc.IndexOf("wsl", StringComparison.OrdinalIgnoreCase) >= 0
                    || desc.IndexOf("tap", StringComparison.OrdinalIgnoreCase) >= 0
                    || desc.IndexOf("vpn", StringComparison.OrdinalIgnoreCase) >= 0)
                    continue;

                IPInterfaceProperties props;
                try { props = ni.GetIPProperties(); }
                catch { continue; }

                if (props.GatewayAddresses == null || props.GatewayAddresses.Count == 0)
                    continue;

                foreach (var ua in props.UnicastAddresses)
                {
                    if (ua?.Address == null || ua.Address.AddressFamily != AddressFamily.InterNetwork)
                        continue;
                    yield return ua.Address.ToString();
                }
            }
        }

        private static IEnumerable<string> GetDnsIPv4Candidates()
        {
            var list = new List<string>();
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                        list.Add(ip.ToString());
                }
            }
            catch { }

            return list;
        }

        private static bool IsUsableLanAddress(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip))
                return false;
            if (!IPAddress.TryParse(ip, out var addr) || addr.AddressFamily != AddressFamily.InterNetwork)
                return false;
            if (IPAddress.IsLoopback(addr))
                return false;
            byte[] b = addr.GetAddressBytes();
            if (b[0] == 169 && b[1] == 254)
                return false;
            return true;
        }
    }

    public sealed class NetworkProxyStatus
    {
        public string Ip { get; set; }
        public int Port { get; set; }
        public string Endpoint { get; set; }
        public bool UsingLocalhostFallback { get; set; }
    }
}
