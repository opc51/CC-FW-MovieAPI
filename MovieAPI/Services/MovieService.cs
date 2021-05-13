using MovieAPI.Interfaces;
using MovieAPI.Models;
using MovieAPI.Repository;
using System.Collections.Generic;
using System.Linq;

namespace MovieAPI.Services
{
    public class MovieService : IMovieService
    {

        private readonly APIContext _data;
        public MovieService(APIContext _database)
        {
            _data = _database;

        }

        public bool AddUpdateReview(AddUpdateReview review)
        {
            var findExistingReview = _data.Reviews.Where(r =>
                                                   r.MovieId == review.MovieId
                                                   & r.ReviewerId == review.ReviewerId);

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


            if (_data.SaveChanges() != 1)
            {
                return false;
            }

            return true;
        }

        public List<Movie> GetMatchingMovies(MovieSearchCriteria sc)
        {
            IQueryable<Movie> data = _data.Set<Movie>();

            if (!string.IsNullOrWhiteSpace(sc.Title))
            {
                data = data.Where(x => x.Title.Contains(sc.Title));
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

        public Movie GetMovieById(int movieId)
        {
            return  _data.Find<Movie>(movieId);
            
        }

        public Reviewer GetReviewerById(int reviewerId)
        {
            return _data.Find<Reviewer>(reviewerId);
        }

        public List<ResultList> GetTopFiveMovies()
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
                                  select new ResultList()
                                  {
                                      MovieId = movieResults.Key.Id,
                                      MovieTitle = movieResults.Key.Title,
                                      Rating = Utilities.RoundToTheNearestHalf(movieResults.Average(x => x.Rating)),
                                      Genres = movieResults.Key.Genre,
                                      YearOfRelease = movieResults.Key.YearOfRelease,
                                      RunningTime = movieResults.Key.RunningTime

                                  };
            return grouppByMovieId.AsQueryable().OrderByDescending(x => x.Rating).ThenBy(x => x.MovieTitle).Take(5).ToList();
        }



        public List<ResultList> GetTopFiveMoviesByReviewer(int reviewerId)
        {

            var combinedMoviesReviews =
                                        from reviews in _data.Reviews
                                        join movies in _data.Movies on reviews.MovieId equals movies.Id
                                        where reviews.ReviewerId == reviewerId
                                        select new ResultList()
                                        {
                                            MovieId = movies.Id,
                                            MovieTitle = movies.Title,
                                            YearOfRelease = movies.YearOfRelease,
                                            RunningTime = movies.YearOfRelease,
                                            Genres = movies.Genre,
                                            Rating = reviews.Score
                                        };

            return combinedMoviesReviews.AsQueryable().OrderByDescending(x => x.Rating)
                                                            .ThenBy(x => x.MovieTitle).Take(5).ToList();
        }
    }
}
