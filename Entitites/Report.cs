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
    public class Report
    {
        public int id { get; set; }
        public int ownerId { get; set; }
        public int petId { get; set; }
        public string description { get; set; }
        public string location { get; set; }
        public bool closed { get; set; }
    }
}