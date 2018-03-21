using Android.App;
using Android.Widget;
using Android.OS;

namespace LostPets.Droid
{
    [Activity(Label = "Lost Pets", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        int count = 1;
        Button button;
        EditText editText;
        TextView txtView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);
            //We create the control instances
            button = FindViewById<Button>(Resource.Id.myButton);
            editText = FindViewById<EditText>(Resource.Id.txtEdit);
            txtView = FindViewById<TextView>(Resource.Id.lblNombre);
            //click event
            button.Click += delegate
            {
                if (!string.IsNullOrWhiteSpace(editText.Text))
                    txtView.Text = editText.Text;
                else
                    button.Text = "Favor ingrese su nombre!.";
            };

        }
    }
}

