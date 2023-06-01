using Movie.Repository;
using Movie.Repository.Entities.Enum;
using Movie.Repository.Services;
using Movie.Respository.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using FluentAssertions;

namespace MovieTests
{

    public class MovieServiceTesting : InMemoryDatabaseFixture
    {
        private MovieService _movieService;

        //public MovieServiceTesting()
        //{
        //    _movieService = new MovieService(_database);
        //}

        [SetUp] 
        public void SetUp()
        {
            _movieService = new MovieService(_database);
        }

        [TearDown] public void TearDown()
        {
            _movieService = null;
        }

        [Test]
        public void ServiceThrowsException_NullContextPassed()
        {
            Assert.Throws<ArgumentNullException>(() => new APIContext(null));
        }

        // [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void GetMovieByIdShould_GetMoviesThatExist(int movieId)
        {
            var movie = _movieService.GetMovieById(movieId);
            Assert.That(movieId.ToString(), Is.EqualTo(movie.Id.ToString()));
            Assert.That(3, Is.EqualTo(movie.Reviews.Count()));
        }

        [TestCase(11)]
        [TestCase(12)]
        [TestCase(31)]
        [TestCase(42)]
        [TestCase(52)]
        public void GetMovieByIdShould_NotGetMoviesThatDoNotExist(int movieId)
        {
            var movie = _movieService.GetMovieById(movieId);
            Assert.Null(movie);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetReviewerByIdShould_GetReviewersThatExist(int reviewerId)
        {
            var reviewer = _movieService.GetReviewerById(reviewerId);
            Assert.That(reviewerId.ToString(), Is.EqualTo(reviewer.Id.ToString()));
        }

        [TestCase(11)]
        [TestCase(12)]
        [TestCase(6)]

        public void GetReviewerByIdShould_NotGetReviewersThatDoNotExist(int reviewerId)
        {
            var reviewer = _movieService.GetReviewerById(reviewerId);
            Assert.Null(reviewer);
        }

        [Test]
        public void GetMatchingMoviesShould_FilterOnTitle()
        {
            var searchResult = _movieService.GetMatchingMovies(new GetMoviesQuery() { Title = "Super" }, new CancellationToken()).Result;
            Assert.That(searchResult.Count, Is.EqualTo(7));
            var numberFound = searchResult.Where(x => x.Title.Contains("Super")).Count();
            Assert.That(numberFound, Is.EqualTo(7));
        }


        [TestCase("super")]
        [TestCase("Super")]
        [TestCase("sUPER")]
        public void GetMatchingMoviesShould_IsCaseInsentivie(string title)
        {
            var searchResult = _movieService.GetMatchingMovies(new GetMoviesQuery() { Title = title }, new CancellationToken()).Result;
            Assert.That(searchResult.Count, Is.EqualTo(7));
        }

        [TestCase(2004, 3)]
        [TestCase(2002, 1)]
        [TestCase(2011, 2)]
        public void GetMatchingMoviesShould_FilterOnYear(int year, int recordCount)
        {
            var searchResult = _movieService.GetMatchingMovies(new GetMoviesQuery() { Year = year }, new CancellationToken()).Result;
            Assert.That(recordCount, Is.EqualTo(searchResult.Count));
            var numberFound = searchResult.Where(x => x.YearOfRelease == year).Count();
            Assert.That(recordCount, Is.EqualTo(numberFound));
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

        [TestCaseSource(typeof(MatchingTestData))]
        public void GetMatchingMoviesShould_FilterOnGenre(GenreType genre, int resultCount)
        {
            var searchResult = _movieService.GetMatchingMovies(new GetMoviesQuery() { Genre = genre.Value }, new CancellationToken()).Result;
            Assert.That(resultCount, Is.EqualTo(searchResult.Count));
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

        [TestCaseSource(typeof(SuperHeroData))]
        public void GettingSuperHeroMovies_WithDifferentNames_Suceeds(GenreType genre, int resultCount)
        {
            var searchResult = _movieService.GetMatchingMovies(new GetMoviesQuery() { Genre = genre.Value }, new CancellationToken()).Result;
            Assert.That(resultCount, Is.EqualTo(searchResult.Count));
        }

        [Test]
        public void GetMatchingMoviesShould_FilterOnAllCriteria()
        {
            var searchResult = _movieService.GetMatchingMovies(new GetMoviesQuery()
            {
                Genre = GenreType.Romance.Value,
                Title = "Super",
                Year = 2004
            }, new CancellationToken()).Result;
            Assert.That(searchResult.Count, Is.EqualTo(1));
        }

        [Test]
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
            result.Should().BeTrue();
            Assert.That(previous, Is.Not.EqualTo(review.Score));
        }

        [Test]
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

        [Test]
        public void GetTopFiveMoviesShould_GiveCorrectResult()
        {
            var results = _movieService.GetTopMovies(5);
            Assert.That(results.Count, Is.EqualTo(5));

            List<double> scores = new();
            foreach (var result in results)
            {
                scores.Add(result.Rating);
            }
            List<double> expected = new() { 4.333333333333333, 3.6666666666666665, 3.6666666666666665, 2.6666666666666665, 2.6666666666666665 };
            CollectionAssert.AreEqual(expected, scores);
        }

        [Test]
        public void GetTopFiveMoviesByReviewerShould_GiveCorrectResults()
        {
            var results = _movieService.GetMoviesByReviewer(5, 3);
            Assert.That(results.Count, Is.EqualTo(5));

            List<double> scores = new();
            foreach (var result in results)
            {
                scores.Add(result.Rating);
            }

            List<double> expected = new() { 5, 5, 2, 1, 1 };//{ 5, 4, 3, 3, 2 };
            Assert.That(expected, Is.EqualTo(scores));
        }
    }
}
