using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Xamarin.Facebook.Login.Widget;

namespace LostPets.Droid.Activities
{
    [Activity(Label = "Lost Pets", Icon = "@mipmap/icon", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class RegisterActivity : AppCompatActivity
    {
        ProfilePictureView oProfilePictureView;
        EditText txtName;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.RegisterPage);
            oProfilePictureView = FindViewById<ProfilePictureView>(Resource.Id.ImgPro);
            txtName = FindViewById<EditText>(Resource.Id.txtName);
            oProfilePictureView.ProfileId = Global.GlobalApp.pictureView;

            txtName.Text = Global.GlobalApp.name;

           

        }
    }
}