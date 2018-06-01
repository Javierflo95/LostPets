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
        public int sizeId { get; set; }
        public string name { get; set; }
        public DateTime dateOfBirth { get; set; }
        public char gender { get; set; }
        public string photo { get; set; }
        public string description { get; set; }

        private static Pet instance;

        public static Pet GetInstance()
        {
            if (instance == null)
                return new Pet();
            return instance;
        }

        public static void SetInstance(Pet pet)
        {
            instance = pet;
        }
    }
}