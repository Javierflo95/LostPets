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
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common.Apis;
using Android.Gms.Auth.Api;
using Android.Gms.Common;
using Android.Gms.Plus;
using Repository;
using System.Threading.Tasks;

namespace LostPets.Droid
{
    [Activity(Label = "Lost Pets", Icon = "@mipmap/icon", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class MainActivity : AppCompatActivity, IFacebookCallback, GraphRequest.IGraphJSONObjectCallback, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
    {

        LoginButton btnLogin;
        ICallbackManager oICallbackManager;
        FacebookService oFacebookService;
        GoogleApiClient oGoogleApiClient;
        ConnectionResult oConnectionResult;
        SignInButton oSignInButton;
        Button btnRegister;
        EditText txtEmail;
        EditText txtPassword;
        private bool mIntentInProgress;
        private bool mSignInClicked;
        private bool mInfoPopulated;
        public string TAG { get; private set; }

        public void OnCancel()
        {
        }

        public void OnError(FacebookException error)
        {
            //string error = $"Ocurrio un erro al intentar conectar con Facebook {error}";
            Toast.MakeText(this, "Ocurrio un erro al intentar conectar con Faceboo", ToastLength.Short).Show();
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
            btnRegister = FindViewById<Button>(Resource.Id.btnRegister);
            oSignInButton = FindViewById<SignInButton>(Resource.Id.sign_in_button);
            txtPassword = FindViewById<EditText>(Resource.Id.txtPassword);
            txtEmail = FindViewById<EditText>(Resource.Id.txtEmail);

            btnRegister.Click += BtnRegister_Click;

            oSignInButton.Click += OSignInButton_Click;

            btnLogin.Click += delegate
            {
                if (AccessToken.CurrentAccessToken != null && Profile.CurrentProfile != null)
                {
                    ConfigurateFacebookData();
                    StartActivity(typeof(RegisterActivity));
                }
                else
                {
                    ConnectWithFacebook();
                }
            };
        }

        private async void BtnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtPassword.Text) && !string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    Repository.Services.Service oService = new Repository.Services.Service();

                    var owner = await oService.LoginApi(txtEmail.Text, txtPassword.Text);

                    if (owner != null)
                    {
                        Owner.SetInstance(owner);
                        var welcome = $"Bienvenido {owner.firstName} a LostPests ";
                        Toast.MakeText(this, welcome, ToastLength.Long).Show();
                        StartActivity(typeof(RegisterActivity));
                    }
                    else
                        Toast.MakeText(this, "Error en la autenticacion", ToastLength.Long).Show();
                }
                else
                {
                    Toast.MakeText(this, "Favor llenar los campos", ToastLength.Short).Show();
                }
            }
            catch (System.Exception ex)
            {
                Toast.MakeText(this, "Erro al conectar con el servicio" + ex.Message, ToastLength.Short).Show();
            }
        }

        private void OSignInButton_Click(object sender, EventArgs e)
        {
            //oGoogleApiClient = new GoogleApiClient.Builder(Application.Context)
            //                    .AddApi(PlusClass.API)
            //                    .AddScope(PlusClass.ScopePlusProfile)
            //                    .AddScope(PlusClass.ScopePlusLogin)
            //                    .Build();
            ConfigurateGoogleSigin();
            oGoogleApiClient.Connect();
            if (!oGoogleApiClient.IsConnecting)
            {
                mSignInClicked = true;
                ResolveSignInError();
            }
            else if (oGoogleApiClient.IsConnected)
            {
                PlusClass.AccountApi.ClearDefaultAccount(oGoogleApiClient);
                oGoogleApiClient.Disconnect();
            }
        }

        private void ConfigurateGoogleSigin()
        {
            GoogleApiClient.Builder gBuilder = new GoogleApiClient.Builder(this);
            gBuilder.AddConnectionCallbacks(this);
            gBuilder.AddOnConnectionFailedListener(this);
            gBuilder.AddApi(PlusClass.API);
            gBuilder.AddScope(PlusClass.ScopePlusProfile);
            gBuilder.AddScope(PlusClass.ScopePlusLogin);
            oGoogleApiClient = gBuilder.Build();
        }

        private void ConnectWithFacebook()
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

        private void ConfigurateFacebookData()
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
            var fb = Owner.GetInstance();

            if (e.mProfile != null)
            {
                try
                {
                    fb.firstName = e.mProfile.FirstName;
                    fb.lastName = e.mProfile.LastName;
                    fb.facebookId = e.mProfile.Id;
                    StartActivity(typeof(RegisterActivity));

                }
                catch (Java.Lang.Exception ex)
                {
                }
            }
            else
            {
                fb.firstName = string.Empty;
                fb.lastName = string.Empty;
                fb.id = string.Empty;
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            oICallbackManager.OnActivityResult(requestCode, (int)resultCode, data);
            //if (requestCode == 0)
            //{
            //    if (resultCode != Result.Ok)
            //        mSignInClicked = false;

            //    mIntentInProgress = false;

            //    if (!oGoogleApiClient.IsConnecting)
            //        oGoogleApiClient.Connect();
            //}
        }

        public void OnCompleted(JSONObject json, GraphResponse response)
        {
            try
            {
                if (json != null)
                {
                    var _json = Newtonsoft.Json.JsonConvert.DeserializeObject<Owner>(json.ToString());
                    Owner.SetInstance(_json);
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

        public void OnConnected(Bundle connectionHint)
        {
            var person = PlusClass.PeopleApi.GetCurrentPerson(oGoogleApiClient);
            var name = string.Empty;
            if (person != null)
            {
                //GoogleProfile.
                // var Img = person.Image.Url;
                //var imageBitmap = GetImageBitmapFromUrl(Img.Remove(Img.Length - 5));
                //if (imageBitmap != null) ImgProfile.SetImageBitmap(imageBitmap);
            }
        }


        private void ResolveSignInError()
        {
            if (oGoogleApiClient.IsConnecting) return;

            if (oConnectionResult.HasResolution)
            {
                try
                {
                    mIntentInProgress = true;
                    StartIntentSenderForResult(oConnectionResult.Resolution.IntentSender, 0, null, 0, 0, 0);
                }
                catch (Android.Content.IntentSender.SendIntentException io)
                {
                    mIntentInProgress = false;
                    oGoogleApiClient.Connect();
                }
            }
        }

        public void OnConnectionSuspended(int cause)
        {
            throw new NotImplementedException();
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            if (!mIntentInProgress)
            {
                oConnectionResult = result;
                if (mSignInClicked)
                    ResolveSignInError();
            }
        }
    }
}

