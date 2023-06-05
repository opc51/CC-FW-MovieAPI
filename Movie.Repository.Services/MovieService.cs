using Microsoft.EntityFrameworkCore;
using Movie.Domain;
using Movie.Repository.Services.DTOs.Output;
using Movie.Repository.Services.TopRankedMoviesByReviewer;
using Movie.Repository.Services.TopRatedMovies;
using Movie.Respository.Services;
using Entity = Movie.Domain;

namespace Movie.Repository.Services
{
    /// <summary>
    /// Implements the IMovieService to provide the data retrevial for the MovieAPI
    /// </summary>
    public class MovieService : IMovieService
    {
        private readonly APIContext _data = null!;

        /// <summary>
        /// Constructor that accepts the database context as a parameter
        /// </summary>
        /// <param name="_database"></param>
        public MovieService(APIContext _database)
        {
            _data = _database ?? throw new ArgumentNullException();
            
        }

        /// <inheritdoc/>
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
                _data.Reviews.Add(Review.Create(review.MovieId, review.ReviewerId, review.Score, null));
            }

            return SaveChanges();
        }

        /// <summary>
        /// Entity framework call to SaveChanges
        /// </summary>
        /// <returns>True is returned for successful save, false is returned for failed save</returns>
        public bool SaveChanges() => _data.SaveChanges().CompareTo(-1) == 0 ? false : true;

        /// <inheritdoc/>
        public async Task<List<Entity.Movie>> GetMatchingMovies(GetMoviesQuery sc, CancellationToken cancellationToken)
        {
            IQueryable<Entity.Movie> data = _data.Movies;
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

        /// <inheritdoc/>
        public Entity.Movie GetMovieById(int movieId)
        {
            return _data.Find<Entity.Movie>(movieId);

        }

        /// <inheritdoc/>
        public Reviewer GetReviewerById(int reviewerId)
        {
            return _data.Find<Reviewer>(reviewerId);
        }

        /// <inheritdoc/>
        public async Task<List<MovieResult>> GetTopMovies(GetTopRatedMoviesQuery query, CancellationToken cancellationToken)
        {
            return await _data.Movies
                            .OrderByDescending(x => x.GetAverageScore)
                            .Take(query.NumberOfMovies)
                                            .Select(x => new MovieResult()
                                            {
                                                MovieId = x.Id,
                                                MovieTitle = x.Title,
                                                YearOfRelease = x.YearOfRelease.Value,
                                                RunningTime = x.YearOfRelease,
                                                Genres = x.Genre.Name,
                                                Rating = x.GetAverageScore
                                            })
                            .ToListAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<List<MovieResult>> GetMoviesByReviewer(TopRankedMoviesByReviewerQuery query, CancellationToken cancellationToken)
        {
            return await _data.Reviews.Where(r => r.ReviewerId == query.ReviewerId)
                                    .OrderByDescending(r => r.Score)
                                    .Take(query.NumberOfMovies)
                                    .Select(x => new MovieResult()
                                    {
                                        MovieId = x.MovieId,
                                        MovieTitle = x.Movie.Title,
                                        YearOfRelease = x.Movie.YearOfRelease.Value,
                                        RunningTime = x.Movie.YearOfRelease,
                                        Genres = x.Movie.Genre.Name,
                                        Rating = x.Score
                                    }
                                    )
                                    .ToListAsync(cancellationToken);
        }
    }
}
