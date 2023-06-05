using Movie.Repository;
using Movie.Domain.Enum;
using Movie.Repository.Services;
using Movie.Repository.Services.Tests.ContextSharing;
using Movie.Respository.Services;
using System.Collections;

namespace Movie.Repositry.Services.Tests
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

        [Fact]
        public void ServiceThrowsException_NullContextPassed()
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Assert.Throws<ArgumentNullException>(() => new APIContext(null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
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
            movie.Should().NotBeNull();
            movieId.Should().Be(movie.Id);
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

        [Fact]
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

        [Theory]
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
            var original = _fixture._database.Reviews.Where(r => r.Id == 8).First();
            original.Should().NotBeNull();

            original.Score = 4;

            var result = _movieService.AddUpdateReview(new AddUpdateReview()
            {
                MovieId = original.MovieId,
                ReviewerId = original.ReviewerId,
                Score = original.Score
            });
            result.Should().BeTrue();
            var now = _fixture._database.Reviews.Where(r => r.Id == 8).First();
            now.Should().NotBeNull();
            now.Score.Should().Be(original.Score);
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

        [Fact] // this test has isues when run alone- other tests are changing the seeddata
        public void GetTopFiveMoviesShould_GiveCorrectResult()
        {
            var results = _movieService.GetTopMovies(5);
            results.Should().NotBeNull();
            results.Count.Should().Be(5);

            var scores = results.Select(x => x.Rating).OrderByDescending(x => x).ToList();

            List<double>? expected = new() { 4.333333333333333, 3.6666666666666665, 3.6666666666666665, 2.6666666666666665, 2.6666666666666665 };

            scores.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void GetTopFiveMoviesByReviewerShould_GiveCorrectResults()
        {
            var results = _movieService.GetMoviesByReviewer(5, 3);
            results.Count.Should().Be(5);

            var scores = results.Select(x => x.Rating).OrderByDescending(x => x).ToList();

            List<double> expected = new() { 5, 5, 2, 1, 1 };
            expected.Should().BeEquivalentTo(scores);
        }
    }
}
