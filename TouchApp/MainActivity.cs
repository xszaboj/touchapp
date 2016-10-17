using System;
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
        private Coordinates _prev;
        private TouchEnum _action = TouchEnum.Single;
        private bool isClickDown;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            if (_gestureDetector == null)
            {
                _gestureDetector = new GestureDetector(this);
            }
            if (_manager == null)
            {
                _manager = new TcpClientmanager();
                _manager.Connect();
            }
        }

        protected override void OnStop()
        {
            _manager.CloseConnection();
            base.OnStop();
        }

        protected override void OnDestroy()
        {
            _manager.CloseConnection();
            base.OnDestroy();
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            var source = e.Source;
            _gestureDetector.OnTouchEvent(e);
            return false;
        }

        public bool OnDown(MotionEvent e)
        {
            _prev = new Coordinates()
            {
                X = (int)e.GetX(),
                Y = (int)e.GetY()
            };
            
            return true;
        }

        public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            TextView t = FindViewById<TextView>(Resource.Id.textView1);
            t.Text = "OnFling";
            return true;
        }

        public void OnLongPress(MotionEvent e)
        {
            isClickDown = false;
            TextView t = FindViewById<TextView>(Resource.Id.textView1);
            _manager.SendMessage(string.Format("{0}|{1}|{2}", 0, 0, (int)TouchEnum.RightClick));
            t.Text = "OnLongPress";
        }

        public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
        {
            DecideAction(e1);

            int diffX = (int)e2.GetX() - _prev.X;
            int diffY = (int)e2.GetY() - _prev.Y;

            SetNewPrevious(e2);

            //send message
            if (_action == TouchEnum.Multi)
            {
                _manager.SendMessage(string.Format("{0}|{1}|{2}", diffX, diffY*2, (int) _action));
            }
            else
            {
                _manager.SendMessage(string.Format("{0}|{1}|{2}", diffX, diffY, (int)_action));
            }
            //Log messages
            TextView t2 = FindViewById<TextView>(Resource.Id.textView2);
            t2.Text = string.Format("e1x:{0}, e2x:{1}", e1.GetX(), e2.GetX());

            TextView t1 = FindViewById<TextView>(Resource.Id.textView1);
            t1.Text = _action == TouchEnum.Single ? "Move" : "Scroll";
            return true;
        }

        public void OnShowPress(MotionEvent e)
        {
            TextView t = FindViewById<TextView>(Resource.Id.textView1);
            t.Text = "onShowpress";
            isClickDown = true;
            _manager.SendMessage(string.Format("{0}|{1}|{2}", 0, 0, (int)TouchEnum.SingleClickDown));
        }

        public bool OnSingleTapUp(MotionEvent e)
        {
            TextView t = FindViewById<TextView>(Resource.Id.textView1);
            if (isClickDown)
            {
                isClickDown = false;
                t.Text = "Up";
                _manager.SendMessage(string.Format("{0}|{1}|{2}", 0, 0, (int) TouchEnum.SingleClickUp));
            }
            else
            {
                t.Text = "Click";
                _manager.SendMessage(string.Format("{0}|{1}|{2}", 0, 0, (int)TouchEnum.SingleClick));
            }
            return true;
        }

        public bool OnDoubleTap(MotionEvent e)
        {
            TextView t = FindViewById<TextView>(Resource.Id.textView1);
            t.Text = "OnDoubleTap";
            _manager.SendMessage(string.Format("{0}|{1}|{2}", 0, 0, (int)TouchEnum.DoubleClick));
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
                var intent = new Intent(this, typeof(OptionsActivity));
                StartActivity(intent);
                return true;
            }
            if (keyCode == Keycode.VolumeDown)
            {
                switchClicks = !switchClicks;
            }
            return base.OnKeyUp(keyCode, e);
        }

        private void DecideAction(MotionEvent e1)
        {
            var screenWidth = GetDeviceWidthInPixels();
            _action = e1.GetX() > screenWidth - (screenWidth/100 * 10) ? TouchEnum.Multi : TouchEnum.Single;
        }

        private int GetDeviceWidthInPixels()
        {
            var screen = Resources.DisplayMetrics;
            var screenWidth = screen.WidthPixels;
            return screenWidth;
        }

        private void SetNewPrevious(MotionEvent e2)
        {
            _prev = new Coordinates()
            {
                X = (int)e2.GetX(),
                Y = (int)e2.GetY()
            };
        }

        private struct Coordinates
        {
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}

