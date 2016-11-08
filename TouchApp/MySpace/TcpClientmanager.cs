using System;
using System.IO;
using System.Net.Sockets;

namespace TouchApp.MySpace
{
    public class TcpClientmanager : IMessagener
    {
        private const int TimeoutInSec = 5;
        readonly TcpClient _clientSocket = new TcpClient();

        public void Connect()
        {
            string address = Settings.IPSettings;
            int port = Settings.Port;
            IAsyncResult result = _clientSocket.BeginConnect(address, port, null, null);
            bool success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(TimeoutInSec));
            if (!success)
            {
                throw new Exception($"Failed to connect to {address}:{port}.");
            }
            _clientSocket.EndConnect(result);
        }


        public void SendMessage(string str)
        {
            var serverStream = _clientSocket.GetStream();
            StreamWriter writer = new StreamWriter(serverStream) { AutoFlush = true };
            writer.WriteLineAsync(str);
        }

        public void CloseConnection()
        {
            _clientSocket.Close();
        }
    }
}