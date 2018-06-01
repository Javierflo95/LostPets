using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Entitites;
namespace LostPets.Droid.Activities
{
    [Activity(Label = "Lost Pets", Icon = "@mipmap/icon", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class RegisterPetActivity : AppCompatActivity
    {

        ImageView imgPhoto;
        ImageButton btnCat;
        ImageButton btnDog;
        Button btnRegisterPet;
        EditText txtNamePet;
        bool _isCat = false;
        bool _isDog = false;
        Pet oPet = Pet.GetInstance();
        Token oToken = Token.GetInstance();
        Owner oOwner = Owner.GetInstance();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.RegisterPet);

            imgPhoto = FindViewById<ImageView>(Resource.Id.ImgPhoto);
            btnCat = FindViewById<ImageButton>(Resource.Id.imgbtnCatType);
            btnDog = FindViewById<ImageButton>(Resource.Id.imgbtnDogType);
            btnRegisterPet = FindViewById<Button>(Resource.Id.btnRegisterPet);
            txtNamePet = FindViewById<EditText>(Resource.Id.txtNamePet);

            btnCat.Click += delegate
            {
                if (!_isCat)
                {
                    btnDog.Enabled = false;
                    _isDog = false;
                    _isCat = true;
                }
            };

            btnCat.Click += delegate
            {
                if (!_isDog)
                {
                    btnCat.Enabled = false;
                    _isCat = false;
                    _isDog = true;
                }
            };

            btnRegisterPet.Click += async delegate 
            {

                oPet.name = txtNamePet.Text;
                oPet.ownerId = Convert.ToInt32(oOwner.id);
                oPet.breedId = 1;
                oPet.sizeId = 2;
                oPet.dateOfBirth = DateTime.Now;
                oPet.gender = 'M';
                Repository.Services.Service oService = new Repository.Services.Service();
                var _oPet = await oService.RegisterPetApi(oPet, oToken);
            };

            imgPhoto.Click += ImgPhoto_Click;

        }

      

        private void ImgPhoto_Click(object sender, EventArgs e)
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

            Bitmap bitmap = MediaStore.Images.Media.GetBitmap(this.ContentResolver, data.Data);
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                byte[] array = stream.ToArray();
                oPet.photo = Base64.EncodeToString(array, Base64Flags.Default);
            }
            imgPhoto = FindViewById<ImageView>(Resource.Id.ImgPhoto);
            imgPhoto.SetImageURI(data.Data);
        }

        


    }
}