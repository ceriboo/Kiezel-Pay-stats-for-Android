using System;
using System.Net.Http;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Appwidget;
using Android.Content;

using Android.Widget;
using datakiezel;
using Newtonsoft.Json;
namespace kiezelpaywidget
{
[BroadcastReceiver(Label = "Kiezel Stats")]
[IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
[MetaData("android.appwidget.provider", Resource = "@xml/appwidgetprovider")]
public class AppWidget : AppWidgetProvider
{
        RemoteViews widgetView;
    public async override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
    {
        ComponentName me = new ComponentName(context, Java.Lang.Class.FromType(typeof(AppWidget)).Name);
            widgetView = new RemoteViews(context.PackageName, Resource.Layout.widget);
            string Apikey = "0123456789abcdef0123456789abcdef";
            string url = "https://api.kiezelpay.com/api/merchant/today?offset={0}&key=";
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


                widgetView.SetTextViewText(Resource.Id.amount, resultat.datakiezel.amount.ToString() + "$");

                widgetView.SetTextViewText(Resource.Id.valpurchase, resultat.datakiezel.purchases.ToString());

                widgetView.SetTextViewText(Resource.Id.rank, resultat.datakiezel.rank.ToString());

                widgetView.SetTextViewText(Resource.Id.date, String.Format("{0:HH:mm}", DateTime.Now));

                appWidgetManager.UpdateAppWidget(me, BuildRemoteViews(context, appWidgetIds));

              
            }











        }

    private RemoteViews BuildRemoteViews(Context context, int[] appWidgetIds)
    {
            //widgetView = new RemoteViews(context.PackageName, Resource.Layout.widget);

            // maj();
            RegisterClicks(context, appWidgetIds, widgetView);
            return widgetView;
            
    }


        private void RegisterClicks(Context context, int[] appWidgetIds, RemoteViews widgetView)
        {
            var intent = new Intent(context, typeof(AppWidget));
            intent.SetAction(AppWidgetManager.ActionAppwidgetUpdate);
            intent.PutExtra(AppWidgetManager.ExtraAppwidgetIds, appWidgetIds);

            // Register click event for the Background
            var piBackground = PendingIntent.GetBroadcast(context, 0, intent, PendingIntentFlags.UpdateCurrent);
            widgetView.SetOnClickPendingIntent(Resource.Id.widgetBackground, piBackground);
        }
    }
}