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
        protected override async void OnCreate(Bundle savedInstanceState)
        {

            Token oToken = Token.GetInstance();

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.OwnerProfile);
            Repository.Services.Service oService = new Repository.Services.Service();
            var a = await oService.GetOwnerApi(oToken);

        }


    }
}