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
        int count = 1;
        private GestureDetector _gestureDetector;
        private TcpClientmanager _manager;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);
            

            button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };
            _gestureDetector = new GestureDetector(this);
            _manager = new TcpClientmanager();
            _manager.Connect();
            button.Click += delegate { _manager.SendMessage("close"); };
        }
        

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (e.PointerCount > 1)
            {
                _manager.SendMessage("multi");
                TextView t = FindViewById<TextView>(Resource.Id.textView1);
                t.Text = "multi";

                TextView coords = FindViewById<TextView>(Resource.Id.textView2);
                coords.Text = string.Format("X: {0}, Y:{1}", e.GetX(), e.GetY());
            }
            else
            {
                _manager.SendMessage("single");
                TextView t = FindViewById<TextView>(Resource.Id.textView1);
                t.Text = "single";

                TextView coords = FindViewById<TextView>(Resource.Id.textView2);
                coords.Text = string.Format("X: {0}, Y:{1}", e.GetX(), e.GetY());
            }

        return true;

        //_gestureDetector.OnTouchEvent(e);
        //    return false;

        }

        public bool OnDown(MotionEvent e)
        {
            TextView t = FindViewById<TextView>(Resource.Id.textView1);
            t.Text = "OnDown";
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
            TextView t = FindViewById<TextView>(Resource.Id.textView1);
            t.Text = "OnLongPress";
        }

        public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
        {
            TextView t = FindViewById<TextView>(Resource.Id.textView1);
            t.Text = string.Format("DistanceX:{0}", distanceX);

            TextView t2 = FindViewById<TextView>(Resource.Id.textView2);
            t2.Text = string.Format("DistanceY:{0}", distanceY);
            return true;
        }

        public void OnShowPress(MotionEvent e)
        {
            TextView t = FindViewById<TextView>(Resource.Id.textView1);
            t.Text = "onShowpress";
        }

        public bool OnSingleTapUp(MotionEvent e)
        {
            TextView t = FindViewById<TextView>(Resource.Id.textView1);
            t.Text = "Onsingletapup";
            return true;
        }

        public bool OnDoubleTap(MotionEvent e)
        {
            TextView t = FindViewById<TextView>(Resource.Id.textView2);
            t.Text = "OnDoubleTap";
            return true;
        }

        public bool OnDoubleTapEvent(MotionEvent e)
        {
            TextView t = FindViewById<TextView>(Resource.Id.textView2);
            t.Text = "OnDoubleTapEvent";
            return true;
        }

        public bool OnSingleTapConfirmed(MotionEvent e)
        {
            TextView t = FindViewById<TextView>(Resource.Id.textView2);
            t.Text = "OnSingleTapConfirmed";
            return true;
        }


    }
}

