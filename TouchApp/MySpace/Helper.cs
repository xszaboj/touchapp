using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TouchApp.MySpace
{
    public static class Helper
    {
        public static int GetDeviceWidthInPixels(this Activity activity)
        {
            var screen = activity.Resources.DisplayMetrics;
            var screenWidth = screen.WidthPixels;
            return screenWidth;
        }
    }
}