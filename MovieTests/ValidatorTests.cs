using AutoFixture;
using FluentValidation.TestHelper;
using MovieAPI.Mediatr;
using Xunit;

namespace MovieTests
{
    public class ValidatorTests
    {
        private GetMoviesQuery _query;
        private GetMoviesQueryValidator _validator;
        private Fixture _fixture;


        public ValidatorTests()
        {
            _validator = new GetMoviesQueryValidator();
            _fixture = new Fixture();
        }

        [Fact]
        public void MovieQuery_MustHave_OneField()
        {
            _query = new GetMoviesQuery();
            var result = _validator.TestValidate(_query);
            result.ShouldHaveValidationErrorFor(x => x.Title);
            result.ShouldHaveValidationErrorFor(x => x.Year);
            result.ShouldHaveValidationErrorFor(x => x.Genre);
        }

        [Fact]
        public void MovieQuery_AcceptsTitleOnly()
        {
            _query = new GetMoviesQuery() { Title = _fixture.Create<string>()};
            var result = _validator.TestValidate(_query);
            result.ShouldNotHaveValidationErrorFor(x => x.Title);
            result.ShouldNotHaveValidationErrorFor(x => x.Year);
            result.ShouldNotHaveValidationErrorFor(x => x.Genre);
        }

        [Fact]
        public void MovieQuery_AcceptsYearOnly()
        {
            _query = new GetMoviesQuery() { Year = _fixture.Create<int>() };
            var result = _validator.TestValidate(_query);
            result.ShouldNotHaveValidationErrorFor(x => x.Title);
            result.ShouldNotHaveValidationErrorFor(x => x.Year);
            result.ShouldNotHaveValidationErrorFor(x => x.Genre);
        }

        [Fact]
        public void MovieQuery_AcceptsGenreOnly()
        {
            _query = new GetMoviesQuery() { Genre = _fixture.Create<int>() };
            var result = _validator.TestValidate(_query);
            result.ShouldNotHaveValidationErrorFor(x => x.Title);
            result.ShouldNotHaveValidationErrorFor(x => x.Year);
            result.ShouldNotHaveValidationErrorFor(x => x.Genre);
        }
    }
}