using Android.App;
using Android.Widget;
using Android.OS;
using LostPets.Droid.Activities;

namespace LostPets.Droid
{
    [Activity(Label = "Lost Pets", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        Button button;
        EditText editText;
        TextView txtView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //Set content View
            SetContentView(Resource.Layout.Main);
            //We create the control instances
            button = FindViewById<Button>(Resource.Id.myButton);
            editText = FindViewById<EditText>(Resource.Id.txtEdit);
            txtView = FindViewById<TextView>(Resource.Id.lblNombre);

            button.Click += delegate
            {
                //validate if the edit text is not empty
                if (!string.IsNullOrWhiteSpace(editText.Text))
                {
                    txtView.Text = $"Hola, {editText.Text} Bienvenido";
                    StartActivity(typeof(FacebookActivity));
                }
                else
                    button.Text = "Favor ingrese su nombre!.";
            };

        }
    }
}

