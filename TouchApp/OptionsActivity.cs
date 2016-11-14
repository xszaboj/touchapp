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
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += HandleExceptions;

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

        private void HandleExceptions(object sender, UnhandledExceptionEventArgs e)
        {
            ShowAlertWindow("Error", e.ToString(), "continue", (o, args) => { });
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