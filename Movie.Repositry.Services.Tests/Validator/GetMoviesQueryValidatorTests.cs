using FluentValidation.TestHelper;
using Movie.Repository.Services;

namespace Movie.Repositry.Services.Tests.Validator
{
    public class GetMovieQueryValidatorTests
    {
        private GetMoviesQuery _query = null!;
        private GetMoviesQueryValidator _validator;

        public GetMovieQueryValidatorTests()
        {
            _validator = new GetMoviesQueryValidator();
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
        public void MovieQuery_MustHave_NonEmptyFields()
        {
            _query = new GetMoviesQuery()
            {
                Genre = 0,
                Title = string.Empty,
                Year = 0,
            };

            var result = _validator.TestValidate(_query);
            result.ShouldHaveValidationErrorFor(x => x.Title);
            result.ShouldHaveValidationErrorFor(x => x.Year);
            result.ShouldHaveValidationErrorFor(x => x.Genre);
        }
    }
}