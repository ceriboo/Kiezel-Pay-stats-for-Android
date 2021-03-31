using System;
using System.Net.Http;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using datakiezel;
using Newtonsoft.Json;

namespace kiezelpaywidget
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        string Apikey = "0123456789abcdef0123456789abcdef";
        string url = "https://api.kiezelpay.com/api/merchant/today?offset={0}&key=";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

          //  Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
           // SetSupportActionBar(toolbar);

           // FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
          //  fab.Click += FabOnClick;
            maj("");
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }

        async void maj(string platform)
        {
            int utc = Math.Abs((DateTimeKind.Local - DateTimeKind.Utc) * 60);
            string _url = String.Format(url, utc.ToString()) + Apikey;
            if (platform !="")
            {
                _url +=  "&platform=fitbit";
            }
          
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");

            //HttpClient client = new HttpClient();
            HttpResponseMessage laresponse = new HttpResponseMessage();
            //laresponse.Version = new Version("1.1");
            //laresponse.RequestMessage.Version = new Version("1.1");
            laresponse = await client.GetAsync(_url);
            
            
            if (laresponse.IsSuccessStatusCode)
            {
                var toto = await laresponse.Content.ReadAsStringAsync();
                Console.WriteLine(toto);
                toto = "{\"datakiezel\":" + toto.ToString() + "}";
                


                mydata resultat = JsonConvert.DeserializeObject<mydata>(toto);
                TextView gain = FindViewById<TextView>(Resource.Id.amount);
                gain.Text = resultat.datakiezel.amount.ToString()+"$";
                TextView nbachat = FindViewById<TextView>(Resource.Id.valpurchase);
                nbachat.Text = resultat.datakiezel.purchases.ToString();
                TextView rang = FindViewById<TextView>(Resource.Id.rank);
                rang.Text = resultat.datakiezel.rank.ToString();
            }


        }
	}
}

