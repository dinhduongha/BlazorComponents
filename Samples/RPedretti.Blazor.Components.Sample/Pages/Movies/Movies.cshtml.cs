﻿//using Cloudcrate.AspNetCore.Blazor.Browser.Storage;
using Microsoft.AspNetCore.Components;
using RPedretti.Blazor.Components.Sample.Models;
using RPedretti.Blazor.Components.Sample.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RPedretti.Blazor.Components.Sample.Pages.Movies
{
    public class MoviesBase : ComponentBase, IDisposable
    {
        #region Fields

        private Dictionary<int, IEnumerable<MoviePosterModel>> CachedMovies = new Dictionary<int, IEnumerable<MoviePosterModel>>();

        private CancellationTokenSource requestToken;

        #endregion Fields

        #region Properties

        //[Inject] private LocalStorage _localStorage { get; set; }

        [Inject] private IMovieService _movieService { get; set; }

        protected int CurrentPage { get; set; }

        protected bool HasContent { get; set; }

        protected bool Loading { get; set; }

        protected List<MoviePosterModel> Movies { get; set; } = new List<MoviePosterModel>();

        protected int MoviesCount { get; set; }

        protected int PageCount { get; set; }

        protected string SearchMovieTitle { get; set; }

        #endregion Properties

        #region Methods

        protected void ClearMovies()
        {
            Movies.Clear();
            MoviesCount = 0;
            HasContent = false;
            CachedMovies.Clear();
        }

        protected async Task GetMoviesAsync(int page = 1)
        {
            IEnumerable<MoviePosterModel> movies;

            Loading = true;

            Movies.Clear();
            CurrentPage = page;

            StateHasChanged();

            if (!CachedMovies.ContainsKey(page))
            {
                try
                {
                    if (requestToken != null)
                    {
                        requestToken.Cancel();
                    }

                    requestToken = new CancellationTokenSource();

                    var moviesResult = await _movieService.FindMoviesByPattern(SearchMovieTitle, page, requestToken.Token);

                    if (moviesResult.Response)
                    {
                        movies = moviesResult.Search.Select(m => new MoviePosterModel
                        {
                            Id = m.imdbID,
                            Plot = m.Plot,
                            Poster = m.Poster,
                            Title = m.Title
                        });

                        CachedMovies[page] = movies;
                        if (MoviesCount == 0)
                        {
                            MoviesCount = moviesResult.totalResults;
                            HasContent = MoviesCount > 0;
                            PageCount = (int)Math.Ceiling(MoviesCount / 10d);
                        }

                        movies = CachedMovies[page];
                        Movies.AddRange(movies);
                    }
                    else
                    {
                        HasContent = false;
                    }
                }
                catch (OperationCanceledException)
                {
                }
                catch
                {
                    HasContent = false;
                }
            }
            else
            {
                movies = CachedMovies[page];
                Movies.AddRange(movies);
            }

            //_localStorage.SetItem("movies", Movies);
            //_localStorage.SetItem("moviesCount", MoviesCount);
            //_localStorage.SetItem("pageCount", PageCount);
            //_localStorage.SetItem("movieTitle", SearchMovieTitle);

            Loading = false;
            StateHasChanged();
        }

        protected override void OnInit()
        {
            //SearchMovieTitle = _localStorage.GetItem("movieTitle");
            //Movies = _localStorage.GetItem<List<MoviePosterModel>>("movies") ?? new List<MoviePosterModel>();
            //MoviesCount = _localStorage.GetItem<int>("moviesCount");
            //PageCount = _localStorage.GetItem<int>("pageCount");
            //CurrentPage = _localStorage.GetItem<int>("page");
            HasContent = MoviesCount > 0;
        }

        protected async void RequestPage(int page)
        {
            await GetMoviesAsync(page);
            //_localStorage.SetItem("page", page);
            StateHasChanged();
        }

        public void Dispose()
        {
            //_localStorage.RemoveItem("movies");
            //_localStorage.RemoveItem("moviesCount");
            //_localStorage.RemoveItem("pageCount");
            //_localStorage.RemoveItem("movieTitle");
            //_localStorage.RemoveItem("page");
        }

        public void GoToMovie(MoviePosterModel model)
        {
            Console.WriteLine($"Olar filme {model.Title} ({model.Id})");
        }

        public async Task SearchAsync()
        {
            ClearMovies();
            await GetMoviesAsync();
        }

        #endregion Methods
    }
}
