using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Support.Design.Widget;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System;
using System.Collections.Generic;
using Android.Content;

namespace MovieDbBrowser
{
    [Activity(Label = "@string/app_name", MainLauncher = true, Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class MainActivity : AppCompatActivity
    {
        BottomNavigationView bottomNavigationView;
        MovieAdapter myAdapter;
        ListView lstData;
        List<oneMoviePreviewData> popMoviesList;
        List<oneMoviePreviewData> favouriteMoviesList;
        List<GenresLib> genreList;
        int operationId;

        void InitObjects()
        {
            bottomNavigationView = FindViewById<BottomNavigationView>(Resource.Id.bottomNavigation);
            bottomNavigationView.NavigationItemSelected += BottomNavigationView_NavigationItemSelected;
            genreList = new List<GenresLib>();
            favouriteMoviesList = new List<oneMoviePreviewData>();
            lstData = FindViewById<ListView>(Resource.Id.lstSomething);
            lstData.ItemClick += LstData_ItemClick;
            lstData.ItemLongClick += LstData_ItemLongClick;
        }

        private void LstData_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            if (favouriteMoviesList.Find(x => x.Title == popMoviesList[e.Position].Title) == null)
            {
                favouriteMoviesList.Add(popMoviesList[e.Position]);
                popMoviesList[e.Position].isFavorited = true;
            }
            else
            {
                if (operationId == 1 || operationId == 2)
                {
                    favouriteMoviesList.Remove(favouriteMoviesList.Find(x => x.Title == popMoviesList[e.Position].Title));
                    if (operationId == 1) UpdateAdapter(1);
                    else UpdateAdapter(2);
                }
                else
                {
                    favouriteMoviesList.RemoveAt(e.Position);
                    UpdateAdapter(3);
                }
                popMoviesList[e.Position].isFavorited = false;
            }
        }

        private void LstData_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            for (int i = 0; i < popMoviesList.Count; i++)
            {
                if (e.Id == i)
                {
                    //Console.WriteLine(popMoviesList[i].Id);
                    Intent intent = new Intent(this, typeof(OneMovieActivity));
                    intent.PutExtra("EXTRA_MOVIE_ID", popMoviesList[i].Id.ToString());
                    StartActivity(intent);
                    break;
                }
            }
        }

        private void BottomNavigationView_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            switch (e.Item.ItemId)
            {
                case Resource.Id.popularTab:
                    operationId = 1;
                    UpdateAdapter(operationId);
                    break;
                case Resource.Id.newTab:
                    operationId = 2;
                    UpdateAdapter(operationId);
                    break;
                case Resource.Id.favoritesTab:
                    operationId = 3;
                    UpdateAdapter(operationId);
                    break;
            }
        }

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            InitObjects();
            genreList = await APIHelper.GetGenreList(genreList);
            popMoviesList = await APIHelper.GetPopularMoviesData(genreList, 1);
            myAdapter = new MovieAdapter(this, popMoviesList);
            lstData.Adapter = myAdapter;
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            return;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private async void UpdateAdapter(int operationId)
        {
            if (operationId == 1 || operationId == 2)
            {
                popMoviesList.Clear();
                popMoviesList = await APIHelper.GetPopularMoviesData(genreList, operationId);
                myAdapter = new MovieAdapter(this, popMoviesList);
                lstData.Adapter = myAdapter;
                myAdapter.NotifyDataSetChanged();
            }
            else
            {
                myAdapter = new MovieAdapter(this, favouriteMoviesList);
                lstData.Adapter = myAdapter;
                myAdapter.NotifyDataSetChanged();
            }

        }
    }
}