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
    public class Owner
    {
        public string id { get; set; }
        public string name { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string birthday { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string accessTokenApi { get; set; }
        public string accessTokenFacebook { get; set; }
        public string facebookId { get; set; }
        public string accessTokenGoogle { get; set; }
        public string contrasena { get; set; }

        private static Owner instance;

        public static Owner GetInstance()
        {
            if (instance == null)
                return new Owner();
            return instance;
        }

        public static void SetInstance(Owner profile)
        {
            instance = profile;
        }
    }
}