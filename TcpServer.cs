using System;
using System.Linq;
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
//            Debug.Log("Server is listening");
                var bytes = new byte[1024];
                while (_isActive)
                {
                    using (_connectedTcpClient = _tcpListener.AcceptTcpClient())
                    {
                        using (var stream = _connectedTcpClient.GetStream())
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
            }
            catch (SocketException socketException)
            {
//            Debug.Log("SocketException " + socketException.ToString());
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
                {
                    stream.Write(message, 0, message.Length);
//                Debug.Log("Server sent his message - should be received by client");
                }
            }
            catch (SocketException socketException)
            {
//            Debug.LogError("Socket exception: " + socketException);
            }
        }
    }
}