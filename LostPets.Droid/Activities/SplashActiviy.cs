using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using GR.Net.Maroulis.Library;

namespace LostPets.Droid.Activities
{
    [Activity(Label = "Lost Pets", MainLauncher = true, Icon = "@mipmap/icon", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class SplashActiviy : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Show Splash Screen
            var config = new EasySplashScreen(this)
                .WithFullScreen()
                .WithTargetActivity(Java.Lang.Class.FromType(typeof(MainActivity)))
                .WithSplashTimeOut(5000)// 5 sec
                .WithBackgroundColor(Color.ParseColor("#F2E2CE"))
                .WithLogo(Resource.Drawable.splashScreen);
                //.WithHeaderText("Bienvenido a LostPets!.")
                //.WithFooterText("Copyright 2018")
                //.WithBeforeLogoText("universidad ECCI")
                //.WithAfterLogoText("Innovacion Tecnologica");

            //Set Text Color
            //config.HeaderTextView.SetTextColor(Color.White);
            //config.FooterTextView.SetTextColor(Color.White);
            //config.BeforeLogoTextView.SetTextColor(Color.White);
            //config.AfterLogoTextView.SetTextColor(Color.White);

            //Create View
            View oView = config.Create();

            //Set Content View
            SetContentView(oView);
        }
    }
}