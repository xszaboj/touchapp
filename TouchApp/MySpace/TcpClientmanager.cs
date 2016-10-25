using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TouchApp.MySpace
{
    public class TcpClientmanager : IMessagener
    {
        private const int TimeoutInSec = 5;
        private bool on = true;
        readonly TcpClient _clientSocket = new TcpClient();

        public void Connect()
        {
            try
            {
                string address = Settings.IPSettings;
                int port = Settings.Port;
                var result = _clientSocket.BeginConnect(address, port, null, null);
                var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(TimeoutInSec));
                if (!success)
                {
                    throw new Exception("Failed to connect.");
                }
                _clientSocket.EndConnect(result);
            }
            catch (Exception e)
            {
                throw;
            }
        }


        public void SendMessage(string str)
        {
            NetworkStream serverStream = _clientSocket.GetStream();
            if (str == "close")
            {
                _clientSocket.Close();
                on = false;
            }
            byte[] outStream = Encoding.ASCII.GetBytes(str + "$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
        }

        public void CloseConnection()
        {
            _clientSocket.Close();
        }
    }
}