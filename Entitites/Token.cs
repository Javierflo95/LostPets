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
    public class Token
    {
        public string accessTokenApi { get; set; }

        private static Token instance;

        public static Token GetInstance()
        {
            if (instance == null)
                return new Token();
            return instance;
        }

        public static void SetInstance(Token oToken)
        {
            instance = oToken;
        }
    }
}