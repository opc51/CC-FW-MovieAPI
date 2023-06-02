using Movie.Repository;
using Movie.Repository.Entities.Enum;
using Movie.Repository.Services;
using Movie.Respository.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FluentAssertions;
using Xunit;
using Movie.Repository.Services.Tests.ContextSharing;

namespace Movie.Repository.Services.Tests
{
    [Collection("In Memory Database Collection")]
    public class MovieServiceTesting
    {
        private InMemoryDatabaseFixture _fixture;

        private readonly MovieService _movieService;

        public MovieServiceTesting(InMemoryDatabaseFixture fixture)
        {
            _fixture = fixture;
            _movieService = new MovieService(fixture._database);
        }

        //[OneTimeTearDown] 
        //public void TearDown()
        //{
        //    // _database.Dispose();
        //}

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
            movieId.ToString().Should().Be(movie.Id.ToString());
            movie.Reviews.Count().Should().Be(3);
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
            reviewerId.ToString().Should().Be(reviewer.Id.ToString());
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

        [Theory]
        [InlineData]
        public void GetMatchingMoviesShould_FilterOnTitle()
        {
            var searchResult = _movieService.GetMatchingMovies(new GetMoviesQuery() { Title = "Super" }, new CancellationToken()).Result;
            searchResult.Count.Should().Be(7);
            var numberFound = searchResult.Where(x => x.Title.Contains("Super")).Count();
            numberFound.Should().Be(7);
        }

        [Theory]
        [InlineData("super")]
        [InlineData("Super")]
        [InlineData("sUPER")]
        public void GetMatchingMoviesShould_IsCaseInsentivie(string title)
        {
            var searchResult = _movieService.GetMatchingMovies(new GetMoviesQuery() { Title = title }, new CancellationToken()).Result;
            searchResult.Count.Should().Be(7);
        }

        [Theory]
        [InlineData(2004, 3)]
        [InlineData(2002, 1)]
        [InlineData(2011, 2)]
        public void GetMatchingMoviesShould_FilterOnYear(int year, int recordCount)
        {
            var searchResult = _movieService.GetMatchingMovies(new GetMoviesQuery() { Year = year }, new CancellationToken()).Result;
            recordCount.Should().Be(searchResult.Count);
            var numberFound = searchResult.Where(x => x.YearOfRelease == year).Count();
            recordCount.Should().Be(numberFound);
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
            resultCount.Should().Be(searchResult.Count);
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

        [ClassData(typeof(SuperHeroData))]
        public void GettingSuperHeroMovies_WithDifferentNames_Suceeds(GenreType genre, int resultCount)
        {
            var searchResult = _movieService.GetMatchingMovies(new GetMoviesQuery() { Genre = genre.Value }, new CancellationToken()).Result;
            resultCount.Should().Be(searchResult.Count);
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
            searchResult.Count.Should().Be(1);
        }

        [Fact]
        public void AddUpdateShould_UpdateExistingRecord()
        {
            var review = _fixture._database.Reviews.Where(r => r.Id == 8).FirstOrDefault();
            var previous = review.Score;
            review.Score = 4;
            var result = _movieService.AddUpdateReview(new AddUpdateReview()
            {
                MovieId = review.MovieId,
                ReviewerId = review.ReviewerId,
                Score = review.Score
            });
            result.Should().BeTrue();
            previous.Should().Be(review.Score);
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
            result.Should().BeTrue();
        }

        [Fact]
        public void GetTopFiveMoviesShould_GiveCorrectResult()
        {
            var results = _movieService.GetTopMovies(5);
            results.Count.Should().Be(5);

            List<double> scores = new();
            foreach (var result in results)
            {
                scores.Add(result.Rating);
            }
            List<double> expected = new() { 4.333333333333333, 3.6666666666666665, 3.6666666666666665, 2.6666666666666665, 2.6666666666666665 };
            expected.Should().BeEquivalentTo(scores);
        }

        [Fact]
        public void GetTopFiveMoviesByReviewerShould_GiveCorrectResults()
        {
            var results = _movieService.GetMoviesByReviewer(5, 3);
            results.Count.Should().Be(5);

            List<double> scores = new();
            foreach (var result in results)
            {
                scores.Add(result.Rating);
            }

            List<double> expected = new() { 5, 5, 2, 1, 1 };//{ 5, 4, 3, 3, 2 };
            expected.Should().BeEquivalentTo(scores);
        }
    }
}
