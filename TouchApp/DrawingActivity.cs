using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TouchApp.MySpace;

namespace TouchApp
{
    [Activity(Label = "DrawingActivity")]
    public class DrawingActivity : Activity, GestureDetector.IOnGestureListener
    {
        private GestureDetector _gestureDetector;
        private TcpClientmanager _manager;
        private DrawingManager _gestureManager;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Drawing);
            if (_gestureDetector == null)
            {
                _gestureDetector = new GestureDetector(this);
            }
            if (_manager == null)
            {
                _manager = new TcpClientmanager();
                _manager.Connect();
            }
            if (_gestureManager == null)
            {
                _gestureManager = new DrawingManager(_manager, new AdditionalInfo()
                {
                    DeviceWidth = this.GetDeviceWidthInPixels()
                });
            }
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
    }
}