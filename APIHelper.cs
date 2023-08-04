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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Android.Graphics;
using Xamarin.Essentials;

namespace MovieDbBrowser
{
    class APIHelper
    {
        public static async Task<List<GenresLib>> GetGenreList(List<GenresLib> theList)
        {
            try
            {
                string key = "69cd3782568a49d136cd77c91aecc6f7";
                var genreList = await HTTPHelper.GetDataFromApi($"https://api.themoviedb.org/3/genre/movie/list?api_key={key}&language=en-US");


                for (int i = 0; i < genreList["genres"].Count(); i++)
                {
                    var genre = new GenresLib();
                    genre.Id = Convert.ToInt32(genreList["genres"][i]["id"]);
                    genre.Name = genreList["genres"][i]["name"].ToString();
                    theList.Add(genre);
                }

                return theList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("========GET GENRE LIST=======");
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                Console.WriteLine("========GET GENRE LIST=======");
                return null;
            }
        }

        public static async Task<List<oneMoviePreviewData>> GetPopularMoviesData(List<GenresLib> allGenres, int operationId)
        {
            try
            {
                string key = "69cd3782568a49d136cd77c91aecc6f7";
                string source;
                if (operationId == 1)
                {
                    source = $"https://api.themoviedb.org/3/movie/popular?api_key={key}&language=en-US&page=1";
                }
                else
                {
                    source = $"https://api.themoviedb.org/3/movie/upcoming?api_key={key}&language=en-US&page=1";
                }
                //if (string.IsNullOrWhiteSpace(source)) source = $"https://api.themoviedb.org/3/movie/popular?api_key={key}&language=en-US&page=1";

                var data = await HTTPHelper.GetDataFromApi(source);
                if (data == null) return null;
                var info = new List<oneMoviePreviewData>();

                //Console.WriteLine(data["results"][0]["title"]);

                for (int i = 0; i < data["results"].Count(); i++)
                {
                    var movie = new oneMoviePreviewData();
                    movie.Title = data["results"][i]["title"].ToString();
                    int tempMovieId = Convert.ToInt32(data["results"][i]["id"]);
                    var tempMovieInfo = await HTTPHelper.GetDataFromApi($"https://api.themoviedb.org/3/movie/{tempMovieId}?api_key={key}&language=en-US");
                    int tempRuntime = Convert.ToInt32(tempMovieInfo["runtime"]);
                    movie.Duration = tempRuntime;
                    movie.ReleaseYear = (DateTime)data["results"][i]["release_date"];

                    foreach (var item in allGenres)
                    {
                        var oneMovieGenres = data["results"][i]["genre_ids"];
                        for (int j = 0; j < oneMovieGenres.Count(); j++)
                        {
                            if (Convert.ToInt32(oneMovieGenres[j]) == item.Id)
                            {
                                movie.Genres += item.Name + ", ";
                            }
                        }

                    }
                    movie.Genres = movie.Genres.Remove(movie.Genres.Length - 2);
                    movie.Id = Convert.ToInt32(data["results"][i]["id"]);

                    //Task.Run(async delegate
                    //{
                    //    var x = await HTTPHelper.GetMoviePoster(data["results"][i]["poster_path"].ToString());
                    //    movie.Poster = x != null ? x : BitmapFactory.DecodeResource(Application.Context.Resources, Resource.Drawable.ic_new);
                    //}).Wait();

                    info.Add(movie);
                }



                return info;
            }
            catch (Exception ex)
            {
                Console.WriteLine("======GET POPULAR MOVIE DATA=========");
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                Console.WriteLine("======GET POPULAR MOVIE DATA END=========");
                return null;
            }
        }

        public static async Task<MovieData> GetOneMovieFullInformation(List<GenresLib> allGenres, int movieID)
        {
            try
            {
                string key = "69cd3782568a49d136cd77c91aecc6f7";
                var data = await HTTPHelper.GetDataFromApi($"https://api.themoviedb.org/3/movie/{movieID}?api_key={key}");
                var movie = new MovieData();

                movie.Title = data["title"].ToString();
                movie.Overview = data["overview"].ToString();
                for (int i = 0; i < data["production_companies"].Count(); i++)
                {
                    movie.Companies += data["production_companies"][i]["name"] + ", ";
                }
                movie.Companies = movie.Companies.Remove(movie.Companies.Length - 2);
                movie.Duration = Convert.ToInt32(data["runtime"]);
                foreach (var item in allGenres)
                {
                    var oneMovieGenres = data["genres"];
                    for (int i = 0; i < oneMovieGenres.Count(); i++)
                    {
                        if (Convert.ToInt32(oneMovieGenres[i]["id"]) == item.Id)
                        {
                            movie.Genres += item.Name + ", ";
                        }
                    }
                }
                movie.Genres = movie.Genres.Remove(movie.Genres.Length - 2);
                movie.ReleaseYear = (DateTime)data["release_date"];

                //Task.Run(async delegate
                //{
                //    var x = await HTTPHelper.GetMoviePoster(data["poster_path"].ToString());
                //    movie.Poster = x != null ? x : BitmapFactory.DecodeResource(Application.Context.Resources, Resource.Drawable.ic_new);
                //}).Wait();

                return movie;
            }
            catch (Exception ex)
            {
                Console.WriteLine("=====GET ONE MOVIE FULL INFO==========");
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                Console.WriteLine("=====GET ONE MOVIE FULL INFO END==========");
                return null;
            }
        }

        public static async Task<List<oneMoviePreviewData>> GetFavoriteMoviesData(List<oneMoviePreviewData> allFavMovies, List<GenresLib> allGenres)
        {
            try
            {
                string key = "69cd3782568a49d136cd77c91aecc6f7";

                foreach (var item in allFavMovies)
                {
                    var data = await HTTPHelper.GetDataFromApi($"https://api.themoviedb.org/3/movie/{item.Id}?api_key={key}");
                    var movie = new oneMoviePreviewData();


                    movie.Title = data["title"].ToString();
                    movie.Duration = Convert.ToInt32(data["runtime"]);
                    foreach (var genre in allGenres)
                    {
                        var oneMovieGenres = data["genres"];
                        for (int i = 0; i < oneMovieGenres.Count(); i++)
                        {
                            if (Convert.ToInt32(oneMovieGenres[i]["id"]) == genre.Id)
                            {
                                movie.Genres += genre.Name + ", ";
                            }
                        }
                    }
                    movie.Genres = movie.Genres.Remove(movie.Genres.Length - 2);
                    movie.ReleaseYear = (DateTime)data["release_date"];

                    /*Task.Run(async delegate
                    {
                        var x = await HTTPHelper.GetMoviePoster(data["poster_path"].ToString());
                        movie.Poster = x != null ? x : BitmapFactory.DecodeResource(Application.Context.Resources, Resource.Drawable.ic_new);
                    }).Wait();*/
                }
                return allFavMovies;
            }
            catch (Exception ex)
            {
                Console.WriteLine("=====GET FAVORITE MOVIES DATA==========");
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                Console.WriteLine("=====GET FAVORITE MOVIES DATA END==========");
                return null;
            }
        }
    }
}