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
using Entitites;
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

            oProfilePictureView.Click += OProfilePictureView_Click;

            Owner oOwner = Owner.GetInstance();
            oProfilePictureView.ProfileId = oOwner.facebookId;
            txtName.Text = $"{oOwner.firstName} { oOwner.lastName}";
            txtCorreo.Text = oOwner.email;
        }

        private void OProfilePictureView_Click(object sender, EventArgs e)
        {
            var imageIntent = new Intent();
            imageIntent.SetType("image/*");
            imageIntent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(
                Intent.CreateChooser(imageIntent, "Select photo"), 0);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok)
            {
                var imageView = FindViewById<ImageView>(Resource.Id.ImgPhoto);
                oProfilePictureView.Visibility = ViewStates.Invisible;
                imageView.SetImageURI(data.Data);
                imageView.Visibility = ViewStates.Visible;
            }
        }
    }
}