using AutoFixture;
using Movie.Repository.Services;
using Movie.Respository.Services;
using Xunit;

namespace Movie.Repositry.Services.Tests.Entities
{
    public class MovieSearchCriteriaTests
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        public static IEnumerable<object[]> invalidSearchCriteria = new List<object[]>()
                                                {
                                                    new object[] { null,  null, 0 },

                                                    new object[] { "", null,  0 },
                                                    new object[] { null, "",  0 },
                                                    new object[] { "", "",  0 },
                                                };

        public static IEnumerable<object[]> validSearchCriteria = new List<object[]>()
                                                {
                                                    new object[] { "Move 1", null, 0 },
                                                    new object[] { "Movie 1 ", "",  0 },
                                                    new object[] { "Movie 1 ", "genre",  0 },
                                                    new object[] { "Movie 1 ", "",  1 },
                                                    new object[] { null, "Genre 1",  0 },
                                                    new object[] { "", "Genre 1",  0 },
                                                    new object[] { "Movie 1", "Genre 1",  1 },
                                                };
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        private Fixture fixture = new();

        [Theory]
        [MemberData(nameof(invalidSearchCriteria))]
        public void MovieSearchCriteriaShould_beInvalidWithNullData(string title, string genre, int year)
        {
            MovieSearchCriteria sut = new() { Title = title, Genre = genre, Year = year };
            Assert.False(sut.IsValid());
        }


        [Theory]
        [MemberData(nameof(validSearchCriteria))]
        public void MovieSearchCriteriaShould_beValidWithGoodData(string title, string genre, int year)
        {
            MovieSearchCriteria sut = new() { Title = title, Genre = genre, Year = year };
            Assert.True(sut.IsValid());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void AddUpdateReviewShould_AllowsValuesBetween1and5(int score)
        {
            AddUpdateReview sut = new()
            {
                MovieId = fixture.Create<int>(),
                ReviewerId = fixture.Create<int>(),
                Score = score
            };
            score.Should().Be(sut.Score);
        }
    }
}
