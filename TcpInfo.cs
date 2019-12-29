using System.Net;

namespace TCPCommunication
{
    static class TcpInfo
    {
        public const int PortNo = 5000;
        public const string IpString = "127.0.0.1";
        public static readonly IPAddress IpAddress = IPAddress.Parse(IpString);
    }
}