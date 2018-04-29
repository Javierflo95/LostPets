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

namespace Entitites
{
    public class GoogleProfile
    {
        public string id { get; set; }
        public string name { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string birthday { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string accessToken { get; set; }

        private static GoogleProfile instance;

        public static GoogleProfile GetInstance()
        {
            if (instance == null)
                return new GoogleProfile();
            return instance;
        }

        public static void SetInstance(GoogleProfile profile)
        {
            instance = profile;
        }
    }
}