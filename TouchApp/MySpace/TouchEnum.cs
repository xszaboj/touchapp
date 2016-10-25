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
    public enum TouchEnum
    {
        Single = 0,
        Scroll = 1,
        SingleClick = 2,
        DoubleClick = 3,
        RightClick = 4,
        SingleClickDown = 5,
        SingleClickUp = 6
    }
}