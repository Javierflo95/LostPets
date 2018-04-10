using Android.App;
using Android.Widget;
using Android.OS;
using LostPets.Droid.Activities;
using Android.Support.V7.App;
using Xamarin.Facebook;
using Java.Lang;
using Xamarin.Facebook.Login.Widget;
using System.Collections.Generic;
using Repository.Services;
using Xamarin.Facebook.Login;
using System;
using Android.Content;
using Android.Runtime;

namespace LostPets.Droid
{
    [Activity(Label = "Lost Pets", Icon = "@mipmap/icon", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class MainActivity : AppCompatActivity, IFacebookCallback
    {

        LoginButton btnLogin;
        ICallbackManager oICallbackManager;
        FacebookService oFacebookService;



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
            //Set content View
            SetContentView(Resource.Layout.Main);
            btnLogin = FindViewById<LoginButton>(Resource.Id.fblogin);

            if (AccessToken.CurrentAccessToken != null && Profile.CurrentProfile != null)
            {
                StartActivity(typeof(RegisterActivity));
            }
            else
            {
                ConnectWithFacebook();
            }

            btnLogin.Click += delegate
            {
                ConnectWithFacebook();
            };

            }

        public void ConnectWithFacebook()
            {
                //Initializes the sdk face
                FacebookSdk.SdkInitialize(this.ApplicationContext);

                oFacebookService = new FacebookService();
                oFacebookService.mOnProfileChanged += OFacebookService_mOnProfileChanged;
                oFacebookService.StartTracking();


                btnLogin.SetReadPermissions(new List<string> { "user_friends", "public_profile" });
                oICallbackManager = CallbackManagerFactory.Create();
                btnLogin.RegisterCallback(oICallbackManager, this);


            }

            private void OFacebookService_mOnProfileChanged(object sender, OnProfileChangedEventArgs e)
            {
                if (e.mProfile != null)
                {
                    try
                    {
                        Global.GlobalApp.firstName = e.mProfile.FirstName;
                        Global.GlobalApp.lastName = e.mProfile.LastName;
                        Global.GlobalApp.name = e.mProfile.Name;
                        Global.GlobalApp.pictureView = e.mProfile.Id;
                        StartActivity(typeof(RegisterActivity));

                    }
                    catch (Java.Lang.Exception ex)
                    {
                    }
                }
                else
                {
                    Global.GlobalApp.firstName = string.Empty;
                    Global.GlobalApp.lastName = string.Empty;
                    Global.GlobalApp.name = string.Empty;
                    Global.GlobalApp.pictureView = string.Empty;
                }
            }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            oICallbackManager.OnActivityResult(requestCode, (int)resultCode, data);
        }
    }
}

