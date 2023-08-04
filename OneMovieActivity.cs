using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using System.Drawing;

namespace MovieDbBrowser
{
    [Activity(Label = "OneMovieActivity", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class OneMovieActivity : AppCompatActivity
    {
        Button btnFavorites;
        ImageView poster;
        TextView txtTitle;
        TextView txtShortInfo;
        TextView txtOverview;
        int movieID;
        bool isFavorited;
        List<GenresLib> genreList;
        MovieData theMovie;
        Color themeColor;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.oneMovieOpenedInformationLayout);
            InitObjects();
            SetMovieInformation();
        }

        async void InitObjects()
        {
            btnFavorites = FindViewById<Button>(Resource.Id.oneMovieOpenedFavBtn);
            btnFavorites.Click += BtnFavorites_Click;
            poster = FindViewById<ImageView>(Resource.Id.oneMovieOpenedPosterImage);
            txtTitle = FindViewById<TextView>(Resource.Id.oneMovieOpenedTitleTxt);
            txtShortInfo = FindViewById<TextView>(Resource.Id.oneMovieOpenedPreviewTxt);
            txtOverview = FindViewById<TextView>(Resource.Id.oneMovieOpenedOverviewTxt);
            movieID = Convert.ToInt32(Intent.GetStringExtra("EXTRA_MOVIE_ID"));
            genreList = new List<GenresLib>();
            genreList = await APIHelper.GetGenreList(genreList);
            themeColor = Color.FromArgb(27, 49, 71);
        }

        private async void BtnFavorites_Click(object sender, EventArgs e)
        {
            string movieNameInUrl = txtTitle.Text.ToLower().Replace(" ", "-");
            Uri uri = new Uri($"https://www.themoviedb.org/movie/{movieID}-{movieNameInUrl}");
            try
            {
                await Browser.OpenAsync(uri, new BrowserLaunchOptions
                {
                    LaunchMode = BrowserLaunchMode.SystemPreferred,
                    TitleMode = BrowserTitleMode.Show,
                    PreferredToolbarColor = themeColor,
                    PreferredControlColor = Color.Violet
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("=====OPEN LINK IN BROWSER ERROR==========");
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                Console.WriteLine("=====OPEN LINK IN BROWSER ERROR END==========");
                return;
            }
        }

        async void SetMovieInformation()
        {
            theMovie = await APIHelper.GetOneMovieFullInformation(genreList, movieID);
            txtTitle.Text = theMovie.Title;
            txtShortInfo.Text = "Duration: " + theMovie.Duration / 60 + " hours " + theMovie.Duration % 60 + " minutes\nRelease year: " + theMovie.ReleaseYear.Year + "\nGenres: " + theMovie.Genres + "\nCompanies: " + theMovie.Companies;
            txtOverview.Text = theMovie.Overview;
            poster.SetImageBitmap(theMovie.Poster);
        }

        public override void OnBackPressed()
        {
            this.Finish();
            base.OnBackPressed();
        }
    }
}