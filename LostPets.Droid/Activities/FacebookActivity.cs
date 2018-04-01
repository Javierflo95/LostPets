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
using LostPets.Droid.Activities;
using Java.Lang;
using Repository.Services;
using Xamarin.Facebook;
using Xamarin.Facebook.Login.Widget;
using Xamarin.Facebook.Login;
using Android.Content.PM;
using Java.Security;

namespace LostPets.Droid.Activities
{

    [Activity(Label = "Lost Pets", Icon = "@mipmap/icon")]
    public class FacebookActivity : Activity, IFacebookCallback
    {
        ICallbackManager oICallbackManager;
        FacebookService oFacebookService;


        TextView txtFirstName;
        TextView txtLastName;
        TextView txtName;
        ProfilePictureView pictureView;
        LoginButton btnLogin;


        public void OnCancel()
        {
        }

        public void OnError(FacebookException error)
        {
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            LoginResult loginResult = (LoginResult)result;

            Console.WriteLine(loginResult.AccessToken);

        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            //Initializes the sdk face
            FacebookSdk.SdkInitialize(this.ApplicationContext);


            //PackageInfo oPackageInfo = this.PackageManager.GetPackageInfo("com.facebook.FacebookActivity", PackageInfoFlags.Signatures);

            //foreach (Android.Content.PM.Signature oSignature in oPackageInfo.Signatures)
            //{
            //    MessageDigest oMessageDigest = MessageDigest.GetInstance("SHA");
            //    oMessageDigest.Update(oSignature.ToByteArray());

            //    string keyHash = Convert.ToBase64String(oMessageDigest.Digest());
            //    Console.WriteLine("KeyHash:", keyHash);
            //}

            //Initializes the Profile Tracker service
            oFacebookService = new FacebookService();
            oFacebookService.mOnProfileChanged += OFacebookService_mOnProfileChanged;
            oFacebookService.StartTracking();
            //Set Content view
            SetContentView(Resource.Layout.FacebookLogin);

            btnLogin = FindViewById<LoginButton>(Resource.Id.fblogin);
            txtFirstName = FindViewById<TextView>(Resource.Id.TxtFirstname);
            txtLastName = FindViewById<TextView>(Resource.Id.TxtLastName);
            txtName = FindViewById<TextView>(Resource.Id.TxtName);
            pictureView = FindViewById<ProfilePictureView>(Resource.Id.ImgPro);

            btnLogin.Click += (o, e) =>
            {
                if (AccessToken.CurrentAccessToken != null && Profile.CurrentProfile != null)
                {
                    //send this info to the other page 
                }
                else
                {
                    btnLogin.SetReadPermissions(new List<string> { "user_friends", "public_profile" });
                    oICallbackManager = CallbackManagerFactory.Create();
                    btnLogin.RegisterCallback(oICallbackManager, this);

                }
            };
        }

        private void OFacebookService_mOnProfileChanged(object sender, OnProfileChangedEventArgs e)
        {
            if (e.mProfile != null)
            {
                try
                {

                    txtFirstName.Text = e.mProfile.FirstName;
                    txtLastName.Text = e.mProfile.LastName;
                    txtName.Text = e.mProfile.Name;
                    pictureView.ProfileId = e.mProfile.Id;

                }
                catch (Java.Lang.Exception ex)
                {
                }
            }
            else
            {
                txtFirstName.Text = "First Name";
                txtLastName.Text = "Last Name";
                txtName.Text = "Name";
                pictureView.ProfileId = null;
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            oICallbackManager.OnActivityResult(requestCode, (int)resultCode, data);
        }
    }
}