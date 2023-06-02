using AutoFixture;
using Movie.Respository.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Movie.Repositry.Services.Tests.Entities
{
    public class AddUpdateTests
    {
        private Fixture fixture = new();

        public static IEnumerable<object[]> invalidReviewSubmissions = new List<object[]>()
                                                {
                                                    new object[]{ 0, 0, 1},
                                                    new object[]{ 0, 1, 1},
                                                    new object[]{ 1, 0, 1},
                                                };


        public static IEnumerable<object[]> validReviewSubmissions = new List<object[]>()
                                                {
                                                    new object[]{ 1, 1, 1}
                                                };



        [Theory]
        [InlineData(0)]
        [InlineData(6)]
        [InlineData(7)]
        public void AddUpdateReviewShould_NotAllowsValuesOutside1and5(int score)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new AddUpdateReview() { Score = score });
        }

        [Theory]
        [MemberData(nameof(invalidReviewSubmissions))]
        public void ReviewModelShould_ReturnFalseForInvalidSubmissions(int reviewerId, int movieId, int score)
        {
            AddUpdateReview sut = new() { ReviewerId = reviewerId, MovieId = movieId, Score = score };
            Assert.False(sut.IsValidForSubmission());
        }


        [Theory]
        [MemberData(nameof(validReviewSubmissions))]
        public void ReviewModelShould_ReturnTrueForValidSubmission(int reviewerId, int movieId, int score)
        {
            AddUpdateReview sut = new() { ReviewerId = reviewerId, MovieId = movieId, Score = score };
            Assert.True(sut.IsValidForSubmission());
        }
    }
}
