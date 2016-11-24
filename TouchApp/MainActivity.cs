using System;
using System.Diagnostics;
using System.Net.Sockets;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using TouchApp.MySpace;

namespace TouchApp
{
    [Activity(Label = "TouchApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, GestureDetector.IOnGestureListener, GestureDetector.IOnDoubleTapListener
    {
        private GestureDetector _gestureDetector;
        private TcpClientmanager _manager;
        private GestureManager _gestureManager;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += HandleExceptions;

            SetContentView(Resource.Layout.Main);
            try
            {
                if (_gestureDetector == null)
                {
                    _gestureDetector = new GestureDetector(this);
                }

                if (_manager == null)
                {
                    try
                    {
                        _manager = new TcpClientmanager();
                        _manager.Connect();
                    }
                    catch (Exception)
                    {
                        ShowAlertWindow("Connection problem", "Connection problem! Check IP address and port.",
                            "Options",
                            (sender, args) =>
                            {
                                 GoToOptions();
                            });
                    }
                }
                if (_gestureManager == null)
                {
                    _gestureManager = new GestureManager(_manager, new AdditionalInfo()
                    {
                        DeviceWidth = GetDeviceWidthInPixels()
                    });
                }
            }
            catch (Exception e)
            {
                
                TextView t = FindViewById<TextView>(Resource.Id.textView1);
                t.Text = e.ToString();
            }
        }

        private void HandleExceptions(object sender, UnhandledExceptionEventArgs e)
        {
            ShowAlertWindow("Error", e.ToString(), "continue", (o, args) => { });
        }

        protected override void OnStop()
        {
            _manager?.CloseConnection();
            base.OnStop();
        }

        protected override void OnDestroy()
        {
            _manager?.CloseConnection();
            base.OnDestroy();
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            _gestureManager.OnTouch(e);
            _gestureDetector.OnTouchEvent(e);
            return false;
        }

        public bool OnDown(MotionEvent e)
        {
            _gestureManager.OnDown(e);
            return true;
        }

        public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            return true;
        }

        public void OnLongPress(MotionEvent e)
        {
            _gestureManager.OnLongPress(e);
        }

        public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
        {
            _gestureManager.OnScroll(e1, e2, distanceX, distanceY);
            return true;
        }

        public void OnShowPress(MotionEvent e)
        {
        }

        public bool OnSingleTapUp(MotionEvent e)
        {
            _gestureManager.OnSingleTapUp(e);
            return true;
        }

        public bool OnDoubleTap(MotionEvent e)
        {
            _gestureManager.OnDoubleTap(e);
            return true;
        }

        public bool OnDoubleTapEvent(MotionEvent e)
        {
            return true;
        }

        public bool OnSingleTapConfirmed(MotionEvent e)
        {
            return true;
        }

        public override bool OnKeyUp(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.VolumeUp)
            {
                _gestureManager.SensitivityUp();
                return false;
            }
            if(keyCode == Keycode.VolumeDown)
            {
                _gestureManager.SensitivityDown();
                return false;
            }
            return base.OnKeyUp(keyCode, e);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.options, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.options:
                    GoToOptions();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void GoToOptions()
        {
            var intent = new Intent(this, typeof(OptionsActivity));
            StartActivity(intent);
        }

        private int GetDeviceWidthInPixels()
        {
            var screen = Resources.DisplayMetrics;
            var screenWidth = screen.WidthPixels;
            return screenWidth;
        }

        private void ShowAlertWindow(string title, string message, string positiveText, EventHandler<DialogClickEventArgs> positiveHandler)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle(title);
            alert.SetMessage(message);
            alert.SetPositiveButton(positiveText, positiveHandler);
            Dialog dialog = alert.Create();
            dialog.Show();
        }
    }
}

