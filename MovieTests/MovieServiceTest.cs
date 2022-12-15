using MovieAPI.Mediatr;
using MovieAPI.Models;
using MovieAPI.Models.Enum;
using MovieAPI.Repository;
using MovieAPI.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        [InlineData(6)]
        public void GetMovieByIdShould_GetMoviesThatExist(int movieId)
        {
            var movie = _movieService.GetMovieById(movieId);
            Assert.Equal(movieId.ToString(), movie.Id.ToString());
            Assert.Equal(3, movie.Reviews.Count());
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
            var searchResult = _movieService.GetMatchingMovies(new GetMoviesQuery() { Title = "Super" }, new CancellationToken()).Result;
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
            var searchResult = _movieService.GetMatchingMovies(new GetMoviesQuery() { Title = title }, new CancellationToken()).Result;
            Assert.Equal(7, searchResult.Count);
        }


        [Theory]
        [InlineData(2004, 3)]
        [InlineData(2002, 1)]
        [InlineData(2011, 2)]
        public void GetMatchingMoviesShould_FilterOnYear(int year, int recordCount)
        {
            var searchResult = _movieService.GetMatchingMovies(new GetMoviesQuery() { Year = year }, new CancellationToken()).Result;
            Assert.Equal(recordCount, searchResult.Count);
            var numberFound = searchResult.Where(x => x.YearOfRelease == year).Count();
            Assert.Equal(recordCount, numberFound);
        }

        class MatchingTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { GenreType.Comedy, 3 };
                yield return new object[] { GenreType.Romance, 2 };
                yield return new object[] { GenreType.SuperHero, 2 };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }


        [Theory]
        [ClassData(typeof(MatchingTestData))]
        public void GetMatchingMoviesShould_FilterOnGenre(GenreType genre, int resultCount)
        {
            var searchResult = _movieService.GetMatchingMovies(new GetMoviesQuery() { Genre = genre.Value }, new CancellationToken()).Result;
            Assert.Equal(resultCount, searchResult.Count);
        }

        class SuperHeroData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { GenreType.SuperHero, 2 };
                yield return new object[] { GenreType.Hero, 2 };
                yield return new object[] { GenreType.Heros, 2 };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [ClassData(typeof(SuperHeroData))]
        public void GettingSuperHeroMovies_WithDifferentNames_Suceeds(GenreType genre, int resultCount)
        {
            var searchResult = _movieService.GetMatchingMovies(new GetMoviesQuery() { Genre = genre.Value }, new CancellationToken()).Result;
            Assert.Equal(resultCount, searchResult.Count);
        }

        [Fact]
        public void GetMatchingMoviesShould_FilterOnAllCriteria()
        {
            var searchResult = _movieService.GetMatchingMovies(new GetMoviesQuery()
            {
                Genre = GenreType.Romance.Value,
                Title = "Super",
                Year = 2004
            }, new CancellationToken()).Result;
            Assert.Single(searchResult);
        }


        [Fact]
        public void AddUpdateShould_UpdateExistingRecord()
        {
            var review = _database.Reviews.Where(r => r.Id == 8).FirstOrDefault();
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
            List<double> expected = new() { 4.333333333333333, 3.6666666666666665, 3.6666666666666665, 2.6666666666666665, 2.6666666666666665 };
            Assert.Equal(expected, scores);
        }

        [Fact]
        public void GetTopFiveMoviesByReviewerShould_GiveCorrectResults()
        {
            var results = _movieService.GetMoviesByReviewer(5, 3);
            Assert.Equal(5, results.Count);

            List<double> scores = new();
            foreach (var result in results)
            {
                scores.Add(result.Rating);
            }

            List<double> expected = new() { 5, 5, 2, 1, 1 };//{ 5, 4, 3, 3, 2 };
            Assert.Equal(expected, scores);
        }

        //[Fact]
        //public void Failed_SaveChanges_ReturnFalse()
        //{
        //    Mock<APIContext> mockContext = new();
        //    mockContext.Setup(c => c.SaveChanges()).Returns(-1);

        //    Mock<MovieService> mockMoviesService = new Mock<MovieService>(mockContext.Object);
        //    Assert.False(mockMoviesService.Object.SaveChanges());
        //}

        //[Fact]
        //public void Successful_SaveChanges_ReturnTrue()
        //{
        //    Mock<APIContext> mockContext = new();
        //    mockContext.Setup(c => c.SaveChanges()).Returns(3);

        //    Mock<MovieService> mockMoviesService = new Mock<MovieService>(mockContext.Object);
        //    Assert.True(mockMoviesService.Object.SaveChanges());
        //}
    }
}
