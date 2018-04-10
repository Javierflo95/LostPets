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

namespace LostPets.Droid.Global
{
    public class GlobalApp : Application
    {
        public static string firstName { get; set; }
        public static string lastName { get; set; }
        public static string name { get; set; }
        public static string pictureView { get; set; }
    }
}