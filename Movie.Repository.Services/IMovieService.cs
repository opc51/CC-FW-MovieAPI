using Movie.Repository.Services.DTOs.Output;
using Movie.Respository.Services;
using Entity = Movie.Repository.Entities;

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
        /// <param name="sc">Movie Search criteria</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A list of movies</returns>
        public Task<List<Entity.Movie>> GetMatchingMovies(GetMoviesQuery sc, CancellationToken cancellationToken);


        /// <summary>
        /// Gets the top rated movie across all reviewers
        /// </summary>
        /// <param name="NumberOfMovies">The number of movies required. Type of <see cref="int"/></param>
        /// <returns>A list of <see cref="MovieResult"/></returns>
        public List<MovieResult> GetTopMovies(int NumberOfMovies);


        /// <summary>
        /// Get the top rated movies for a specific reviewer
        /// </summary>
        /// <param name="numberOfMovies">The number of top ranked movies required</param>
        /// <param name="reviewerId">The primary key of the reviewer</param>
        /// <returns>A list of type <see cref="MovieResult"/></returns>
        public List<MovieResult> GetMoviesByReviewer(int numberOfMovies, int reviewerId);


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
