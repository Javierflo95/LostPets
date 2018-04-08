using Android.App;
using Android.Widget;
using Android.OS;
using LostPets.Droid.Activities;
using Android.Support.V7.App;

namespace LostPets.Droid
{
    [Activity(Label = "Lost Pets", Icon = "@mipmap/icon", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class MainActivity : AppCompatActivity
    {
        //Button button;
        //EditText editText;
        //TextView txtView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //Set content View
            SetContentView(Resource.Layout.Main);



            //We create the control instances
            //button = FindViewById<Button>(Resource.Id.btnCricle);
            //editText = FindViewById<EditText>(Resource.Id.txtEdit);
            //txtView = FindViewById<TextView>(Resource.Id.lblNombre);

            //button.Click += delegate
            //{

            //    Toast.MakeText(this, "Facebook", ToastLength.Short);

            //    ////validate if the edit text is not empty
            //    //if (!string.IsNullOrWhiteSpace(editText.Text))
            //    //{
            //    //    txtView.Text = $"Hola, {editText.Text} Bienvenido";
            //    //    StartActivity(typeof(FacebookActivity));
            //    //}
            //    //else
            //    //    StartActivity(typeof(ViewMapActivity));
            //};

        }
    }
}

