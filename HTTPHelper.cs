using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Android.Graphics;

namespace MovieDbBrowser
{
    class HTTPHelper
    {
        public static async Task<JContainer> GetDataFromApi(string source)
        {
            try
            {
                HttpClient client = new HttpClient();
                var json = await client.GetStringAsync(source);
                if (json == null) return null;
                return JsonConvert.DeserializeObject<JContainer>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("=======GET DATA FROM API ERROR========");
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                Console.WriteLine("=======GET DATA FROM API ERROR END========");
                return null;
            }
        }

        public static async Task<Bitmap> GetMoviePoster(string posterPath)
        {
            try
            {
                string path = $"https://image.tmdb.org/t/p/original{posterPath}";
                Java.Net.URL url = new Java.Net.URL(path);
                var img = await BitmapFactory.DecodeStreamAsync(url.OpenStream());
                return img;
            }
            catch (Exception ex)
            {
                Console.WriteLine("========GET IMAGE ERROR=======");
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                Console.WriteLine("========GET IMAGE ERROR END=======");
                return null;
            }
        }
    }
}