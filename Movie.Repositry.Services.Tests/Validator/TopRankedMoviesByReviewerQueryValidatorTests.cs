using FluentValidation.TestHelper;
using Movie.Repository.Services.TopRankedMoviesByReviewer;

namespace Movie.Repositry.Services.Tests.Validator
{
    public class TopRankedMoviesByReviewerQueryValidatorTests
    {
        public TopRankedMoviesByReviewerQuery _query = null!;
        public TopRankedMoviesByReviewerQueryValidator _validator;

        public TopRankedMoviesByReviewerQueryValidatorTests()
        {
            _validator = new TopRankedMoviesByReviewerQueryValidator();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void IdAndNumber_MustBeAboveZero(int input)
        {
            _query = new TopRankedMoviesByReviewerQuery()
            {
                ReviewerId = input,
                NumberOfMovies = input,
            };
            
            var sut = _validator.TestValidate(_query);
            sut.ShouldHaveValidationErrorFor(x => x.NumberOfMovies);
            sut.ShouldHaveValidationErrorFor(x => x.ReviewerId);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        public void Succeeds_When_MoreThanOne(int input)
        {
            _query = new TopRankedMoviesByReviewerQuery()
            {
                ReviewerId = input,
                NumberOfMovies = input,
            };

            var sut =_validator.TestValidate(_query);
            sut.ShouldNotHaveAnyValidationErrors();
        }
    }
}
