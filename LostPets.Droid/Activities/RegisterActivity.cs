using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Entitites;
using Xamarin.Facebook.Login.Widget;
using Repository.Services;

namespace LostPets.Droid.Activities
{
    [Activity(Label = "Lost Pets", Icon = "@mipmap/icon", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class RegisterActivity : AppCompatActivity, IOnMapReadyCallback, ILocationListener
    {
        ProfilePictureView oProfilePictureView;
        EditText txtName;
        EditText txtNumber;   
        EditText txtCedula;
        EditText txtCorreo;
        Button btnRegistro;
        Location _currentLocation;
        LocationManager _locationManager;
        string _locationProvider;
        Owner oOwner = Owner.GetInstance();

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
            btnRegistro = FindViewById<Button>(Resource.Id.btnRegistro);
            txtCedula = FindViewById<EditText>(Resource.Id.txtCedula);
            txtNumber = FindViewById<EditText>(Resource.Id.txtNumber);

            oProfilePictureView.Click += OProfilePictureView_Click;


            oProfilePictureView.ProfileId = oOwner.facebookId;
            txtName.Text = $"{oOwner.firstName} { oOwner.lastName}";
            txtCorreo.Text = oOwner.email;
            InitializeLocationManager();

            btnRegistro.Click += async delegate
            {
                Random random = new Random();
                string randomNumber = string.Join(string.Empty, Enumerable.Range(0, 10).Select(number => random.Next(0, 9).ToString()));

                oOwner.firstName = txtName.Text;
                oOwner.email = txtCorreo.Text;
                oOwner.identificationNumber = randomNumber;
                oOwner.phone = txtNumber.Text;
                Repository.Services.Service oService = new Repository.Services.Service();
                var _oOwner = await oService.RegisterApi(oOwner);
            };


        }
   


        private void OProfilePictureView_Click(object sender, EventArgs e)
        {
            var imageIntent = new Intent();
            imageIntent.SetType("image/*");
            imageIntent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(
                Intent.CreateChooser(imageIntent, "Select photo"), 0);
        }

        private void InitializeLocationManager()
        {
            _locationManager = (LocationManager)GetSystemService(LocationService);
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine
            };
            IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

            if (acceptableLocationProviders.Any())
            {
                _locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                _locationProvider = string.Empty;
            }
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

        private async Task<Address> ReverseGeocodeCurrentLocation()
        {
            Geocoder geocoder = new Geocoder(this);
            IList<Address> addressList =
                await geocoder.GetFromLocationAsync(_currentLocation.Latitude, _currentLocation.Longitude, 10);

            Address address = addressList.FirstOrDefault();
            return address;
        }

        public async void OnLocationChanged(Location location)
        {
            _currentLocation = location;
            if (_currentLocation == null)
            {
                //_locationText.Text = "Unable to determine your location. Try again in a short while.";
            }
            else
            {
                var address = string.Format("{0} {1}", _currentLocation.Latitude.ToString().Replace(',','.'), _currentLocation.Longitude.ToString().Replace(',', '.'));
                var geocoder = new Geocoder(this);
                IList<Address> addressList = await geocoder.GetFromLocationAsync(location.Latitude, location.Longitude, 3);
                Address direccion = addressList.FirstOrDefault();
                var _direccion = $"{direccion.Thoroughfare} {direccion.SubThoroughfare} {direccion.SubLocality}";
                oOwner.address = _direccion;
                oOwner.location = address;
            }
        }




        public void OnProviderDisabled(string provider)
        {
        }

        public void OnProviderEnabled(string provider)
        {
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
        }

        protected override void OnResume()
        {
            base.OnResume();
            _locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);
        }

        protected override void OnPause()
        {
            base.OnPause();
            _locationManager.RemoveUpdates(this);
        }
    }
}