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
    public class Pet
    {
        public int breedId { get; set; }
        public int ownerId { get; set; }
        public List<Pet> pets { get; set; }


    }
}