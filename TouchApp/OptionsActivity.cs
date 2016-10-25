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
    [Activity(Label = "OptionsActivity")]
    public class OptionsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Options);

            EditText t = FindViewById<EditText>(Resource.Id.editText1);
            t.Text = Settings.IPSettings;
            EditText t2 = FindViewById<EditText>(Resource.Id.editText2);
            t2.Text = Settings.Port.ToString();

            var saveButton = FindViewById<Button>(Resource.Id.button1);
            saveButton.Click += (sender, e) =>
            {
                Settings.IPSettings = t.Text;
                Settings.Port = int.Parse(t2.Text);
                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            };
        }


    }
}