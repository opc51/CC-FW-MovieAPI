using Microsoft.EntityFrameworkCore;
using Moq;
using MovieAPI.Models;
using MovieAPI.Repository;
using MovieAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MovieTests
{
    [Collection("In Memory Database Collection")]
    public class MovieServiceTesting
    {
        private readonly APIContext _database;
        private readonly MovieService _movieService;

        public MovieServiceTesting(InMemoryDatabaseFixture fixture)
        {
            _database = fixture._database;
            _movieService = new MovieService(_database);
        }

        [Fact]
        public void ServiceThrowsException_NullContextPassed()
        {
            Assert.Throws<ArgumentNullException>(() => new APIContext(null));
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
            var searchResult = _movieService.GetMatchingMovies(new MovieSearchCriteria() { Title = "Super" });
            Assert.Equal(7, searchResult.Count);
            var numberFound = searchResult.Where(x => x.Title.Contains("Super")).Count();
            Assert.Equal(7, numberFound);
        }

        [Theory]
        [InlineData("super")]
        [InlineData("Super")]
        [InlineData("sUPER")]
        public void GetMatchingMoviesShould_IsCaseInsentivie(string title)
        {
            var searchResult = _movieService.GetMatchingMovies(new MovieSearchCriteria() { Title = title });
            Assert.Equal(7, searchResult.Count);
        }


        [Fact]
        public void GetMatchingMoviesShould_FilterOnYear()
        {
            var searchResult = _movieService.GetMatchingMovies(new MovieSearchCriteria() { Year = 2004 });
            Assert.Equal(3, searchResult.Count);
            var numberFound = searchResult.Where(x => x.YearOfRelease == 2004).Count();
            Assert.Equal(3, numberFound);
        }

        [Fact]
        public void GetMatchingMoviesShould_FilterOnComedy()
        {
            const string COMEDY = "Comedy";
            var searchResult = _movieService.GetMatchingMovies(new MovieSearchCriteria() { Genre = COMEDY });
            Assert.Equal(5, searchResult.Count);
            var numberFound = searchResult.Where(x => string.Equals(x.Genre, COMEDY)).Count();
            Assert.Equal(5, numberFound);
        }


        [Fact]
        public void GetMatchingMoviesShould_FilterOnAllCriteria()
        {
            var searchResult = _movieService.GetMatchingMovies(new MovieSearchCriteria()
            {
                Genre = "Romance",
                Title = "Super",
                Year = 2004
            });
            Assert.Single(searchResult);
        }


        [Fact]
        public void AddUpdateShould_UpdateExistingRecord()
        {
            var review = _database.Reviews.FirstAsync().Result;
            var previous = review.Score;
            review.Score = 4;
            var result = _movieService.AddUpdateReview(new AddUpdateReview()
            {
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
            var results = _movieService.GetTopMovies(5);
            Assert.Equal(5, results.Count);
            List<double> scores = new();
            foreach (var result in results)
            {
                scores.Add(result.Rating);
            }
            List<double> expected = new() { 4.5, 3.5, 3.5, 2.5, 2.5 };
            Assert.Equal(expected, scores);
        }

        [Fact]
        public void GetTopFiveMoviesByReviewerShould_GiveCorrectResults()
        {
            var results = _movieService.GetMoviesByReviewer(5, 2);

            Assert.Equal(5, results.Count);

            List<double> scores = new();
            foreach (var result in results)
            {
                scores.Add(result.Rating);
            }

            List<double> expected = new() { 5, 4, 4, 3, 3 };
            Assert.Equal(expected, scores);
        }

        [Fact]
        public void Failed_SaveChanges_ReturnFalse()
        {
            Mock<APIContext> mockContext = new();
            mockContext.Setup(c => c.SaveChanges()).Returns(-1);

            Mock<MovieService> mockMoviesService = new Mock<MovieService>(mockContext.Object);
            Assert.False(mockMoviesService.Object.SaveChanges());
        }

        [Fact]
        public void Successful_SaveChanges_ReturnTrue()
        {
            Mock<APIContext> mockContext = new();
            mockContext.Setup(c => c.SaveChanges()).Returns(3);

            Mock<MovieService> mockMoviesService = new Mock<MovieService>(mockContext.Object);
            Assert.True(mockMoviesService.Object.SaveChanges());
        }
    }
}
