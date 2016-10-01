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
    public class TcpClientmanager
    {
        private bool on = true;
        readonly TcpClient _clientSocket = new TcpClient();

        public void Connect()
        {
            try
            {
                _clientSocket.Connect("192.168.0.5", 8889);
            }
            catch (Exception e)
            {
                throw e;
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
    }
}