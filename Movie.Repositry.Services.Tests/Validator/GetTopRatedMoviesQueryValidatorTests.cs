using FluentValidation.TestHelper;
using Movie.Repository.Services;
using Movie.Repository.Services.TopRatedMovies;

namespace Movie.Repositry.Services.Tests.Validator
{
    public class GetTopRatedMoviesQueryValidatorTests
    {
        private GetTopRatedMoviesQuery _query = null!;
        private GetTopRatedMoviesQueryValidator _validator;

        public GetTopRatedMoviesQueryValidatorTests()
        {
            _validator = new GetTopRatedMoviesQueryValidator();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ErrorWhen_LessThanOne(int number)
        {
            _query = new GetTopRatedMoviesQuery() { NumberOfMovies = number };
            var result = _validator.TestValidate(_query);
            result.ShouldHaveValidationErrorFor(x => x.NumberOfMovies);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        public void Succeeds_When_MoreThanOne(int number)
        {
            _query = new GetTopRatedMoviesQuery() { NumberOfMovies = number };
            var result = _validator.TestValidate(_query);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}