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
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace TouchApp.MySpace
{
    public static class Settings
    {
        #region Setting Constants

        private const string SettingsKey = "ip_address";
        private static readonly string SettingsDefault = "192.168.0.6";
        private const string portKey = "port";
        private static readonly int portDefault = 8889;

        #endregion

        private static ISettings AppSettings
        {
            get { return CrossSettings.Current; }
        }

        public static string IPSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault<string>(SettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<string>(SettingsKey, value);
            }
        }

        public static int Port
        {
            get { return AppSettings.GetValueOrDefault(portKey, portDefault); }
            set { AppSettings.AddOrUpdateValue(portKey, value); } }
    }
}