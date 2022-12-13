using Microsoft.EntityFrameworkCore;
using MovieAPI.Interfaces;
using MovieAPI.Mediatr;
using MovieAPI.Models;
using MovieAPI.Models.DTOs.Outputs;
using MovieAPI.Models.Entities;
using MovieAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
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
                _data.Reviews.Add(Review.Create(review.MovieId, review.ReviewerId, review.Score));
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
        /// <param name="cancellationToken">Cancellation Token for async operations </param>
        /// <returns>A list of movies</returns>
        public async Task<List<Entity.Movie>> GetMatchingMovies(GetMoviesQuery sc, CancellationToken cancellationToken)
        {
            //var asNumber = ConvertStringToEnumInt(sc.Genre);
            var data = _data.Movies.Select(x => x);
            if (!string.IsNullOrWhiteSpace(sc.Title))
            {
                data = data.Where(x => x.Title.ToLower().Contains(sc.Title.ToLower()));
            }
            if (sc.Year != 0)
            {
                data = data.Where(x => x.YearOfRelease == sc.Year);
            }
            if (sc.Genre != 0)
            {
                data = data.Where(x => x.Genre.Value == sc.Genre);
            }

            return await data.ToListAsync(cancellationToken);
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
            return _data.Movies
                            .OrderByDescending(x => x.GetAverageScore)
                            .Take(numberOfMovies)
                                            .Select(x => new MovieResultsList()
                                            {
                                                MovieId = x.Id,
                                                MovieTitle = x.Title,
                                                YearOfRelease = x.YearOfRelease.Value,
                                                RunningTime = x.YearOfRelease,
                                                Genres = x.Genre.Name,
                                                Rating = x.GetAverageScore
                                            })
                            .ToList();
        }

        /// <summary>
        /// Find the top 5 movies for a specific reviewer
        /// </summary>
        /// <param name="numberofMovies">The number of movies to return</param>
        /// <param name="reviewerId">The primary key of the reviewer</param>
        /// <returns>List of MovieResults</returns>
        public List<MovieResultsList> GetMoviesByReviewer(int numberofMovies, int reviewerId)
        {
            return _data.Reviews.Where(r => r.ReviewerId == reviewerId)
                                    .OrderByDescending(r => r.Score)
                                    .Select(x => new MovieResultsList()
                                    {
                                        MovieId = x.MovieId,
                                        MovieTitle = x.Movie.Title,
                                        YearOfRelease = x.Movie.YearOfRelease.Value,
                                        RunningTime = x.Movie.YearOfRelease,
                                        Genres = x.Movie.Genre.Name,
                                        Rating = x.Score
                                    }
                                    )
                                    .Take(numberofMovies)
                                    .ToList();
        }
    }
}
