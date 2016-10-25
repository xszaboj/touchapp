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

namespace TouchApp.MySpace
{
    public class GestureManager
    {
        private Coordinates _prev;
        private TouchEnum _action = TouchEnum.Single;
        private bool _select;
        private float _sensitivity = 1;

        private readonly IMessagener _manager;
        private readonly AdditionalInfo _info;

        public GestureManager(IMessagener manager, AdditionalInfo info)
        {
            _manager = manager;
            _info = info;
        }

        private struct Coordinates
        {
            public int X { get; set; }
            public int Y { get; set; }
        }


        private void SetNewPrevious(MotionEvent e2)
        {
            _prev = new Coordinates()
            {
                X = (int)e2.GetX(),
                Y = (int)e2.GetY()
            };
        }

        public void OnTouch(MotionEvent e)
        {
            if (!_select && e.PointerCount == 2)
            {
                _select = true;
                _manager.SendMessage(string.Format("{0}|{1}|{2}", 0, 0, (int)TouchEnum.SingleClickDown));
            }
            if (_select && e.Action == MotionEventActions.Up)
            {
                _select = false;
                _manager.SendMessage(string.Format("{0}|{1}|{2}", 0, 0, (int)TouchEnum.SingleClickUp));
            }
        }

        public void OnDown(MotionEvent e)
        {

            _prev = new Coordinates()
            {
                X = (int)e.GetX(),
                Y = (int)e.GetY()
            };
        }

        public void OnLongPress(MotionEvent e)
        {
            _manager.SendMessage(string.Format("{0}|{1}|{2}", 0, 0, (int)TouchEnum.RightClick));
        }

        public void OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
        {
            DecideAction(e1);

            int diffX = (int)e2.GetX() - _prev.X;
            int diffY = (int)e2.GetY() - _prev.Y;

            SetNewPrevious(e2);

            //send message
            if (_action == TouchEnum.Scroll)
            {
                _manager.SendMessage($"{diffX}|{diffY*2*_sensitivity}|{(int) _action}");
            }
            else
            {
                _manager.SendMessage($"{Math.Round(diffX*_sensitivity)}|{Math.Round(diffY * _sensitivity)}|{(int) _action}");
            }
        }

        public void OnSingleTapUp(MotionEvent motionEvent)
        {
            _manager.SendMessage($"{0}|{0}|{(int) TouchEnum.SingleClick}");
        }

        public void OnDoubleTap(MotionEvent motionEvent)
        {
            _manager.SendMessage($"{0}|{0}|{(int) TouchEnum.DoubleClick}");
        }

        public void SensitivityUp()
        {
            if (_sensitivity >= 5)
            {
                _sensitivity = 5;
            }
            _sensitivity = _sensitivity + 0.5f;
        }

        public void SensitivityDown()
        {
            if (_sensitivity <= 1)
            {
                _sensitivity = 1;
            }
            _sensitivity = _sensitivity - 0.5f;
        }

        private void DecideAction(MotionEvent e1)
        {
            var screenWidth = _info.DeviceWidth;
            _action = e1.GetX() > screenWidth - (screenWidth / 100 * 10) ? TouchEnum.Scroll : TouchEnum.Single;
        }
    }
}