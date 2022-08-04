using MovieAPI.Interfaces;
using MovieAPI.Mediatr;
using MovieAPI.Models;
using MovieAPI.Models.DTOs.Outputs;
using MovieAPI.Models.Entities;
using MovieAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DTO = MovieAPI.Models.DTOs;
using Entity = MovieAPI.Models.Entities;

namespace MovieAPI.Services
{
    /// <summary>
    /// Implements the IMovieService to provide the data retrevial for the MovieAPI
    /// </summary>
    public class MovieService : IMovieService
    {

        private readonly APIContext _data;
        /// <summary>
        /// Constructor that accepts the database context as a parameter
        /// </summary>
        /// <param name="_database"></param>
        public MovieService(APIContext _database)
        {
            _data = _database ?? throw new ArgumentNullException();
        }


        /// <summary>
        /// Adds or updates a movie review.
        /// 
        /// It first checks if the reviewer has already reviewed this movie. 
        /// 
        /// If the reviewer has the existing score is updated in the existing context. If not it adds a new review
        /// 
        /// It then saves the changes in the context.
        /// </summary>
        /// <param name="review"></param>
        /// <returns></returns>
        public bool AddUpdateReview(AddUpdateReview review)
        {
            var findExistingReview = _data.Reviews.Where(r =>
                                                   r.MovieId == review.MovieId
                                                   && r.ReviewerId == review.ReviewerId);

            if (findExistingReview.Any())
            {
                findExistingReview.First().Score = review.Score;
            }
            else
            {
                _data.Reviews.Add(new Review()
                {
                    MovieId = review.MovieId,
                    ReviewerId = review.ReviewerId,
                    Score = review.Score
                });
            }

            return SaveChanges();
        }

        /// <summary>
        /// Entity framework call to SaveChanges
        /// </summary>
        /// <returns>True is returned for successful save, false is returned for failed save</returns>
        public bool SaveChanges() => _data.SaveChanges().CompareTo(-1) == 0 ? false : true;



        /// <summary>
        /// Finds a list of movies that match specific search criteria
        /// </summary>
        /// <param name="sc">The criteria used to find movies. Includes title, year and genre </param>
        /// <returns>A list of movies</returns>
        public List<Entity.Movie> GetMatchingMovies(GetMoviesQuery sc, CancellationToken cancellationToken)
        {
            IQueryable<Entity.Movie> data = _data.Set<Entity.Movie>();

            if (!string.IsNullOrWhiteSpace(sc.Title))
            {
                data = data.Where(x => x.Title.ToLower().Contains(sc.Title.ToLower()));
            }
            if (sc.Year != 0)
            {
                data = data.Where(x => x.YearOfRelease == sc.Year);
            }
            if (!string.IsNullOrWhiteSpace(sc.Genre))
            {
                data = data.Where(x => x.Genre.Contains(sc.Genre));
            }

            return data.ToList();
        }


        /// <summary>
        /// Find a movie based upon the primary key
        /// </summary>
        /// <param name="movieId">The integer pimary key</param>
        /// <returns></returns>
        public Entity.Movie GetMovieById(int movieId)
        {
            return _data.Find<Entity.Movie>(movieId);

        }

        /// <summary>
        /// Find a review by the primary key
        /// </summary>
        /// <param name="reviewerId">integer primary key</param>
        /// <returns></returns>
        public Reviewer GetReviewerById(int reviewerId)
        {
            return _data.Find<Reviewer>(reviewerId);
        }


        /// <summary>
        /// Finds the top 5 rated movies. Rating determined by the scores given by all reviewers
        /// </summary>
        /// <returns>List of MovieResults</returns>
        public List<MovieResultsList> GetTopMovies(int numberOfMovies)
        {
            var combinedMoviesReviews =
                                    from reviews in _data.Reviews
                                    join movies in _data.Movies on reviews.MovieId equals movies.Id
                                    select new
                                    {
                                        movies.Id,
                                        movies.Title,
                                        movies.YearOfRelease,
                                        movies.RunningTime,
                                        movies.Genre,
                                        Rating = reviews.Score
                                    };

            var grouppByMovieId = from cmr in combinedMoviesReviews
                                  group cmr by new
                                  {
                                      cmr.Id,
                                      cmr.Title,
                                      cmr.RunningTime,
                                      cmr.YearOfRelease,
                                      cmr.Genre
                                  }
                                  into movieResults
                                  select new MovieResultsList()
                                  {
                                      MovieId = movieResults.Key.Id,
                                      MovieTitle = movieResults.Key.Title,
                                      Rating = Utilities.RoundToTheNearestHalf(movieResults.Average(x => x.Rating)),
                                      Genres = movieResults.Key.Genre,
                                      YearOfRelease = movieResults.Key.YearOfRelease,
                                      RunningTime = movieResults.Key.RunningTime

                                  };

            return grouppByMovieId.AsQueryable().OrderByDescending(x => x.Rating).ThenBy(x => x.MovieTitle).Take(numberOfMovies).ToList();
        }


        /// <summary>
        /// Find the top 5 movies for a specific reviewer
        /// </summary>
        /// <param name="numberofMovies">The number of movies to return</param>
        /// <param name="reviewerId">The primary key of the reviewer</param>
        /// <returns>List of MovieResults</returns>
        public List<MovieResultsList> GetMoviesByReviewer(int numberofMovies, int reviewerId)
        {

            var combinedMoviesReviews =
                                        (from reviews in _data.Reviews

                                         join movies in _data.Movies on reviews.MovieId equals movies.Id
                                         where reviews.ReviewerId == reviewerId
                                         orderby reviews.Score descending, movies.Title descending
                                         select new MovieResultsList()
                                         {
                                             MovieId = movies.Id,
                                             MovieTitle = movies.Title,
                                             YearOfRelease = movies.YearOfRelease,
                                             RunningTime = movies.YearOfRelease,
                                             Genres = movies.Genre,
                                             Rating = reviews.Score
                                         }).Take(numberofMovies).ToList();

            return combinedMoviesReviews;
        }
    }
}
