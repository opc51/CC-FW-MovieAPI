using Movie.Repository;
using Movie.Domain.Enum;
using Movie.Repository.Services;
using Movie.Repository.Services.Tests.ContextSharing;
using Movie.Respository.Services;
using System.Collections;
using Movie.Repository.Services.TopRatedMovies;
using Movie.Domain;
using Movie.Repository.Services.TopRankedMoviesByReviewer;

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

        [Fact] // this test has isues when run alone- other tests are changing the seeddata
        public void GetTopFiveMoviesShould_GiveCorrectResult()
        {
            var query = new GetTopRatedMoviesQuery() { NumberOfMovies = 5 };

            var results = _movieService.GetTopMovies(query, new CancellationToken()).Result;
            results.Should().NotBeNull();
            results.Count().Should().Be(5);

            var scores = results.Select(x => x.Rating).OrderByDescending(x => x).ToList();

            List<double>? expected = new() { 4.333333333333333, 3.6666666666666665, 3.6666666666666665, 2.6666666666666665, 2.6666666666666665 };

            scores.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void GetTopFiveMoviesByReviewerShould_GiveCorrectResults()
        {
            var query = new TopRankedMoviesByReviewerQuery()
            {
                NumberOfMovies = 5,
                ReviewerId = 3
            };
            var results = _movieService.GetMoviesByReviewer(query, new CancellationToken());
            results.Result.Count().Should().Be(5);

            var scores = results.Result.Select(x => x.Rating).OrderByDescending(x => x).ToList();

            List<double> expected = new() { 5, 5, 2, 1, 1 };
            expected.Should().BeEquivalentTo(scores);
        }
    }
}
