using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.Wearable.Views;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.Wearable.Activity;
using Java.Interop;
using Android.Views.Animations;
using System.Net.Http;
using Newtonsoft.Json;
using datakiezel;

namespace kiezelwear
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : WearableActivity
    {
        string Apikey = "0123456789abcdef0123456789abcdef";
        string url = "https://api.kiezelpay.com/api/merchant/today?offset={0}&key=";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.activity_main);

            maj();
            SetAmbientEnabled();
        }



        async void maj()
        {
            int utc = Math.Abs((DateTimeKind.Local - DateTimeKind.Utc) * 60);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");

            //HttpClient client = new HttpClient();
            HttpResponseMessage laresponse = new HttpResponseMessage();
            //laresponse.Version = new Version("1.1");
            //laresponse.RequestMessage.Version = new Version("1.1");
            laresponse = await client.GetAsync(String.Format(url, utc.ToString()) + Apikey);


            if (laresponse.IsSuccessStatusCode)
            {
                var toto = await laresponse.Content.ReadAsStringAsync();
                Console.WriteLine(toto);
                toto = "{\"datakiezel\":" + toto.ToString() + "}";



                mydata resultat = JsonConvert.DeserializeObject<mydata>(toto);
                TextView gain = FindViewById<TextView>(Resource.Id.amount);
                gain.Text = resultat.datakiezel.amount.ToString() + "$";
                TextView nbachat = FindViewById<TextView>(Resource.Id.valpurchase);
                nbachat.Text = resultat.datakiezel.purchases.ToString();
                TextView rang = FindViewById<TextView>(Resource.Id.rank);
                rang.Text = resultat.datakiezel.rank.ToString();
            }


        }
    }
}


