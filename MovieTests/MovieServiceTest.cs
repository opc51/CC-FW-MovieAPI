using Microsoft.EntityFrameworkCore;
using MovieAPI.Repository;
using MovieAPI.Services;
using MovieAPI.Models;
using Xunit;
using System.Collections.Generic;

namespace MovieTests
{
    public class MovieServiceTesting
    {

        private readonly APIContext _database;
        private readonly MovieService _movieService;


        public MovieServiceTesting()
        {
            DbContextOptions options = new DbContextOptionsBuilder<APIContext>()
                                        .UseInMemoryDatabase("MovieDatabase").Options;
            _database = new APIContext(options);
            _movieService = new MovieService(_database);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void GetMovieByIdShould_GetMoviesThatExist(int movieId)
        {
            var movie = _movieService.GetMovieById(movieId);
            Assert.Equal(movieId.ToString(), movie.Id.ToString());
        }

        [Theory]
        [InlineData(11)]
        [InlineData(12)]
        [InlineData(31)]
        [InlineData(42)]
        [InlineData(52)]
        public void GetMovieByIdShould_NotGetMoviesThatDoNotExist(int movieId)
        {
            var movie = _movieService.GetMovieById(movieId);
            Assert.Null(movie);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void GetReviewerByIdShould_GetReviewersThatExist(int reviewerId)
        {
            var reviewer = _movieService.GetReviewerById(reviewerId);
            Assert.Equal(reviewerId.ToString(), reviewer.Id.ToString());
        }

        [Theory]
        [InlineData(11)]
        [InlineData(12)]
        [InlineData(6)]

        public void GetReviewerByIdShould_NotGetReviewersThatDoNotExist(int reviewerId)
        {
            var reviewer = _movieService.GetReviewerById(reviewerId);
            Assert.Null(reviewer);
        }

        [Fact]
        public void GetMatchingMoviesShould_FilterOnTitle()
        {
            var searchResult =_movieService.GetMatchingMovies(new MovieSearchCriteria() { Title = "Super" });
            Assert.Equal(7, searchResult.Count);
            foreach (var item in searchResult)
            {
                Assert.True(item.Title.Contains("Super"));
            }
        }

        [Fact]
        public void GetMatchingMoviesShould_FilterOnYear()
        {
            var searchResult = _movieService.GetMatchingMovies(new MovieSearchCriteria() { Year = 2004 });
            Assert.Equal(3, searchResult.Count);
            foreach (var item in searchResult)
            {
                Assert.Equal(item.YearOfRelease, 2004);
            }
        }

        [Fact]
        public void GetMatchingMoviesShould_FilterOnComedy()
        {
            var searchResult = _movieService.GetMatchingMovies(new MovieSearchCriteria() {  Genre = "Comedy" });
            Assert.Equal(5, searchResult.Count);
            foreach (var item in searchResult)
            {
                Assert.True(string.Equals(item.Genre, "Comedy"));
            }
        }


        [Fact]
        public void GetMatchingMoviesShould_FilterOnAllCriteria()
        {
            var searchResult = _movieService.GetMatchingMovies(new MovieSearchCriteria() 
                                                                { 
                                                                    Genre = "Romance" ,
                                                                    Title = "Super",
                                                                    Year = 2004
                                                                 });
            Assert.Equal(1, searchResult.Count);
        }


        [Fact]
        public void AddUpdateShould_UpdateExistingRecord()
        {
            var review = _database.Reviews.FirstAsync().Result;
            var previous = review.Score;
            review.Score = 4;
            var result = _movieService.AddUpdateReview(new AddUpdateReview() { 
                                             MovieId = review.MovieId,
                                             ReviewerId = review.ReviewerId,
                                             Score = review.Score
                                          });
            Assert.True(result);
            Assert.NotEqual(previous, review.Score);
        }


        [Fact]
        public void AddUpdateShould_CreateNewRecord()
        {
            var result = _movieService.AddUpdateReview(new AddUpdateReview()
            {
                MovieId = 7,
                ReviewerId = 1,
                Score = 5
            });
            Assert.True(result);
        }


        [Fact]
        public void GetTopFiveMoviesShould_GiveCorrectResult()
        {
            var results = _movieService.GetTopFiveMovies();
            Assert.Equal(5, results.Count);
            List<double> scores = new();
            foreach (var result in results)
            {
                scores.Add(result.Rating);
            }
            List<double> expected = new() { 4.5,3.5,3.5,2.5,2.5 };
            Assert.Equal(expected, scores);
        }

        [Fact]
        public void GetTopFiveMoviesByReviewerShould_GiveCorrectResults()
        {
            var results = _movieService.GetTopFiveMoviesByReviewer(2);

            Assert.Equal(5, results.Count);

            List<double> scores = new();
            foreach (var result in results)
            {
                scores.Add(result.Rating);
            }

            List<double> expected = new() { 5,4,4,3,3 };
            Assert.Equal(expected, scores);
        }
    }
}
