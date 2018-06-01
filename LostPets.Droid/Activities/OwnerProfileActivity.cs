using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Entitites;
using Repository.Services;

namespace LostPets.Droid.Activities
{
    [Activity(Label = "Lost Pets", Icon = "@mipmap/icon", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class OwnerProfileActivity : AppCompatActivity
    {
        TextView lblName;
        TextView lblPhone;
        TextView lblMail;
        TextView lblIdentification;
        TextView lblAddress;
        Button btnAddPet;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            Owner oOwner = new Owner();
            Token oToken = Token.GetInstance();

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.OwnerProfile);

            lblName = FindViewById<TextView>(Resource.Id.lblName);
            lblPhone = FindViewById<TextView>(Resource.Id.lblPhone);
            lblMail = FindViewById<TextView>(Resource.Id.lblMail);
            lblIdentification = FindViewById<TextView>(Resource.Id.lblIdentification);
            lblAddress = FindViewById<TextView>(Resource.Id.lblAddress);
            btnAddPet = FindViewById<Button>(Resource.Id.btnAddPet);

            Repository.Services.Service oService = new Repository.Services.Service();
            Owner.SetInstance(await oService.GetOwnerApi(oToken));
            oOwner = Owner.GetInstance();
            lblName.Text = oOwner.firstName;
            lblPhone.Text = oOwner.phone;
            lblMail.Text = oOwner.email;
            lblIdentification.Text = oOwner.identificationNumber;
            lblAddress.Text = oOwner.address;

            btnAddPet.Click += (e, v) =>
            {
                StartActivity(typeof(RegisterPetActivity));
            };

        }

    }
}