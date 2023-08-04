using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieDbBrowser
{
    class oneMoviePreviewData
    {
        public string Title { get; set; }
        public int Duration { get; set; }
        public DateTime ReleaseYear { get; set; }
        public string Genres { get; set; }
        public Bitmap Poster { get; set; }
        public int Id { get; set; }
        public bool isFavorited { get; set; }
    }
}