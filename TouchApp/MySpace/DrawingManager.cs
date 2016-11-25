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
    public class DrawingManager
    {
        private Coordinates _prev;
        private TouchEnum _action = TouchEnum.Single;
        private bool _draw;
        private float _sensitivity = 1;

        private readonly IMessagener _manager;
        private readonly AdditionalInfo _info;

        public DrawingManager(IMessagener manager, AdditionalInfo info)
        {
            _manager = manager;
            _info = info;
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
            if (IsInDrawArea(e) && !_draw)
            {
                //start drawing
                _draw = true;
                _manager.SendMessage(string.Format("{0}|{1}|{2}", 0, 0, (int)TouchEnum.SingleClickDown));
            }
            else if (_draw && e.Action == MotionEventActions.Up)
            {
                //stop drawing
                _draw = false;
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
            int diffX = (int)e2.GetX() - _prev.X;
            int diffY = (int)e2.GetY() - _prev.Y;

            SetNewPrevious(e2);
            _manager.SendMessage($"{Math.Round(diffX * _sensitivity)}|{Math.Round(diffY * _sensitivity)}|{(int)_action}");
            
        }

        public void OnSingleTapUp(MotionEvent motionEvent)
        {
            _manager.SendMessage($"{0}|{0}|{(int)TouchEnum.SingleClick}");
        }

        private bool IsInDrawArea(MotionEvent e1)
        {
            var screenWidth = _info.DeviceWidth;
            return e1.GetX() < screenWidth - (screenWidth / 100 * 30);
        }
    }
}