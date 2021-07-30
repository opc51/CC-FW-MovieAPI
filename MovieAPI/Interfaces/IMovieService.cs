using MovieAPI.Models;
using System.Collections.Generic;

namespace MovieAPI.Interfaces
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
        /// <returns>A list of movies</returns>
        public List<Movie> GetMatchingMovies(MovieSearchCriteria sc);


        /// <summary>
        /// Get the top 5 highest rated movies
        /// </summary>
        /// <returns></returns>
        public List<MovieResultsList> GetTopFiveMovies();


        /// <summary>
        /// Get the top 5 rated movies for a specific reviewer
        /// </summary>
        /// <param name="reviewerId">The primary key of the reviewer</param>
        /// <returns>A list of Movie Results</returns>
        public List<MovieResultsList> GetTopFiveMoviesByReviewer(int reviewerId);
        

        /// <summary>
        /// Find a specific movie based upon it's primary key
        /// </summary>
        /// <param name="movieId">The primary key of the movie</param>
        /// <returns>A single movie record</returns>
        public Movie GetMovieById(int movieId);
        
        
        /// <summary>
        /// Find a reviewer based upon it's primary key
        /// </summary>
        /// <param name="reviewerId">Th eprimary key of the reviewer</param>
        /// <returns>A single reviewer</returns>
        public Reviewer GetReviewerById(int reviewerId);
        
        
        /// <summary>
        /// Add or update a movie review
        /// </summary>
        /// <param name="review">A movie review</param>
        /// <returns>A boolean success flag</returns>
        public bool AddUpdateReview(AddUpdateReview review);
    }
}
