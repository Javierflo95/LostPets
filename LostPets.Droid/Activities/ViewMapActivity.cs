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
using Android.Views;
using Android.Widget;

namespace LostPets.Droid.Activities
{
    [Activity(Label = "ViewMapActivity")]
    public class ViewMapActivity : Activity, IOnMapReadyCallback
    {
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

            SetContentView(Resource.Layout.ViewMap);
            MapFragment oMapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            oMapFragment.GetMapAsync(this);

        }
    }
}