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
    }
}