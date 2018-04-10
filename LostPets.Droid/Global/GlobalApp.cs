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
using Entitites;

namespace LostPets.Droid.Global
{
    public class GlobalApp : Application
    {
        public static FacebookProfile facebookProfile { get; set; }
        public static string accessToken { get; set; }
    }
}