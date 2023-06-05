using Movie.Repository.Services.DTOs.Output;
using Movie.Repository.Services.TopRankedMoviesByReviewer;
using Movie.Repository.Services.TopRatedMovies;
using Movie.Respository.Services;
using Entity = Movie.Domain;

namespace Movie.Repository.Services
{
    /// <summary>
    /// interface that outlines the functionality that will be implemented by the Movies API
    /// </summary>
    public interface IMovieService
    {
        /// <summary>
        /// Find movies according to search criteria
        /// </summary>
        /// <param name="query">Type of <see cref="GetMoviesQuery"/></param>
        /// <param name="cancellationToken">Type of <see cref="CancellationToken"/></param>
        /// <returns>A list of <see cref="Entity.Movie"/></returns>
        public Task<List<Entity.Movie>> GetMatchingMovies(GetMoviesQuery query, CancellationToken cancellationToken); // to do should be output dto not entity


        /// <summary>
        /// Gets the top rated movie across all reviewers
        /// </summary>
        /// <param name="query">Type of <see cref="GetTopRatedMoviesQuery"/></param>
        /// <returns>A list of <see cref="MovieResult"/></returns>
        public Task<List<MovieResult>> GetTopMovies(GetTopRatedMoviesQuery query, CancellationToken cancellationToken);

        /// <summary>
        /// Get the top rated movies for a specific reviewer
        /// </summary>
        /// <param name="query">Typeof <see cref="TopRankedMoviesByReviewerQuery"/></param>
        /// <returns>A list of type <see cref="MovieResult"/></returns>
        public Task<List<MovieResult>> GetMoviesByReviewer(TopRankedMoviesByReviewerQuery query, CancellationToken cancellationToken);

        /// <summary>
        /// Find a specific movie based upon it's primary key
        /// </summary>
        /// <param name="movieId">The primary key of the movie. Type of <see cref="int"/></param>
        /// <returns>A single <see cref="Movie"/> record</returns>
        public Entity.Movie GetMovieById(int movieId);

        /// <summary>
        /// Find a review by the primary key
        /// </summary>
        /// <param name="reviewerId"> <see cref="int"/> value of the Reviewer primary key</param>
        /// <returns>The reviewer of type <see cref="Entity.Reviewer"/></returns>
        public Entity.Reviewer GetReviewerById(int reviewerId);

        /// <summary>
        /// Adds or updates a movie review.
        /// 
        /// It first checks if the reviewer has already reviewed this movie. 
        /// 
        /// If the reviewer has the existing score is updated in the existing context. If not it adds a new review
        /// 
        /// It then saves the changes in the context.
        /// </summary>
        /// <param name="review">The review to be added or updated. Type of <see cref="AddUpdateReview"/></param>
        /// <returns></returns>
        public bool AddUpdateReview(AddUpdateReview review);
    }
}