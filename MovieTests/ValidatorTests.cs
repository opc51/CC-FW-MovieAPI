﻿using FluentValidation.TestHelper;
using MovieAPI.Mediatr;
using Xunit;

namespace MovieTests
{
    public class ValidatorTests
    {
        private GetMoviesQuery? _query;
        private GetMoviesQueryValidator _validator;


        public ValidatorTests()
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
    }
}