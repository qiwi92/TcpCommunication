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
                _clientReceiveThread = new Thread(new ThreadStart(ListenForData));
                _clientReceiveThread.IsBackground = true;
                _clientReceiveThread.Start();
            }
            catch (Exception e)
            {
//            Debug.LogError("On client connect exception " + e);
            }
        }

        void ListenForData()
        {
            try
            {
                _socketConnection = new System.Net.Sockets.TcpClient(TcpInfo.IpString, TcpInfo.PortNo);
                var bytes = new byte[1024];
                while (_isActive)
                {
                    using (var stream = _socketConnection.GetStream())
                    {
                        int length;
                        while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            var incomingData = new byte[length];
                            Array.Copy(bytes, 0, incomingData, 0, length);
                            OnBytesReceived?.Invoke(bytes);
                        }
                    }
                }
            }
            catch (SocketException socketException)
            {
//            Debug.Log("Socket exception: " + socketException);
            }
        }

        public void SendMessage(byte[] message)
        {
            if (_socketConnection == null)
                return;

            try
            {
                // Get a stream object for writing. 			
                var stream = _socketConnection.GetStream();
                if (stream.CanWrite)
                {
                    stream.Write(message, 0, message.Length);
//                Debug.Log("Client sent his message - should be received by server");
                }
            }
            catch (SocketException socketException)
            {
//            Debug.LogError("Socket exception: " + socketException);
            }
        }
    }
}