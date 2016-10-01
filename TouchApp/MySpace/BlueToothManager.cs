using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Java.Util;
using Exception = System.Exception;

namespace TouchApp.MySpace
{
    public class BlueToothManager
    {
        private StreamWriter socketWriter;
        private StreamReader socketReader;

        public void ConnectToBt()
        {
            try
            {
                var device = BluetoothAdapter.DefaultAdapter.GetRemoteDevice("5C:93:A2:F5:4B:F9");
                if (device != null)
                {
                    IntPtr createRfcommSocket = JNIEnv.GetMethodID(device.Class.Handle, "createInsecureRfcommSocket", "(I)Landroid/bluetooth/BluetoothSocket;");
                    IntPtr _socket = JNIEnv.CallObjectMethod(device.Handle, createRfcommSocket, new Android.Runtime.JValue(15));
                    var socket = Java.Lang.Object.GetObject<BluetoothSocket>(_socket, JniHandleOwnership.TransferLocalRef);
                    socket.Connect();
                    this.socketWriter = new StreamWriter(socket.OutputStream);
                    this.socketReader = new StreamReader(socket.InputStream);                
                    Task.Factory.StartNew(() => {
                        while (true)
                        {
                            var tele = this.socketReader.ReadLine();
                            Console.WriteLine(tele);
                        }

                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        public void HandleChange(object obj)
        {
            //  Console.WriteLine(obj.Value + " " + obj.Axis);
            if (this.socketWriter != null)
            {
                this.WritePacket(obj);

            }
        }

        private void WritePacket(object obj)
        {
            this.socketWriter.WriteLine(obj);
            this.socketWriter.Flush();
        }
    }
}