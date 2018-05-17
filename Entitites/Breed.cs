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
    public class Breed
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public List<Pet> pets { get; set; }

        private static Breed instance;

        public static Breed GetInstance()
        {
            if (instance == null)
                return new Breed();
            return instance;
        }

        public static void SetInstance(Breed breed)
        {
            instance = breed;
        }
    }
}