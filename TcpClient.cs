using System;
using System.Net.Sockets;
using System.Threading;

namespace TCPCommunication
{
    public sealed class TcpClient
    {
        System.Net.Sockets.TcpClient _socketConnection;
        Thread _clientReceiveThread;
        bool _isActive;

        public event Action<byte[]> OnBytesReceived;
        public event Action<Exception> OnException;


        public void Initialize()
        {
            _isActive = true;
            ConnectToTcpServer();
        }

        public void Stop() => _isActive = false;

        void ConnectToTcpServer()
        {
            try
            {
                _clientReceiveThread = new Thread(ListenForData);
                _clientReceiveThread.IsBackground = true;
                _clientReceiveThread.Start();
            }
            catch (Exception e)
            {
                OnException?.Invoke(e);
            }
        }

        void ListenForData()
        {
            try
            {
                _socketConnection = new System.Net.Sockets.TcpClient(TcpInfo.IpString, TcpInfo.PortNo);
                while (_isActive)
                {
                    using (var stream = _socketConnection.GetStream())
                    {
                        var bytes = new byte[_socketConnection.ReceiveBufferSize];
                        while (stream.Read(bytes, 0, bytes.Length) != 0) 
                            OnBytesReceived?.Invoke(bytes);
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
            if (_socketConnection == null)
                return;

            try
            {
                var stream = _socketConnection.GetStream();
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