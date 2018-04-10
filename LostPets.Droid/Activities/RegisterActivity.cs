using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
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
    public class RegisterActivity : AppCompatActivity, IOnMapReadyCallback
    {
        ProfilePictureView oProfilePictureView;
        EditText txtName;
        EditText txtCorreo;

        public void OnMapReady(GoogleMap googleMap)
        {
            MarkerOptions oMarkerOptions = new MarkerOptions();
            oMarkerOptions.SetPosition(new LatLng(4.7396702, -74.098686));
            oMarkerOptions.SetTitle("I'm Here Javier");
            googleMap.AddMarker(oMarkerOptions);

            googleMap.UiSettings.ZoomControlsEnabled = true;
            googleMap.UiSettings.CompassEnabled = true;
            googleMap.MoveCamera(CameraUpdateFactory.ZoomIn());
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.RegisterPage);

            MapFragment oMapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            oMapFragment.GetMapAsync(this);

            oProfilePictureView = FindViewById<ProfilePictureView>(Resource.Id.ImgPro);
            txtCorreo = FindViewById<EditText>(Resource.Id.txtCorreo);
            txtName = FindViewById<EditText>(Resource.Id.txtName);

            var a = Global.GlobalApp.facebookProfile;
            oProfilePictureView.ProfileId = Global.GlobalApp.facebookProfile.id;
            txtName.Text = Global.GlobalApp.facebookProfile.name;
            txtCorreo.Text = Global.GlobalApp.facebookProfile.email;
        }
    }
}