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
using Org.Json;
using Entitites;

namespace LostPets.Droid
{
    [Activity(Label = "Lost Pets", Icon = "@mipmap/icon", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class MainActivity : AppCompatActivity, IFacebookCallback, GraphRequest.IGraphJSONObjectCallback
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
                ConfigurateFacebookData();
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


            btnLogin.SetReadPermissions(new List<string>
            {
                "user_friends",
                "public_profile",
                "email"
            });

            ConfigurateFacebookData();

        }

        public void ConfigurateFacebookData()
        {
            GraphRequest oGraphRequest = GraphRequest.NewMeRequest(AccessToken.CurrentAccessToken, this);
            Bundle parameters = new Bundle();
            parameters.PutString("fields", "id,name,birthday,email,address");
            oGraphRequest.Parameters = parameters;
            oGraphRequest.ExecuteAsync();
            oICallbackManager = CallbackManagerFactory.Create();
            btnLogin.RegisterCallback(oICallbackManager, this);
        }

        private void OFacebookService_mOnProfileChanged(object sender, OnProfileChangedEventArgs e)
        {
            if (e.mProfile != null)
            {
                try
                {
                    Global.GlobalApp.facebookProfile.firstName = e.mProfile.FirstName;
                    Global.GlobalApp.facebookProfile.lastName = e.mProfile.LastName;
                    Global.GlobalApp.facebookProfile.name = e.mProfile.Name;
                    Global.GlobalApp.facebookProfile.id = e.mProfile.Id;
                    StartActivity(typeof(RegisterActivity));

                }
                catch (Java.Lang.Exception ex)
                {
                }
            }
            else
            {
                Global.GlobalApp.facebookProfile.firstName = string.Empty;
                Global.GlobalApp.facebookProfile.lastName = string.Empty;
                Global.GlobalApp.facebookProfile.name = string.Empty;
                Global.GlobalApp.facebookProfile.id= string.Empty;
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            oICallbackManager.OnActivityResult(requestCode, (int)resultCode, data);
        }

        public void OnCompleted(JSONObject json, GraphResponse response)
        {
            try
            {
                if (json != null)
                {
                    var _json = Newtonsoft.Json.JsonConvert.DeserializeObject<FacebookProfile>(json.ToString());

                    Global.GlobalApp.facebookProfile = _json;
                    Global.GlobalApp.facebookProfile.id = json.OptString("id");
                    Global.GlobalApp.facebookProfile.name = _json.name;
                    Global.GlobalApp.accessToken = AccessToken.CurrentAccessToken.ToString();
                }
            }
            catch (System.Exception ex)
            {
                StartActivity(typeof(MainActivity));
            }
        }

        protected override void OnDestroy()
        {
            oFacebookService.StopTracking();
            base.OnDestroy();

        }
    }
}

