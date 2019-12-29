using System;
using System.Net.Sockets;
using System.Threading;

namespace TCPCommunication
{
    public sealed class TcpServer
    {
        TcpListener _tcpListener;
        Thread _tcpListenerThread;
        System.Net.Sockets.TcpClient _connectedTcpClient;

        public event Action<byte[]> OnBytesReceived;
        public event Action<Exception> OnException;
        
        bool _isActive;

        public void Initialize()
        {
            _isActive = true;
            _tcpListenerThread = new Thread(ListenForIncomingRequests);
            _tcpListenerThread.IsBackground = true;
            _tcpListenerThread.Start();
        }

        public void Stop() => _isActive = false;

        void ListenForIncomingRequests()
        {
            try
            {
                _tcpListener = new TcpListener(TcpInfo.IpAddress, TcpInfo.PortNo);
                _tcpListener.Start();
                while (_isActive)
                {
                    using (_connectedTcpClient = _tcpListener.AcceptTcpClient())
                    {
                        var bytes = new byte[_connectedTcpClient.ReceiveBufferSize];
                        using (var stream = _connectedTcpClient.GetStream())
                        {
                            while (stream.Read(bytes, 0, bytes.Length) != 0) 
                                OnBytesReceived?.Invoke(bytes);
                        }
                    }
                }
            }
            catch (SocketException socketException)
            {
                OnException?.Invoke(socketException);
            }
        }

        public void SendMessage(byte[] message)
        {
            if (_connectedTcpClient == null)
                return;

            try
            {
                var stream = _connectedTcpClient.GetStream();
                if (stream.CanWrite) 
                    stream.Write(message, 0, message.Length);
            }
            catch (SocketException socketException)
            {
                OnException?.Invoke(socketException);
            }
        }
    }
}