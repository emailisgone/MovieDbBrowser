using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using Java.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Console = System.Console;

namespace MovieDbBrowser
{
    class MovieAdapter : BaseAdapter<oneMoviePreviewData>
    {

        Context context;
        List<oneMoviePreviewData> allMovies;

        public MovieAdapter(Context context, List<oneMoviePreviewData> allMovies)
        {
            this.context = context;
            this.allMovies = allMovies;
        }


        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            MovieAdapterViewHolder holder = null;

            if (view != null)
                holder = view.Tag as MovieAdapterViewHolder;

            if (holder == null)
            {
                holder = new MovieAdapterViewHolder();
                var inflater = context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
                //replace with your item and your holder items
                //comment back in
                view = inflater.Inflate(Resource.Layout.oneMoviePreviewDataLayout, parent, false);

                holder.Title = view.FindViewById<TextView>(Resource.Id.titleTxt);
                holder.ShortPreview = view.FindViewById<TextView>(Resource.Id.previewDescriptionTxt);
                holder.Poster = view.FindViewById<ImageView>(Resource.Id.posterImage);
                view.Tag = holder;
            }


            //fill in your items
            holder.Title.Text = allMovies[position].Title;
            holder.ShortPreview.Text = "Duration: " + allMovies[position].Duration / 60 + " hours " + allMovies[position].Duration % 60 + " minutes\nRelease year: " + allMovies[position].ReleaseYear.Year+"\nGenres: "+allMovies[position].Genres;
            if (allMovies[position].isFavorited)
            {
                holder.ShortPreview.Text += "\nFavorited.";
            }
            holder.Poster.SetImageBitmap(allMovies[position].Poster);
            return view;
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get
            {
                return allMovies.Count;
            }
        }


        public override oneMoviePreviewData this[int position]
        {
            get
            {
                return allMovies[position];
            }
        }
    }

    class MovieAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        public TextView Title { get; set; }
        public TextView ShortPreview { get; set; }
        public ImageView Poster { get; set; }
    }
}