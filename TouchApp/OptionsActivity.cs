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

namespace TouchApp
{
    [Activity(Label = "OptionsActivity")]
    public class OptionsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Options);

            TextView t = FindViewById<TextView>(Resource.Id.textView1);
            t.Text = "192.168.1.0";

            var saveButton = FindViewById<Button>(Resource.Id.button1);
            saveButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            };
        }


    }
}