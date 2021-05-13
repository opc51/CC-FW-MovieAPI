using MovieAPI.Models;
using System.Collections.Generic;

namespace MovieAPI.Interfaces
{
    public interface IMovieService
    {
        public List<Movie> GetMatchingMovies(MovieSearchCriteria sc);

        public List<ResultList> GetTopFiveMovies();
        public List<ResultList> GetTopFiveMoviesByReviewer(int reviewerId);

        public Movie GetMovieById(int movieId);

        public Reviewer GetReviewerById(int reviewerId);

        public bool AddUpdateReview(AddUpdateReview review);
    }
}
